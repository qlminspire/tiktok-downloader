using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

using TikTok.Downloader.Models;

namespace TikTok.Downloader.Services.Downloader;

internal sealed class TikTokDownloader : ITikTokDownloader
{
    private const string TikTokBaseUrl = "https://www.tiktokv.com/share/video";

    private static readonly HttpClient _httpClient = new();

    public Task<byte[]> Download(string videoId, CancellationToken cancellationToken)
    {
        return Download(new TikTokVideo(videoId, $"{TikTokBaseUrl}/{videoId}"), cancellationToken);
    }

    public async Task<byte[]> Download(TikTokVideo tikTokVideo, CancellationToken cancellationToken = default)
    {
        var downloadLink = await GetDownloadLink(tikTokVideo, cancellationToken);
        return await DownloadVideo(downloadLink, cancellationToken);
    }

    private static async Task<string> GetDownloadLink(TikTokVideo video, CancellationToken cancellationToken = default)
    {
        var htmlWeb = new HtmlWeb();
        var htmlDocument = await htmlWeb.LoadFromWebAsync(video.Link, cancellationToken);

        var sigiStateElement = htmlDocument.GetElementbyId("SIGI_STATE");
        var sigiStateElementJObject = JObject.Parse(sigiStateElement.InnerText);

        return sigiStateElementJObject["ItemModule"]?[video.Id]?["video"]?["downloadAddr"]?.Value<string>();
    }

    private static async Task<byte[]> DownloadVideo(string downloadLink, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetByteArrayAsync(downloadLink, cancellationToken);
    }
}
