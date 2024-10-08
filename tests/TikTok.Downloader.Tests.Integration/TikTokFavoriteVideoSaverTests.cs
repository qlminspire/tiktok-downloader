using Microsoft.Extensions.Logging.Abstractions;
using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Downloader;
using TikTok.Downloader.Core.Services.Parser;
using TikTok.Downloader.Core.Services.Saver;

namespace TikTok.Downloader.Tests.Integration;

public class TikTokFavoriteVideoSaverTests
{
    private static string ProjectDirectoryPath =>
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

    private static string TargetPath => Path.Combine(ProjectDirectoryPath, "targets");

    [Theory]
    [InlineData("7395812517471161608")]
    [InlineData("7389594419172855045")]
    [InlineData("7373680469029817643")]
    public async Task Should_Save_DownloadedVideo_To_FileSystem(string videoId)
    {
        // Arrange
        var video = new TikTokVideo($"https://www.tiktokv.com/share/video/{videoId}/");

        var downloadLinkParser = new TikTokVideoDownloadLinkParser();
        var downloader = new TikTokVideoDownloader(downloadLinkParser, new NullLogger<TikTokVideoDownloader>());
        var sut = new TikTokVideoSaver(downloader, new NullLogger<TikTokVideoSaver>());

        var targetFile = Path.ChangeExtension(Path.Combine(TargetPath, videoId), "mp4");

        // Act
        await sut.SaveAsync(video, TargetPath);

        // Assert
        Assert.True(File.Exists(targetFile));
    }
}