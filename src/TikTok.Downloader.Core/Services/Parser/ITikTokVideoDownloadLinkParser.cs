namespace TikTok.Downloader.Core.Services.Parser;

public interface ITikTokVideoDownloadLinkParser
{
    string? Parse(string content);
}