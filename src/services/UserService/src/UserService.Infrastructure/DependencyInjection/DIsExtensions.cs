
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.CrossCutting;
using UserService.Application.Interfaces;
using UserService.Application.UseCases.Commands.LoginUserUseCase;
using UserService.Application.UseCases.Commands.LogoutUserUseCase;
using UserService.Application.UseCases.Commands.RefreshAuthUseCase;
using UserService.Application.UseCases.Commands.RegisterUserUseCase;
using UserService.Application.UseCases.Queries.UserProfileUseCase;
using UserService.Domain.Contracts;
using UserService.Domain.DomainServices;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Outbox;
using UserService.Domain.Repositories;
using UserService.Domain.Security;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.CrossCutting;
using UserService.Infrastructure.CrossCutting.JsonConverters;
using UserService.Infrastructure.Messaging.Bus;
using UserService.Infrastructure.Messaging.Outbox;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Security;
using UserService.Infrastructure.UnitOfWork;

namespace UserService.Infrastructure.DependencyInjection;

public static class DIsExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEfPostgresDbContext(configuration);

        services.AddJwtAuthentication(configuration);

        services.AddRedisCaching(configuration);

        services.AddSession(configuration);

        services.AddRateLimiter(configuration);

        services.AddApiVersioning(configuration);

        services.AddMassTransitMessaging(configuration);

        services.AddFluentEmailService(configuration);

        services.AddControllers(config =>
            config.SuppressAsyncSuffixInActionNames = false);

        services.AddUseCases();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IEventOutbox, EfPostgresEventOutbox>();

        services.AddScoped<IMessageBus, MassTransitMessageBus>();

        services.AddScoped<IUserRepository, EfUserRepository>();

        services.AddScoped<IUserDomainService, UserDomainService>();

        services.AddSingleton<ICorrelationContext, CorrelationContext>();

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

        services.AddHostedService<DIsInitializerHostedService>();

        services.AddHostedService<MassTransitOutboxBackgroundService>();

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
