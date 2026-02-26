
using System;
using Microsoft.Extensions.Hosting;

namespace UserService.Infrastructure.DependencyInjection;

public sealed class DIsInitializerHostedService(
) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
