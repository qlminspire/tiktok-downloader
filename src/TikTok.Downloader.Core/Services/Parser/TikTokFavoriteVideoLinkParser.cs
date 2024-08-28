using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Parser;

internal sealed class TikTokFavoriteVideoLinkParser : ITikTokFavoriteVideoLinkParser
{
    public async Task<ICollection<TikTokVideo>> ParseAsync(string path, CancellationToken cancellationToken = default)
    {
        using var file = File.OpenText(path);
        await using JsonTextReader jsonReader = new(file);
        
        var json = await JToken.ReadFromAsync(jsonReader, cancellationToken) as JObject;
        var favoriteVideoJson = json?["Activity"]?["Favorite Videos"]?["FavoriteVideoList"];
        
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
