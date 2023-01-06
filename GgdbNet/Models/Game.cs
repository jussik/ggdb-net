namespace GgdbNet.Models;

public class Game
{
    public string GameId { get; }
    public string DateAdded { get; }
    public IReadOnlyList<string> ReleaseIds { get; }
    public string? SteamAppId { get; }
    public string? Title { get; }
    public string? SortingTitle { get; }
    public IReadOnlyList<PlatformTitle> AllTitles { get; set; }
    public string? VerticalCover { get; }
    public IReadOnlyList<string> Genres { get; }
    public IReadOnlyList<string> Themes { get; }
    public string? Summary { get; }
    public DateTimeOffset? ReleaseDate { get; }
    public IReadOnlyList<string> Platforms { get; }
    public IReadOnlyList<string> Screenshots { get; }

    public class PlatformTitle
    {
        public string Platform { get; }
        public string Title { get; }
        public string? SortingTitle { get; }

        public PlatformTitle(Release r)
        {
            Platform = r.Platform;
            Title = r.GetString("title", "title")!;
            SortingTitle = r.GetString("sortingTitle", "title");
        }
    }

    public static Game? Create(IEnumerable<Release> rs)
    {
        List<Release> releases = rs.Where(g => g.GetString("images", "verticalCover") != null)
            .ToList();

        return releases.Count == 0 ? null : new Game(releases);
    }

    private Game(List<Release> rs)
    {
        Release g = rs[0];
        GameId = g.GameId;
        DateAdded = rs.Min(r => r.AddedDate)!;
        
        ReleaseIds = g.GetStrings("allGameReleases", "releases")?.ToList() ?? new();
        if (ReleaseIds.FirstOrDefault(r => r.StartsWith("steam_")) is { } steamReleaseId)
            SteamAppId = steamReleaseId[..steamReleaseId.IndexOf('_')];

        AllTitles = rs
            .Select(r => new PlatformTitle(r))
            .OrderBy(o => o.Platform == "steam" ? 0 : 1)
            .ThenBy(o => o.Title.Length)
            .ToList();
        
        Title = AllTitles[0].Title;
        SortingTitle = AllTitles[0].SortingTitle;
        if (Title == SortingTitle)
            SortingTitle = null;
        
        VerticalCover = g.GetString("images", "verticalCover");
        Genres = (IReadOnlyList<string>?)g.GetStrings("meta", "genres")?.ToList() ?? Array.Empty<string>();
        Themes = (IReadOnlyList<string>?)g.GetStrings("meta", "themes")?.ToList() ?? Array.Empty<string>();
        Summary = g.GetString("summary", "summary");
        
        if (g.GetProperty("meta", "releaseDate")?.TryGetInt64(out long ts) == true
            && DateTimeOffset.FromUnixTimeSeconds(ts) is {Year: > 1970} releaseDate)
            ReleaseDate = releaseDate;
        
        Platforms = rs.Select(r => r.Platform).Order().Distinct().ToList();
        Screenshots = (IReadOnlyList<string>?)g.GetStrings("media", "screenshots")
            ?.Select(url => url.Replace("{formatter}", "").Replace("{ext}", "jpg"))
            .ToList() ?? Array.Empty<string>();
    }
}