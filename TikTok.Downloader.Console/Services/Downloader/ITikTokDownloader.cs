using TikTok.Downloader.Models;

namespace TikTok.Downloader.Services.Downloader;

public interface ITikTokDownloader
{
    Task<byte[]> Download(string videoId, CancellationToken cancellationToken = default);
    Task<byte[]> Download(TikTokVideo tikTokVideo, CancellationToken cancellationToken = default);
}
