
using System;

namespace UserService.Domain.Exceptions.Role;

public sealed class UnsupportedRoleException(
    string value
) : RoleException($"unsupported role: {value}") { }