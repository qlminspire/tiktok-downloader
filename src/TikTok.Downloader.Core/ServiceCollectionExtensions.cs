using Microsoft.Extensions.DependencyInjection;
using TikTok.Downloader.Core.Configurations;
using TikTok.Downloader.Core.Services.Downloader;
using TikTok.Downloader.Core.Services.Parser;
using TikTok.Downloader.Core.Services.Saver;

namespace TikTok.Downloader.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDownloaderDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ITikTokDownloaderConfiguration, TikTokDownloaderConfiguration>()
            .AddSingleton<ITikTokVideoDownloader, TikTokVideoDownloader>()
            .AddSingleton<ITikTokFavoriteVideosLinkParser, TikTokFavoriteVideosLinkJsonParser>()
            .AddSingleton<ITikTokVideoSaver, TikTokVideoSaver>();

        return services;
    }
}