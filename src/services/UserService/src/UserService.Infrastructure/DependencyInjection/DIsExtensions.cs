
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Application.Security;
using UserService.Application.UseCases.SignUpUser;
using UserService.Domain.DomainServices;
using UserService.Domain.Interfaces;
using UserService.Domain.Outbox;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.CrossCutting;
using UserService.Infrastructure.Messaging.Outbox;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Security;
using UserService.Infrastructure.UnitOfWork;

namespace UserService.Infrastructure.DependencyInjection;

public static class DIsExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(typeof(SignUpUserCommand).Assembly);

        services.AddScoped<ISignUpUserService, SignUpUserService>();

        services.AddScoped<IUserDomainService, UserDomainService>();

        services.AddScoped<IUserRepository, EfUserRepository>();

        services.AddScoped<IOutbox, EfOutbox>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IValidator<SignUpUserCommand>, SignUpUserValidator>();

        services.AddSingleton<ICorrelationContext, CorrelationContext>();

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

        services.AddScoped<IEmailTemplatePathResolver, EmailTemplatePathResolver>();

        return services;
    }
}
