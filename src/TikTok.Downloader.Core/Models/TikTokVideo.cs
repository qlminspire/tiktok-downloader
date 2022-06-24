namespace TikTok.Downloader.Core.Models;

public record TikTokVideo(string Link)
{
    public string Id => Link?.Split("/")?[^2];
}