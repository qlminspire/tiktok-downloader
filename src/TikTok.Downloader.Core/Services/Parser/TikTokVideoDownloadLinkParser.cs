using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace TikTok.Downloader.Core.Services.Parser;

internal sealed class TikTokVideoDownloadLinkParser : ITikTokVideoDownloadLinkParser
{
    public string? Parse(string content)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        var baseHtmlElement = htmlDocument.GetElementbyId("__UNIVERSAL_DATA_FOR_REHYDRATION__");
        var baseHtmlElementJObject = JObject.Parse(baseHtmlElement.InnerText);

        var videoJObject =
            baseHtmlElementJObject["__DEFAULT_SCOPE__"]?["webapp.video-detail"]?["itemInfo"]?["itemStruct"]?["video"];
        return videoJObject?["bitrateInfo"]?.MinBy(x => x["QualityType"]?.Value<int>())?["PlayAddr"]?["UrlList"]
            ?.LastOrDefault()?.Value<string>();
    }
}