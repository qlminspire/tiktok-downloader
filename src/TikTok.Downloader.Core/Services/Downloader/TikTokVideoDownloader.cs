using Microsoft.Extensions.Logging;
using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Parser;

namespace TikTok.Downloader.Core.Services.Downloader;

internal sealed class TikTokVideoDownloader : ITikTokVideoDownloader
{
    private static readonly HttpClient HttpClient = new();
    private readonly ILogger<TikTokVideoDownloader> _logger;
    private readonly ITikTokVideoDownloadLinkParser _tikTokVideoDownloadLinkParser;

    public TikTokVideoDownloader(ITikTokVideoDownloadLinkParser tikTokVideoDownloadLinkParser,
        ILogger<TikTokVideoDownloader> logger
    )
    {
        _logger = logger;
        _tikTokVideoDownloadLinkParser = tikTokVideoDownloadLinkParser;
    }

    public async Task<byte[]> DownloadAsync(TikTokVideo tikTokVideo, CancellationToken cancellationToken = default)
    {
        try
        {
            var downloadLink = await GetVideoDownloadLinkAsync(tikTokVideo.Link, cancellationToken);
            if (string.IsNullOrWhiteSpace(downloadLink))
            {
                _logger.LogInformation(
                    "Failed to fetch download link for video: '{video}'. The downloading for this video will be skipped",
                    tikTokVideo.Link);
                return [];
            }

            _logger.LogTrace("Start downloading video: '{video}'", tikTokVideo.Link);

            var downloadedVideo = await DownloadVideoAsync(downloadLink, cancellationToken);

            _logger.LogTrace("Downloading completed for video: '{video}'", tikTokVideo.Link);

            return downloadedVideo;
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Failed to download video: '{video}'. {exception}", tikTokVideo.Link, exception.Message);
        }

        return [];
    }

    private async Task<string?> GetVideoDownloadLinkAsync(string link, CancellationToken cancellationToken = default)
    {
        var html = await HttpClient.GetStringAsync(link, cancellationToken);
        return _tikTokVideoDownloadLinkParser.Parse(html);
    }

    private static Task<byte[]> DownloadVideoAsync(string downloadLink, CancellationToken cancellationToken = default)
    {
        return HttpClient.GetByteArrayAsync(downloadLink, cancellationToken);
    }
}