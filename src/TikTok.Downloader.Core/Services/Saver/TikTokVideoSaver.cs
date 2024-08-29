using Microsoft.Extensions.Logging;
using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Downloader;

namespace TikTok.Downloader.Core.Services.Saver;

internal sealed class TikTokVideoSaver : ITikTokVideoSaver
{
    private readonly ILogger<TikTokVideoSaver> _logger;
    private readonly ITikTokVideoDownloader _tikTokDownloader;

    public TikTokVideoSaver(ITikTokVideoDownloader tikTokDownloader,
        ILogger<TikTokVideoSaver> logger
    )
    {
        _tikTokDownloader = tikTokDownloader;
        _logger = logger;
    }

    public async Task SaveAsync(TikTokVideo tikTokVideo, string outputPath)
    {
        var downloadedVideo = await _tikTokDownloader.DownloadAsync(tikTokVideo);
        if (downloadedVideo.Length == 0)
            return;

        await SaveVideoAsync(tikTokVideo.Id, downloadedVideo, outputPath);
    }

    public async Task SaveManyAsync(ICollection<TikTokVideo> tikTokVideos, string outputPath, int batchSize = 1)
    {
        var hasPartialIteration = (tikTokVideos.Count % batchSize) > 0;
        var batchIterations = (tikTokVideos.Count / batchSize) + (hasPartialIteration ? 1 : 0);

        for (var i = 0; i < batchIterations; i++)
        {
            var linksBatch = tikTokVideos.Skip(i * batchSize).Take(batchSize);

            var saveTasks = linksBatch.Select(link => SaveAsync(link, outputPath));
            await Task.WhenAll(saveTasks);
        }
    }

    private async Task SaveVideoAsync(string videoId, byte[] video, string outputPath,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var path = Path.ChangeExtension(Path.Combine(outputPath, videoId), "mp4");

            await using var streamWriter = File.Create(path);
            await streamWriter.WriteAsync(video, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning("Failed to save video {videoId} to '{path}'. {exception}", videoId, outputPath,
                exception.Message);
        }
    }
}