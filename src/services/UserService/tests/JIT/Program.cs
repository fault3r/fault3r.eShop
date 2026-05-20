
using System;

namespace JIT;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.");

        var downloader = new Downloader();

        downloader.OnComplete += OnDownloadComplete;

        var result = await downloader.Download("filename.tst");

        Console.WriteLine("everything is ended.");
    }

    private static void OnDownloadComplete(object? sender, DownloadEventArgs e)
    {
        Console.WriteLine($"{e.Filename} saved");
    }
}


public class DownloadEventArgs(string filename) : EventArgs
{
    public string Filename { get; init; } = filename;
}

public sealed class Downloader
{
    public event EventHandler<DownloadEventArgs>? OnComplete;

    public async Task<string> Download(string url)
    {
        Console.WriteLine("downloading...");

        await Task.Delay(3000);

        Console.WriteLine("downloaded.");

        OnComplete?.Invoke(this,new DownloadEventArgs(url));

        return "data";
    }
}