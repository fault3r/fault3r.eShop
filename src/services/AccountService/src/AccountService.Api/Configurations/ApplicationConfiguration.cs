
using System;
using AccountService.Api.Exceptions;
using AccountService.Application.DTOs;
using AccountService.Application.Interfaces.Services;
using AccountService.Application.UseCases.SignUpAccount;
using AccountService.Domain.Interfaces;
using AccountService.Infrastructure.Repositories;
using FluentValidation;

namespace AccountService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            using var provider = services.BuildServiceProvider();
            var _logger = provider.GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring Application..");
                services.AddScoped(typeof(IRepository<>), typeof(PostgresRepository<>));
                services.AddScoped<IValidator<SignUpAccountRequest>, SignUpAccountValidator>();
                _logger.LogInformation("Application configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure Application settings!");
                throw new InvalidConfigurationException();
            }
        }
    }
}