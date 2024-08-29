using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using TikTok.Downloader.Core.Services.Downloader;
using TikTok.Downloader.Core.Services.Parser;
using TikTok.Downloader.Core.Services.Saver;

namespace TikTok.Downloader.Tests.Integration;

public class RunAll
{
    private static string ProjectDirectoryPath =>
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

    private static string SourcesPath => Path.Combine(ProjectDirectoryPath, "sources", "user_data_tiktok.json");
    private static string TargetPath => Path.Combine(ProjectDirectoryPath, "targets");

    [Fact]
    public async Task Should_Download_All_Favorite_Video()
    {
        // Arrange
        var tikTokUserDataJson = await File.ReadAllTextAsync(SourcesPath, Encoding.UTF8);
        var favoriteVideosLinkParser = new TikTokFavoriteVideosLinkJsonParser();

        var downloadLinkParser = new TikTokVideoDownloadLinkParser();
        var downloader = new TikTokVideoDownloader(downloadLinkParser, new NullLogger<TikTokVideoDownloader>());
        var saver = new TikTokVideoSaver(downloader, new NullLogger<TikTokVideoSaver>());

        // Act
        var tikTokVideos = favoriteVideosLinkParser.Parse(tikTokUserDataJson);
        await saver.SaveManyAsync(tikTokVideos, TargetPath, 5);

        // Assert
    }
}