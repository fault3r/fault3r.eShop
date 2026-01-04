
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

    public async Task CreateAsync(
        SessionData session,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        await EnforceSessionLimitAsync(session.UserId, ct);

        var payload = JsonSerializer.Serialize(session, jsonSerializerOptions);

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

    private async Task EnforceSessionLimitAsync(string userId, CancellationToken ct)
    {
        var userSessionsKey = GetUserSessionsKey(userId);

        var sessionIds = await _database.SetMembersAsync(userSessionsKey);

        if (sessionIds.Length < _settings.MaxSessionsPerUser)
            return;

        var sessions = new List<(string SessionId, SessionData Data)>();
        foreach (var id in sessionIds)
        {
            var key = GetSessionKey(id!);
            var payload = await _database.StringGetAsync(key);

            if (payload.IsNullOrEmpty)
                continue;

            var session = JsonSerializer.Deserialize<SessionData>(payload!, jsonSerializerOptions);
            if (session != null)
                sessions.Add((id!, session));
        }

        if (sessions.Count < _settings.MaxSessionsPerUser)
            return;

        var (SessionId, Data) = sessions
            .OrderBy(s => s.Data.LastAccessedAt)
            .First();
        var oldestSessionKey = GetSessionKey(SessionId);

        var transaction = _database.CreateTransaction();

        _ = transaction.KeyDeleteAsync(oldestSessionKey);
        _ = transaction.SetRemoveAsync(userSessionsKey, SessionId);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task UpdateAsync(
        SessionData session,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var payload = JsonSerializer.Serialize(session, jsonSerializerOptions);

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

    public async Task<SessionData?> GetAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        var payload = await _database.StringGetAsync(key);

        if (payload.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<SessionData>(payload!, jsonSerializerOptions)!;
    }

    public async Task<bool> ExistAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        return await _database.KeyExistsAsync(key);
    }

    public async Task InvalidateAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var sessionKey = GetSessionKey(sessionId);

        var payload = await _database.StringGetAsync(sessionKey);

        if (payload.IsNullOrEmpty) return;

        var session = JsonSerializer.Deserialize<SessionData>(payload!, jsonSerializerOptions);

        var userSessionsKey = GetUserSessionsKey(session!.UserId);

        var transaction = _database.CreateTransaction();

        _ = transaction.KeyDeleteAsync(sessionKey);
        _ = transaction.SetRemoveAsync(userSessionsKey, sessionId);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task InvalidateAllAsync(
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

        foreach (var id in sessionIds)
        {
            var key = GetSessionKey(id!);
            _ = transaction.KeyDeleteAsync(key);
        }

        _ = transaction.KeyDeleteAsync(userSessionsKey);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    private string GetSessionKey(string sessionId)
        => $"{_settings.SessionKey}:{sessionId}";

    private string GetUserSessionsKey(string userId)
        => $"{_settings.UserSessionsKey}:{userId}";

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
