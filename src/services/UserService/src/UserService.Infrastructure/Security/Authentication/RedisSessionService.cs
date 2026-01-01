using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Infrastructure.Exceptions.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class RedisSessionService(
    IConnectionMultiplexer connectionMultiplexer,
    IOptions<RedisSetting> options)
        : ISessionService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly RedisSetting _settings = options.Value;

    public async Task CreateSessionAsync(
        SessionData session,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var serialized = JsonSerializer.Serialize(session, jsonOptions);

        var expiry = session.ExpiresAt - DateTime.UtcNow;
        if (expiry <= TimeSpan.Zero)
            expiry = TimeSpan.FromMinutes(1);

        var sessionKey = GetSessionKey(session.SessionId);
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var transaction = _database.CreateTransaction();

        await transaction.StringSetAsync(sessionKey, serialized, expiry);
        await transaction.SetAddAsync(userSessionsKey, session.SessionId);
        await transaction.KeyExpireAsync(userSessionsKey, expiry);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task<SessionData?> GetSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        var value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty) return null;

        var session = JsonSerializer.Deserialize<SessionData>(value!, jsonOptions)!;

        var newExpiry = TimeSpan.FromDays(_settings.RefreshTokenLifetimeDays);
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var transaction = _database.CreateTransaction();

        await transaction.KeyExpireAsync(key, newExpiry);
        await transaction.KeyExpireAsync(userSessionsKey, newExpiry);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();

        return session;
    }

    public async Task InvalidateSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);
        var value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
        {
            await _database.KeyDeleteAsync(key);
            return;
        }

        var session = JsonSerializer.Deserialize<SessionData>(value!, jsonOptions)!;
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var tran = _database.CreateTransaction();

        _ = tran.SetRemoveAsync(userSessionsKey, sessionId);
        _ = tran.KeyDeleteAsync(key);

        await tran.ExecuteAsync();
    }

    public async Task InvalidateAllUserSessionsAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var userSessionsKey = GetUserSessionsKey(userId);
        var sessionIds = await _database.SetMembersAsync(userSessionsKey);

        var tran = _database.CreateTransaction();

        foreach (var sessionId in sessionIds)
        {
            var key = GetSessionKey(sessionId!);
            _ = tran.KeyDeleteAsync(key);
        }

        _ = tran.KeyDeleteAsync(userSessionsKey);

        await tran.ExecuteAsync();
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
