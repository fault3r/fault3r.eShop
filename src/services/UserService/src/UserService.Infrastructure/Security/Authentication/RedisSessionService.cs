
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

        var value = JsonSerializer.Serialize(session, jsonOptions);

        var expiry = session.ExpiresAt - DateTime.UtcNow;
        if (expiry <= TimeSpan.Zero)
            expiry = TimeSpan.FromMinutes(1);

        var sessionKey = GetSessionKey(session.SessionId);
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var batch = _database.CreateBatch();
        var tasks = new List<Task>
        {
            batch.StringSetAsync(sessionKey, value, expiry),
            batch.SetAddAsync(userSessionsKey, session.SessionId),
            batch.KeyExpireAsync(userSessionsKey, expiry),
        };
        batch.Execute();

        return Task.WhenAll(tasks);
    }

    public async Task<SessionData?> GetSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        var value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return null;

        var session = JsonSerializer.Deserialize<SessionData>(value!, jsonOptions)!;

        var newExpiry = TimeSpan.FromDays(_settings.RefreshTokenLifetimeDays);

        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var batch = _database.CreateBatch();
        var tasks = new List<Task>
        {
            batch.KeyExpireAsync(key, newExpiry),
            batch.KeyExpireAsync(userSessionsKey, newExpiry)
        };
        batch.Execute();

        await Task.WhenAll(tasks);

        return session;
    }


    public async Task InvalidateSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        var value = await _database.StringGetAsync(key);

        if (!value.IsNullOrEmpty)
        {
            var session = JsonSerializer.Deserialize<SessionData>(value!, jsonOptions)!;
            var userSessionsKey = GetUserSessionsKey(session.UserId);

            var batch = _database.CreateBatch();
            var tasks = new List<Task>
            {
                batch.SetRemoveAsync(userSessionsKey, sessionId),
                batch.KeyDeleteAsync(key),
            };
            batch.Execute();
            await Task.WhenAll(tasks);
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
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var userSessionsKey = GetUserSessionsKey(userId);
        var sessionIds = await _database.SetMembersAsync(userSessionsKey);

        var batch = _database.CreateBatch();
        var tasks = new List<Task>();
        foreach (var sessionId in sessionIds)
        {
            var key = GetSessionKey(sessionId!);
            tasks.Add(batch.KeyDeleteAsync(key));
        }
        tasks.Add(batch.KeyDeleteAsync(userSessionsKey));
        batch.Execute();

        await Task.WhenAll(tasks);
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