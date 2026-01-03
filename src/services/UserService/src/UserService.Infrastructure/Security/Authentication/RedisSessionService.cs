
using System;
using System.Text.Json;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Infrastructure.Exceptions.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class RedisSessionService(
    IConnectionMultiplexer connectionMultiplexer,
    RedisSettings settings
) : ISessionService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly RedisSettings _settings = settings;

    public async Task CreateSessionAsync(
        SessionData session,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var payload = JsonSerializer.Serialize(session, jsonOptions);

        var expiry = session.RefreshTokenExpiresAt - DateTimeOffset.UtcNow;
        if (expiry <= TimeSpan.Zero) expiry = TimeSpan.FromMinutes(1);

        var sessionKey = GetSessionKey(session.SessionId);
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var transaction = _database.CreateTransaction();

        _ = transaction.StringSetAsync(sessionKey, payload, expiry);
        _ = transaction.SetAddAsync(userSessionsKey, session.SessionId);
        _ = transaction.KeyExpireAsync(userSessionsKey, expiry);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task UpdateSessionAsync(
        SessionData session,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var payload = JsonSerializer.Serialize(session, jsonOptions);

        var expiry = session.RefreshTokenExpiresAt - DateTimeOffset.UtcNow;
        if (expiry <= TimeSpan.Zero) expiry = TimeSpan.FromMinutes(1);

        var sessionKey = GetSessionKey(session.SessionId);
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var transaction = _database.CreateTransaction();

        _ = transaction.StringSetAsync(sessionKey, payload, expiry);
        _ = transaction.KeyExpireAsync(userSessionsKey, expiry);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task<SessionData?> GetSessionAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        var payload = await _database.StringGetAsync(key);

        if (payload.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<SessionData>(payload!, jsonOptions)!;
    }

    public async Task<bool> SessionExistAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        return await _database.KeyExistsAsync(key);
    }

    public async Task InvalidateSessionAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var sessionKey = GetSessionKey(sessionId);

        var payload = await _database.StringGetAsync(sessionKey);

        if (payload.IsNullOrEmpty) return;

        var session = JsonSerializer.Deserialize<SessionData>(payload!, jsonOptions);

        var userSessionsKey = GetUserSessionsKey(session!.UserId);

        var transaction = _database.CreateTransaction();

        _ = transaction.KeyDeleteAsync(sessionKey);
        _ = transaction.SetRemoveAsync(userSessionsKey, sessionId);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task InvalidateAllUserSessionsAsync(
        string userId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var userSessionsKey = GetUserSessionsKey(userId);

        var sessionIds = await _database.SetMembersAsync(userSessionsKey);
        if (sessionIds.Length == 0)
        {
            await _database.KeyDeleteAsync(userSessionsKey);
            return;
        }

        var transaction = _database.CreateTransaction();

        foreach (var sessionId in sessionIds)
        {
            var sessionKey = GetSessionKey(sessionId!);
            _ = transaction.KeyDeleteAsync(sessionKey);
        }

        _ = transaction.KeyDeleteAsync(userSessionsKey);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    private string GetSessionKey(string sessionId)
        => $"{_settings.SessionKey}:{sessionId}";

    private string GetUserSessionsKey(string userId)
        => $"{_settings.UserSessionsKey}:{userId}";

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
