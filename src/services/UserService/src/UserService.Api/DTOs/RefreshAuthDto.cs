
using System;

namespace UserService.Api.DTOs;

public sealed record RefreshAuthDto(
    string ExpiredAccessToken,
    string ProvidedRefreshToken
);
