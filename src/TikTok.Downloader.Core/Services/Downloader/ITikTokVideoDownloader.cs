using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Downloader;

public interface ITikTokVideoDownloader
{
    Task<byte[]> Download(string videoId, CancellationToken cancellationToken = default);
    Task<byte[]> Download(TikTokVideo tikTokVideo, CancellationToken cancellationToken = default);
}
