
using System;
using Polly;
using Polly.Timeout;

namespace JIT;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var cancellationToken = new CancellationToken();

        Console.WriteLine("started.");
        var tp = Policy.TimeoutAsync(3);

        try
        {
            await tp.ExecuteAsync(async (ct) =>
            {
                await Downloader.Download(ct);

            }, cancellationToken);
        }
        catch(TimeoutRejectedException)
        {
            Console.WriteLine("could send in 3 secs.");
        }

        Console.WriteLine("ended.");
    }
}

public class Downloader
{
    public static async Task<string> Download(CancellationToken cancellationToken)
    {
        Console.WriteLine("downloading..");

        await Task.Delay(5000, cancellationToken);
        return "data";
    }
}