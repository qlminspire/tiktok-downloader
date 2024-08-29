using TikTok.Downloader.Core.Models;
using TikTok.Downloader.Core.Services.Parser;

namespace TikTok.Downloader.Tests.Unit;

public class TikTokFavoriteVideosLinkJsonParserTests
{
    [Fact]
    public void Should_Return_TwoTikTokVideo_When_Json_FavoriteVideoList_Contains_TwoRecords()
    {
        // Arrange
        var expected = new List<TikTokVideo>
        {
            new("https://www.tiktokv.com/share/video/7404519706050170117/",
                DateTimeOffset.Parse("2024-08-20 13:22:36")),
            new("https://www.tiktokv.com/share/video/7366854421403241745/", DateTimeOffset.Parse("2024-07-30 20:22:29"))
        };

        const string json = """
                            {
                              "Activity": {
                                "Favorite Videos": {
                                  "App": 1,
                                  "FavoriteVideoList": [
                                    {
                                      "Date": "2024-08-20 13:22:36",
                                      "Link": "https://www.tiktokv.com/share/video/7404519706050170117/"
                                    },
                                    {
                                      "Date": "2024-07-30 20:22:29",
                                      "Link": "https://www.tiktokv.com/share/video/7366854421403241745/"
                                    }
                                  ]
                                }
                              }
                            }

                            """;

        var sut = new TikTokFavoriteVideosLinkJsonParser();

        // Act
        var result = sut.Parse(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Should_Return_EmptyTikTokVideoList_When_Json_FavoriteVideoList_Is_Empty()
    {
        // Arrange
        var expected = new List<TikTokVideo>();

        const string json = """
                            {
                              "Activity": {
                                "Favorite Videos": {
                                  "App": 1,
                                  "FavoriteVideoList": [
                                  ]
                                }
                              }
                            }

                            """;

        var sut = new TikTokFavoriteVideosLinkJsonParser();

        // Act
        var result = sut.Parse(json);

        // Assert
        Assert.Equal(expected, result);
    }
}