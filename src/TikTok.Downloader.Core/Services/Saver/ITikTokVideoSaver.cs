using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Saver
{
    public interface ITikTokVideoSaver
    {
        Task Save(TikTokVideo tikTokVideo, string outputPath);

        Task SaveMany(ICollection<TikTokVideo> tikTokVideos, string outputPath, int batchSize = 10);
    }
}
