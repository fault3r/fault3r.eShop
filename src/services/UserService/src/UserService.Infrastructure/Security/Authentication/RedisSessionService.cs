
using System;
using System.Text.Json;
using StackExchange.Redis;
using UserService.Domain.Contracts;
using UserService.Domain.Security.Authentication;
using UserService.Infrastructure.Exceptions.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class RedisSessionService(
    IConnectionMultiplexer redisConnection,
    SessionSettings settings
) : ISessionService
{
    private readonly IDatabase _database = redisConnection.GetDatabase();
    private readonly SessionSettings _settings = settings;

    private readonly JsonSerializerOptions jsonOptions
        = SharedJsonOptions.DefaultOptions;
        
    public async Task CreateAsync(
        SessionData session,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        await EnforceSessionLimitAsync(session.UserId, cancellationToken);

        var payload = JsonSerializer.Serialize(session, jsonOptions);

        var expires = session.RefreshTokenExpiresAt - DateTimeOffset.UtcNow;
        if (expires <= TimeSpan.Zero) expires = TimeSpan.FromMinutes(1);

        var sessionKey = GetSessionKey(session.SessionId);
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var transaction = _database.CreateTransaction();

        _ = transaction.StringSetAsync(sessionKey, payload, expires);
        _ = transaction.SetAddAsync(userSessionsKey, session.SessionId);
        _ = transaction.KeyExpireAsync(userSessionsKey, expires);

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

            var session = JsonSerializer.Deserialize<SessionData>(payload!, jsonOptions);
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

    public async Task<SessionData?> GetAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        var payload = await _database.StringGetAsync(key);

        if (payload.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<SessionData>(payload!, jsonOptions)!;
    }

    public async Task<bool> ExistAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var key = GetSessionKey(sessionId);

        return await _database.KeyExistsAsync(key);
    }

    public async Task UpdateAsync(
        SessionData session,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var payload = JsonSerializer.Serialize(session, jsonOptions);

        var expires = session.RefreshTokenExpiresAt - DateTimeOffset.UtcNow;
        if (expires <= TimeSpan.Zero) expires = TimeSpan.FromMinutes(1);

        var sessionKey = GetSessionKey(session.SessionId);
        var userSessionsKey = GetUserSessionsKey(session.UserId);

        var transaction = _database.CreateTransaction();

        _ = transaction.StringSetAsync(sessionKey, payload, expires);
        _ = transaction.KeyExpireAsync(userSessionsKey, expires);

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }
    
    public async Task InvalidateAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
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

    public async Task InvalidateAllAsync(
        string userId,
        CancellationToken cancellationToken = default)
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
}
