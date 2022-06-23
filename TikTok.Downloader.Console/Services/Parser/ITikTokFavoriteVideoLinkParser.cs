using TikTok.Downloader.Models;

namespace TikTok.Downloader.Services.Parser;

public interface ITikTokFavoriteVideoLinkParser
{
    Task<ICollection<TikTokVideo>> Parse(string path, CancellationToken cancellationToken = default);
}
