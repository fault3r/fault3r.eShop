
using System;

namespace JIT;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.");

        Console.WriteLine("everything is ended.");
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
        Console.WriteLine($"downloading {url}...");

        await Task.Delay(3000);

        Console.WriteLine("downloaded.");

        OnComplete?.Invoke(this,new DownloadEventArgs("test.file"));

        return "data";
    }
}