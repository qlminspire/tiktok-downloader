using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TikTok.Downloader.Core.Models;

namespace TikTok.Downloader.Core.Services.Parser;

internal sealed class TikTokFavoriteVideoLinkParser : ITikTokFavoriteVideoLinkParser
{
    public async Task<ICollection<TikTokVideo>> Parse(string path, CancellationToken cancellationToken = default)
    {
        var links = new List<TikTokVideo>();

        using (StreamReader file = File.OpenText(path))
        using (JsonTextReader reader = new(file))
        {
            var jObject = await JToken.ReadFromAsync(reader, cancellationToken) as JObject;
            var favoriteVideos = jObject["Activity"]?["Favorite Videos"]?["FavoriteVideoList"];

            links = favoriteVideos.Select(x => new TikTokVideo(x["Link"]?.Value<string>(), DateTimeOffset.Parse(x["Date"]?.Value<string>()))).ToList();
        }

        return links;
    }
}
