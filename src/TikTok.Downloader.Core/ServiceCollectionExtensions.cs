﻿using Microsoft.Extensions.DependencyInjection;

using TikTok.Downloader.Core.Services.Parser;
using TikTok.Downloader.Core.Services.Downloader;
using TikTok.Downloader.Core.Services.Saver;
using TikTok.Downloader.Core.Configurations;

namespace TikTok.Downloader.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDownloaderDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ITikTokDownloaderConfiguration, TikTokDownloaderConfiguration>()
            .AddSingleton<ITikTokVideoDownloader, TikTokVideoDownloader>()
            .AddSingleton<ITikTokFavoriteVideoLinkParser, TikTokFavoriteVideoLinkParser>()
            .AddSingleton<ITikTokVideoSaver, TikTokVideoSaver>();

        return services;
    }
}