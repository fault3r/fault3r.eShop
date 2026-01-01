
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Application.Messaging;
using UserService.Application.UseCases.UserAggregate.SignInUser;
using UserService.Application.UseCases.UserAggregate.SignUpUser;
using UserService.Domain.DomainServices;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.CrossCutting;
using UserService.Infrastructure.Messaging.Notification;
using UserService.Infrastructure.Messaging.Outbox;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Security.PasswordHasher;
using UserService.Infrastructure.UnitOfWork;

namespace UserService.Infrastructure.DependencyInjection;

public static class DIsExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IDomainOutbox, EfDomainOutbox>();

        services.AddScoped<IDomainNotification, MediatorDomainNotification>();

        services.AddScoped<IUserRepository, EfUserRepository>();

        services.AddScoped<IUserDomainService, UserDomainService>();

        services.AddSingleton<ICorrelationContext, CorrelationContext>();

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

        services.AddSingleton<IEventNotificationMapper, DomainEventNotificationMapper>();

        return services;
    }

    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddMediatR(typeof(SignUpUserCommand).Assembly);
        services.AddScoped<ISignUpUserService, SignUpUserService>();
        services.AddScoped<IValidator<SignUpUserCommand>, SignUpUserValidator>();

        services.AddMediatR(typeof(SignInUserCommand).Assembly);
        services.AddScoped<ISignInUserService, SignInUserService>();
        services.AddScoped<IValidator<SignInUserCommand>, SignInUserValidator>();

        return services;
    }
}
