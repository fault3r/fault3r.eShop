
using System;

namespace JIT;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var ct = new CancellationToken();

        var loader = new Downloader();
        loader.DownloadComplete += (sender, url) =>
        {
            Console.WriteLine("Successfully downloaded");
        };

        await loader.DownloadAsync("filename.txt", ct);

        Console.WriteLine("run second test..");

        var ct2 = new CancellationTokenSource();

        var downloadTask = XDownloader.Download("url", ct2.Token);

        // ct2.Cancel();

        string data = await downloadTask;

        data = data == string.Empty ? "nothing" : data;

        Console.WriteLine($"downloaded data: {data}");
    }
}

public sealed class XDownloader
{
    public static async Task<string> Download(
        string url = "default",
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
            Console.WriteLine("canceled!");

            return string.Empty;
        }
    }
}

public class Downloader
{
    public async Task<string> DownloadAsync(string url, CancellationToken cancellationToken)
    {
        Console.WriteLine("downlading..");

        await Task.Delay(2000, cancellationToken);

        Console.WriteLine("data downloaded.");

        OnDownloadComplete(this, url);

        return $"data: {url}";
    }

    protected virtual void OnDownloadComplete(object? sender, string url)
        => DownloadComplete?.Invoke(this, url);

    public event EventHandler<string>? DownloadComplete;
}