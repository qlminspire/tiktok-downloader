using Newtonsoft.Json.Linq;
using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Parser;

internal sealed class TikTokFavoriteVideosLinkJsonParser : ITikTokFavoriteVideosLinkParser
{
    public ICollection<TikTokVideo> Parse(string content)
    {
        var jObject = JToken.Parse(content);
        var favoriteVideoJson = jObject["Activity"]?["Favorite Videos"]?["FavoriteVideoList"];

        var tikTokVideos = favoriteVideoJson?.Select(x =>
            {
                var link = x["Link"]?.Value<string>()?.Trim();

                DateTimeOffset.TryParse(x["Date"]?.Value<string>(), out var date);

                return !string.IsNullOrWhiteSpace(link)
                    ? new TikTokVideo(link, date)
                    : null;
            }).Where(x => x is not null)
            .ToList() ?? [];

        return tikTokVideos;
    }
}