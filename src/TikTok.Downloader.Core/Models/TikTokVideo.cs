namespace TikTok.Downloader.Core.Models;

public record TikTokVideo(string Link, DateTimeOffset? Date = null)
{
    public string Id => Link?.Split("/")?[^2];
}