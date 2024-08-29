using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Parser;

public interface ITikTokFavoriteVideosLinkParser
{
    ICollection<TikTokVideo> Parse(string content);
}