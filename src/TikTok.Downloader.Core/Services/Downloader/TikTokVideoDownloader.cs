using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

using TikTok.Downloader.Core.Configurations;
using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Downloader;

internal sealed class TikTokVideoDownloader : ITikTokVideoDownloader
{
    private readonly ITikTokDownloaderConfiguration _configuration;
    private readonly ILogger<TikTokVideoDownloader> _logger;

    private static readonly HttpClient HttpClient = new();

    public TikTokVideoDownloader(ITikTokDownloaderConfiguration configuration, 
        ILogger<TikTokVideoDownloader> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task<byte[]> DownloadAsync(string videoId, CancellationToken cancellationToken)
    {
        return DownloadAsync(new TikTokVideo($"{_configuration.TikTokVideoBaseUrl}/{videoId}"), cancellationToken);
    }

    public async Task<byte[]> DownloadAsync(TikTokVideo tikTokVideo, CancellationToken cancellationToken = default)
    {
        try
        {
            var downloadLink = await GetVideoDownloadLinkAsync(tikTokVideo, cancellationToken);
            if (string.IsNullOrWhiteSpace(downloadLink))
            {
                _logger.LogInformation("Failed to fetch download link for video: {video}. The downloading for this video will be skipped", tikTokVideo);
                return [];
            }

            _logger.LogInformation("Start downloading video: {video}", tikTokVideo);

            var downloadedVideo = await DownloadVideoAsync(downloadLink, cancellationToken);

            _logger.LogInformation("Downloading completed for video: {video}", tikTokVideo);
            
            return downloadedVideo;
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Failed to download video: {video}. {exception}", tikTokVideo, exception.Message);
        }
        
        return [];
    }

    private static async Task<string?> GetVideoDownloadLinkAsync(TikTokVideo video, CancellationToken cancellationToken = default)
    {
        var htmlWeb = new HtmlWeb();
        var htmlDocument = await htmlWeb.LoadFromWebAsync(video.Link, cancellationToken);

        var baseHtmlElement = htmlDocument.GetElementbyId("__UNIVERSAL_DATA_FOR_REHYDRATION__");
        var baseHtmlElementJObject = JObject.Parse(baseHtmlElement.InnerText);

        var videoJObject =
            baseHtmlElementJObject["__DEFAULT_SCOPE__"]?["webapp.video-detail"]?["itemInfo"]?["itemStruct"]?["video"];
       return videoJObject?["bitrateInfo"]?.MinBy(x => x["QualityType"]?.Value<int>())?["PlayAddr"]?["UrlList"]?.LastOrDefault()?.Value<string>();
    }

    private static Task<byte[]> DownloadVideoAsync(string downloadLink, CancellationToken cancellationToken = default)
    {
        return HttpClient.GetByteArrayAsync(downloadLink, cancellationToken);
    }
}
