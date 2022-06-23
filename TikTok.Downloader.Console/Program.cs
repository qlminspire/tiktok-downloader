using System.Diagnostics;

using Cocona;
using Microsoft.Extensions.DependencyInjection;

using TikTok.Downloader.Services.Parser;
using TikTok.Downloader.Services.Downloader;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<ITikTokDownloader, TikTokDownloader>()
                .AddSingleton<ITikTokFavoriteVideoLinkParser, TikTokFavoriteVideoLinkParser>();

var app = builder.Build();

app.AddCommand(async (ITikTokDownloader tikTokDownloader, ITikTokFavoriteVideoLinkParser tikTokFavoriteVideoLinkParser,
   [Option('p', Description = "Path to TikTok profile json")] string path,
   [Option('o', Description = "Path to folder where downloaded videos will be")] string outputPath,
   [Option('l', Description = "Max amount of video to download")] int? limit) =>
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var links = await tikTokFavoriteVideoLinkParser.Parse(path);

    if (limit.HasValue)
        links = links.Take(limit.Value).ToList();

    foreach (var tikTokVideo in links)
    {
        try
        {
            var downloadedVideo = await tikTokDownloader.Download(tikTokVideo);
            await SaveVideo(tikTokVideo.Id, downloadedVideo, outputPath);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"- Can't download {tikTokVideo}. Exception: {exception.Message}");
        }
        Console.WriteLine($"+ Downloaded {tikTokVideo}");
    }

    stopwatch.Stop();

    Console.WriteLine($"\r\nDownloading completed.\r\nTime spent (seconds): {stopwatch.Elapsed.TotalSeconds}");
});

app.Run();

static async Task SaveVideo(string videoId, byte[] video, string outputPath, CancellationToken cancellationToken = default)
{
    using var streamWriter = File.Create(@$"{outputPath}\{videoId}.mp4");
    await streamWriter.WriteAsync(video, cancellationToken);
}