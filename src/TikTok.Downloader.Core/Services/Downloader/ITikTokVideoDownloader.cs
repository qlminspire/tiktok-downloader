using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Downloader;

public interface ITikTokVideoDownloader
{
    Task<byte[]> DownloadAsync(string videoId, CancellationToken cancellationToken = default);
    
    Task<byte[]> DownloadAsync(TikTokVideo tikTokVideo, CancellationToken cancellationToken = default);
}
