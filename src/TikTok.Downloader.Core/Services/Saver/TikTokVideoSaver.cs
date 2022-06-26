using Microsoft.Extensions.Logging;

using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Downloader;

namespace TikTok.Downloader.Core.Services.Saver
{
    internal sealed class TikTokVideoSaver : ITikTokVideoSaver
    {
        private readonly ITikTokVideoDownloader _tikTokDownloader;
        private readonly ILogger<TikTokVideoSaver> _logger;

        public TikTokVideoSaver(ITikTokVideoDownloader tikTokDownloader,
            ILogger<TikTokVideoSaver> logger
            )
        {
            _tikTokDownloader = tikTokDownloader;
            _logger = logger;
        }

        public async Task Save(TikTokVideo tikTokVideo, string outputPath)
        {
            try
            {
                _logger.LogInformation($"Start downloading {tikTokVideo}");
                var downloadedVideo = await _tikTokDownloader.Download(tikTokVideo);
                await SaveVideo(tikTokVideo.Id, downloadedVideo, outputPath);
                _logger.LogInformation($"Downloaded {tikTokVideo}");
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Can't download {tikTokVideo}. {exception}");
            }
        }

        public async Task SaveMany(ICollection<TikTokVideo> tikTokVideos, string outputPath, int batchSize = 1)
        {
            var hasPartialIteration = (tikTokVideos.Count % batchSize) > 0; 
            var batchIterations = (tikTokVideos.Count / batchSize) + (hasPartialIteration ? 1 : 0);

            for (var i = 0; i < batchIterations; i++)
            {
                var linksBatch = tikTokVideos.Skip(i * batchSize).Take(batchSize);

                var saveTasks = linksBatch.Select(link => Save(link, outputPath));
                await Task.WhenAll(saveTasks);
            }
        }

        static async Task SaveVideo(string videoId, byte[] video, string outputPath, CancellationToken cancellationToken = default)
        {
            using var streamWriter = File.Create(@$"{outputPath}\{videoId}.mp4");
            await streamWriter.WriteAsync(video, cancellationToken);
        }
    }
}
