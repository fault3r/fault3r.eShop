
using System;
using System.Threading.Tasks;

namespace JIT;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.");

        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

        var downloader = new Downloader();

        var task = downloader.Download("url", cts.Token);

        var data = await task;

        Console.WriteLine($"downloaded: {data}");

        Console.WriteLine("everything is ended.");
    }
}

public class Downloader
{
    public async Task<string> Download(
        string url,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Console.WriteLine("downloading..");

            await Task.Delay(3000, cancellationToken);

            Console.WriteLine("downloaded.");

            return "data";
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("canceled.");

            return "nothing";
        }
    }
}
