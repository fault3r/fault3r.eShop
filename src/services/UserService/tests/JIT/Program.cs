
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