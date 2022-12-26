namespace GgdbNet.Models;

public class Game
{
    public string GameId { get; }
    public string DateAdded { get; }
    public string? SteamAppId { get; }
    public string? Title { get; }
    public string? SortingTitle { get; }
    public string? VerticalCover { get; }
    public IReadOnlyList<string> Genres { get; }
    public IReadOnlyList<string> Themes { get; }
    public string? Summary { get; }
    public int? Year { get; }
    public IReadOnlyList<string> Platforms { get; }
    public IReadOnlyList<string> Screenshots { get; }

    public static Game? Create(IEnumerable<Release> rs)
    {
        List<Release> releases = rs.Where(g => g.GetString("images", "verticalCover") != null)
            .OrderBy(g => g.GetString("title", "title"))
            .ThenBy(g => g.Platform)
            .ToList();

        return releases.Count == 0 ? null : new Game(releases);
    }

    private Game(List<Release> rs)
    {
        Release g = rs[0];
        GameId = g.GameId;
        DateAdded = g.AddedDate;
        SteamAppId = g.GetStrings("allGameReleases", "releases")
            ?.FirstOrDefault(r => r.StartsWith("steam_"))?.Split('_')[1];
        Title = g.GetString("title", "title");
        SortingTitle = g.GetString("sortingTitle", "title");
        if (Title == SortingTitle)
            SortingTitle = null;
        VerticalCover = g.GetString("images", "verticalCover");
        Genres = (IReadOnlyList<string>?)g.GetStrings("meta", "genres")?.ToList() ?? Array.Empty<string>();
        Themes = (IReadOnlyList<string>?)g.GetStrings("meta", "themes")?.ToList() ?? Array.Empty<string>();
        Summary = g.GetString("summary", "summary");
        if (g.GetProperty("meta", "releaseDate")?.TryGetInt64(out long ts) == true
            && DateTimeOffset.FromUnixTimeSeconds(ts).Year is > 1970 and var year)
            Year = year;
        Platforms = rs.Select(r => r.Platform).Order().ToList();
        Screenshots = (IReadOnlyList<string>?)g.GetStrings("media", "screenshots")
            ?.Select(url => url.Replace("{formatter}", "").Replace("{ext}", "jpg"))
            .ToList() ?? Array.Empty<string>();
    }
}