
using System;

namespace JIT;

internal class Program
{
    public static void Main(string[] args)
    {

    }
}

public class Downloader
{
    public static async Task<string> Download(
        string url,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine("start downloading..");

        await Task.Delay(3000, cancellationToken);

        Console.WriteLine("downloaded.");

        return "data";
    }
}
