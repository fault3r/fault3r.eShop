
using System;
using Polly;
using Polly.Timeout;

namespace UserService.JITests;

internal class Program
{
    private static readonly CancellationTokenSource cts = new();

    private async static Task Save()
    {
        await Task.Delay(3000);
        throw new Exception();
    }

    public static void Main(string[] args)
    {
        var ct = cts.Token;

        var timeout = Policy
            .TimeoutAsync(
                TimeSpan.FromSeconds(10),
                TimeoutStrategy.Pessimistic
            );

        var commit = timeout.ExecuteAsync(async ct =>
        {
            Console.WriteLine("operation started.");

            var operation = Save();

            Console.WriteLine("waiting..");

            await Task.WhenAny(operation, Task.Delay(Timeout.Infinite, ct));

            ct.ThrowIfCancellationRequested();

            await operation;

            Console.WriteLine("operation completed.");

        }, ct);

        commit.Wait();
    }
}