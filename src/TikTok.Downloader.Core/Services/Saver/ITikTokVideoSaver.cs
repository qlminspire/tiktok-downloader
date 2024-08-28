using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Saver;

public interface ITikTokVideoSaver
{
    Task SaveAsync(TikTokVideo tikTokVideo, string outputPath);

    Task SaveManyAsync(ICollection<TikTokVideo> tikTokVideos, string outputPath, int batchSize = 10);
}