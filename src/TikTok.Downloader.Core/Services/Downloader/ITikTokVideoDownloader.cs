using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Downloader;

public interface ITikTokVideoDownloader
{
    Task<byte[]> DownloadAsync(TikTokVideo tikTokVideo, CancellationToken cancellationToken = default);
}