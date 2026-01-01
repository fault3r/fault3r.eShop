
using System;
using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class RedisSessionService(
    IConnectionMultiplexer connectionMultiplexer,
    IOptions<RedisSetting> options)
        : ISessionService
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
    private readonly RedisSetting _settings = options.Value;

    public Task CreateSessionAsync(
        SessionData session,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var value = JsonSerializer.Serialize(session);

        var expiry = session.ExpiresAt - DateTime.UtcNow;
        if (expiry <= TimeSpan.Zero)
            expiry = TimeSpan.FromMinutes(1);

        var sessionKey = GetSessionKey(session.SessionId);
        var userSessionsKey = GetUserSessionsKey(session.UserId);
        
        var batch = _database.CreateBatch();
        _ = batch.StringSetAsync(sessionKey, value, expiry);
        _ = batch.SetAddAsync(userSessionsKey, session.SessionId);
        _ = batch.KeyExpireAsync(userSessionsKey, expiry);
        batch.Execute();

        return Task.CompletedTask;
    }

    public async Task<SessionData?> GetSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var key = GetSessionKey(sessionId);

        var value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty) return null;

        var serializes = JsonSerializer.Deserialize<SessionData>(value!);
        return serializes;
    }

    public async Task InvalidateSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        var key = GetSessionKey(sessionId);

        var value = await _database.StringGetAsync(key);

        if (!value.IsNullOrEmpty)
        {
            var session = JsonSerializer.Deserialize<SessionData>(value!)!;
            var userSessionsKey = GetUserSessionsKey(session.UserId);

            var batch = _database.CreateBatch();
            _ = batch.KeyDeleteAsync(key);
            _ = batch.SetRemoveAsync(userSessionsKey, sessionId);
            batch.Execute();
        }
        else
        {
            await _database.KeyDeleteAsync(key);
        }
    }

    public async Task InvalidateAllUserSessionsAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var userSessionsKey = GetUserSessionsKey(userId);
        var sessionIds = await _database.SetMembersAsync(userSessionsKey);

        var batch = _database.CreateBatch();
        foreach (var sessionId in sessionIds)
        {
            var key = GetSessionKey(sessionId!);
            _ = batch.KeyDeleteAsync(key);
        }
        _ = batch.KeyDeleteAsync(userSessionsKey);
        batch.Execute();
    }

    private string GetSessionKey(string sessionId)
        => $"{_settings.SessionKey}:{sessionId}";

    private string GetUserSessionsKey(string sessionId)
        => $"{_settings.UserSessionsKey}:{sessionId}";
}