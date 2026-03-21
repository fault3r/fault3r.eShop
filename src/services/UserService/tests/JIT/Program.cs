
using System;
using System.Runtime.CompilerServices;

namespace JIT;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("started.");

        void Subscriber1(object? sender, string url) => Console.WriteLine($"subscriber1: received.");
        void Subscriber2(object? sender, string url) => Console.WriteLine($"subscriber2: received.");

        var loader = new Downloader();

        loader.DownloadComplete += Subscriber1;
        loader.DownloadComplete += Subscriber2;

        loader.Download("test.txt");

        Console.WriteLine("ended.");
    }
}

public class Downloader
{
    public void Download(string url)
    {
        Console.WriteLine("downloading…");

        Thread.Sleep(3000);

        Console.WriteLine($"file {url} downloaded.");

        OnDownloadComplete(url);
    }

    protected virtual void OnDownloadComplete(string url)
        => DownloadComplete?.Invoke(this, url);

    public event EventHandler<string>? DownloadComplete;
}

