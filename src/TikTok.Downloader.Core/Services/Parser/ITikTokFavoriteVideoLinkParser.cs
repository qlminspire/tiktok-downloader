using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Parser;

public interface ITikTokFavoriteVideoLinkParser
{
    Task<ICollection<TikTokVideo>> Parse(string path, CancellationToken cancellationToken = default);
}
