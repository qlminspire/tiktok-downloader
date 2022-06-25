using Cocona;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

using TikTok.Downloader.Console.Validation;

using TikTok.Downloader.Core;
using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Parser;
using TikTok.Downloader.Core.Services.Saver;

var builder = CoconaApp.CreateBuilder();

builder.Services.RegisterDownloaderDependencies();
builder.Logging.AddConsole();

var app = builder.Build();

app.AddCommand("link", async (ITikTokVideoSaver tikTokVideoSaver,
    [Option('u', Description = "TikTok video url")][UrlValidation] string url,
    [Option('o', Description = "Path to output folder")][PathValidation] string outputPath) =>
{
    await tikTokVideoSaver.Save(new TikTokVideo(url), outputPath);
});

app.AddCommand("json", async (ITikTokFavoriteVideoLinkParser tikTokFavoriteVideoLinkParser, ITikTokVideoSaver tikTokVideoSaver,
   [Option('p', Description = "Path to TikTok profile json file (user_data.json)")][PathValidation] string path,
   [Option('o', Description = "Path to output folder")][PathValidation] string outputPath,
   [Option('b', Description = "How many items download at a time")][Range(1, 25)] int batchSize,
   [Option('d', Description = "Download videos with date after specified")][DateValidation] string? afterDate,
   [Option('l', Description = "Maximum amount of items to download")] int? limit
) =>
{
    var tikTokVideoLinks = await tikTokFavoriteVideoLinkParser.Parse(path);

    if(DateTimeOffset.TryParse(afterDate, out var date))
        tikTokVideoLinks = tikTokVideoLinks.OrderBy(link => link.Date).Where(link => link.Date.Value > date).ToList();

    if (limit.HasValue)
        tikTokVideoLinks = tikTokVideoLinks.Take(limit.Value).ToList();

    await tikTokVideoSaver.SaveMany(tikTokVideoLinks, outputPath, batchSize);
});

app.Run();

