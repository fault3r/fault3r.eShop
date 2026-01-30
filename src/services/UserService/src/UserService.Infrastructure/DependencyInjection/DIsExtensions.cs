
using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.CrossCutting;
using UserService.Application.Interfaces;
using UserService.Application.Messaging.Notification;
using UserService.Application.UseCases.Commands.LoginUserUseCase;
using UserService.Application.UseCases.Commands.LogoutUserUseCase;
using UserService.Application.UseCases.Commands.RefreshAuthUseCase;
using UserService.Application.UseCases.Commands.RegisterUserUseCase;
using UserService.Application.UseCases.Queries.UserProfileUseCase;
using UserService.Domain.DomainServices;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;
using UserService.Domain.Messaging.Outbox;
using UserService.Domain.Repositories;
using UserService.Domain.Security;
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

        services.AddScoped<IEventOutbox, EfEventOutbox>();

        services.AddSingleton<INotificationOutbox, RedisNotificationOutbox>();

        services.AddScoped<IUserRepository, EfUserRepository>();

        services.AddScoped<IUserDomainService, UserDomainService>();

        services.AddSingleton<ICorrelationContext, CorrelationContext>();

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

        services.AddHostedService<MediatorNotificationPublisherBackgroundService>();

        services.AddSingleton<INotificationFactory, NotificationFactory>();

        return services;
    }

    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddMediatR(typeof(ICorrelationContext).Assembly);

        // services.AddMediatR(typeof(RegisterUserCommandHandler).Assembly);
        services.AddScoped<IRegisterUserService, RegisterUserService>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserValidator>();
        // services.AddMediatR(typeof(UserRegisteredNotificationHandler).Assembly);

        // services.AddMediatR(typeof(LoginUserCommandHandler).Assembly);
        services.AddScoped<ILoginUserService, LoginUserService>();
        services.AddScoped<IValidator<LoginUserCommand>, LoginUserValidator>();

        // services.AddMediatR(typeof(RefreshAuthCommandHandler).Assembly);
        services.AddScoped<IRefreshAuthService, RefreshAuthService>();
        services.AddScoped<IValidator<RefreshAuthCommand>, RefreshAuthValidator>();

        // services.AddMediatR(typeof(LogoutUserCommandHandler).Assembly);
        services.AddScoped<ILogoutUserService, LogoutUserService>();
        services.AddScoped<IValidator<LogoutUserCommand>, LogoutUserValidator>();

        // services.AddMediatR(typeof(UserProfileQueryHandler).Assembly);
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IValidator<UserProfileQuery>, UserProfileValidator>();

        return services;
    }
}
