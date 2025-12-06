
using System;
using UserService.Domain.Common;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Interfaces;

public interface IUserDomainService
{
    Task<Result> CanCreateUserAsync(Email email);
}
