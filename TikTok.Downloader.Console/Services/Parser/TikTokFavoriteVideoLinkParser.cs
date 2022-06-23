using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TikTok.Downloader.Models;

namespace TikTok.Downloader.Services.Parser;

internal sealed class TikTokFavoriteVideoLinkParser : ITikTokFavoriteVideoLinkParser
{
    public async Task<ICollection<TikTokVideo>> Parse(string path, CancellationToken cancellationToken = default)
    {
        var links = new List<TikTokVideo>();

        using (StreamReader file = File.OpenText(path))
        using (JsonTextReader reader = new(file))
        {
            var jObject = await JToken.ReadFromAsync(reader, cancellationToken) as JObject;
            var favoriteLinks = jObject["Activity"]?["Favorite Videos"]?["FavoriteVideoList"]?.Select(x => x["Link"]?.Value<string>());

            links = favoriteLinks.Select(link => new TikTokVideo(Id: link.Split("/")[^2], link)).ToList();
        }

        return links;
    }
}
