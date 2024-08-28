using System.Text.RegularExpressions;

namespace TikTok.Downloader.Core.Models;

public sealed record TikTokVideo(string Link, DateTimeOffset? Date = null)
{
    private static readonly Regex IdRegex = new(@"(video\/)(\w+)", RegexOptions.Compiled);

    public string Id => IdRegex.Match(Link).Groups[2].Value;
}