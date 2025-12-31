
using System;

namespace UserService.Application.Security.Authentication;

public sealed class SessionData
{
    public required string SessionId { get; init; } 
    public required string UserId { get; init; } 
    public required string Email { get; init; } 
    public required string FullName { get; init; } 
    public required string Role { get; init; } 
    public required string Status { get; init; } 
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresAt { get; init; }
}
