using System.Text.RegularExpressions;

namespace TikTok.Downloader.Core.Models;

public record TikTokVideo(string Link, DateTimeOffset? Date = null)
{
    private readonly Regex IdRegex = new(@"(video\/)(\w+)");

    public string Id => IdRegex.Match(Link).Groups[2].Value;
}