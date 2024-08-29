using System.ComponentModel.DataAnnotations;
using System.Text;
using Cocona;
using Microsoft.Extensions.Logging;
using TikTok.Downloader.Console.Validation;
using TikTok.Downloader.Core;
using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Parser;
using TikTok.Downloader.Core.Services.Saver;

var builder = CoconaApp.CreateBuilder();

builder.Logging.AddConsole();

builder.Services.RegisterDownloaderDependencies();

var app = builder.Build();

app.AddCommand("url", async (ITikTokVideoSaver tikTokVideoSaver,
    [Option('p', Description = "TikTok video url")] [UrlValidation]
    string path,
    [Option('o', Description = "Path to output folder")] [PathValidation]
    string outputPath) =>
{
    await tikTokVideoSaver.SaveAsync(new TikTokVideo(path), outputPath);
});

app.AddCommand("json", async (ITikTokFavoriteVideosLinkParser tikTokFavoriteVideoLinkParser,
    ITikTokVideoSaver tikTokVideoSaver,
    [Option('p', Description = "Path to TikTok profile json file (user_data.json)")] [PathValidation]
    string path,
    [Option('o', Description = "Path to output folder")] [PathValidation]
    string outputPath,
    [Option('b', Description = "How many items download at a time")] [Range(1, 25)]
    int batchSize,
    [Option('d', Description = "Download videos with date after specified")] [DateValidation]
    string? afterDate,
    [Option('l', Description = "Maximum amount of items to download")]
    int? limit
) =>
{
    var tikTokUserDataJson = await File.ReadAllTextAsync(path, Encoding.UTF8);
    var tikTokVideoLinks = tikTokFavoriteVideoLinkParser.Parse(tikTokUserDataJson);

    if (DateTimeOffset.TryParse(afterDate, out var date))
        tikTokVideoLinks = tikTokVideoLinks.OrderBy(link => link.Date)
            .Where(link => link.Date is not null && link.Date > date)
            .ToList();

    if (limit.HasValue)
        tikTokVideoLinks = tikTokVideoLinks.Take(limit.Value).ToList();

    await tikTokVideoSaver.SaveManyAsync(tikTokVideoLinks, outputPath, batchSize);
});

app.Run();