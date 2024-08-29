using Microsoft.Extensions.Logging.Abstractions;
using TikTok.Downloader.Core.Configurations;
using TikTok.Downloader.Core.Services.Downloader;
using TikTok.Downloader.Core.Services.Parser;
using TikTok.Downloader.Core.Services.Saver;

namespace TikTok.Downloader.Tests.Integration;

public class TikTokFavoriteVideoLinkJsonParserTests
{
    private static string ProjectDirectory =>
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

    private static string SourceFile => Path.Combine(ProjectDirectory, "sources", "user_data_tiktok.json");
    private static string TargetPath => Path.Combine(ProjectDirectory, "targets");

    [Fact]
    public Task Test1()
    {
        // Arrange

        var sut = new TikTokFavoriteVideoLinkJsonParser();

        // Act

        var result = sut.Parse(SourceFile);

        // Assert

        return Task.CompletedTask;
    }

    [Fact]
    public async Task Test2()
    {
        // Arrange

        var parser = new TikTokFavoriteVideoLinkJsonParser();

        var configuration = new TikTokDownloaderConfiguration();
        var downloader = new TikTokVideoDownloader(configuration, new NullLogger<TikTokVideoDownloader>());
        var saver = new TikTokVideoSaver(downloader, new NullLogger<TikTokVideoSaver>());

        // Act

        var result = parser.Parse(SourceFile);
        await saver.SaveManyAsync(result, TargetPath, 5);

        // Assert
    }
}