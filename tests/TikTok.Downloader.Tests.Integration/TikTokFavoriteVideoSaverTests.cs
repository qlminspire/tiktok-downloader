using Microsoft.Extensions.Logging.Abstractions;
using TikTok.Downloader.Core.Configurations;
using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Downloader;
using TikTok.Downloader.Core.Services.Saver;

namespace TikTok.Downloader.Tests.Integration;

public class TikTokFavoriteVideoSaverTests
{
    private static string ProjectDirectory => Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;

    private static string SourceFile => Path.Combine(ProjectDirectory, "sources", "user_data_tiktok.json");
    
    private static string TargetPath =>  Path.Combine(ProjectDirectory, "targets");
    
    [Theory]
    [InlineData("7395812517471161608")]
    [InlineData("7389594419172855045")]
    [InlineData("7374132398323289349")]
    [InlineData("7373680469029817643")]
    public async Task Test1(string videoId)
    {
        // Arrange

        var tikTokVideo = new TikTokVideo($"https://www.tiktokv.com/share/video/{videoId}/");
        
        var configuration = new TikTokDownloaderConfiguration();
        var downloader = new TikTokVideoDownloader(configuration, new NullLogger<TikTokVideoDownloader>());
        var sut = new TikTokVideoSaver(downloader, new NullLogger<TikTokVideoSaver>());

        // Act

        await sut.SaveAsync(tikTokVideo, TargetPath);

        // Assert
    }
}