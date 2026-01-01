
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

        var serialized = JsonSerializer.Serialize(session);

        var expiry = session.ExpiresAt - DateTime.UtcNow;
        if (expiry <= TimeSpan.Zero)
            expiry = TimeSpan.FromMinutes(1);

        var batch = _database.CreateBatch();

        var sessionKey = GetSessionKey(session.SessionId);
        _ = batch.StringSetAsync(sessionKey, serialized, expiry);

        var userSessionsKey = GetUserSessionsKey(session.UserId);
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
        return JsonSerializer.Deserialize<SessionData>(value!)!;
    }

    private string GetSessionKey(string sessionId)
        => _settings.SessionKey + sessionId;

    private string GetUserSessionsKey(string sessionId)
        => _settings.SessionKey + sessionId;
}