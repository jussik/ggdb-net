using GgdbNet.Shared.Models;

namespace GgdbNet.Client.Models;

public static class GameFactory
{
    public static Game? Create(IEnumerable<Release> releases)
    {
        List<Release> rs = releases.Where(g => g.GetString("images", "verticalCover") != null)
            .ToList();

        if (rs.Count == 0)
            return null;
        
        Release g = rs[0];
        List<string> releaseIds = g.GetStrings("allGameReleases", "releases")?.ToList() ?? new();
        List<Game.PlatformTitle> allTitles = rs
            .Select(r => new Game.PlatformTitle(
                r.Platform,
                r.GetString("title", "title")!,
                r.GetString("sortingTitle", "title")))
            .OrderBy(o => o.Platform == "steam" ? 0 : 1)
            .ThenBy(o => o.Title.Length)
            .ToList();
        (_, string title, string? sortingTitle) = allTitles[0];
        
        return new()
        {
            GameId = g.GameId,
            DateAdded = rs.Min(r => r.AddedDate)!,
            ReleaseIds = releaseIds,
            SteamAppId = releaseIds.FirstOrDefault(r => r.StartsWith("steam_")) is { } steamReleaseId
                ? long.Parse(steamReleaseId[(steamReleaseId.IndexOf('_') + 1)..])
                : null,
            AllTitles = allTitles,
            Title = title,
            SortingTitle = title != sortingTitle ? sortingTitle : null,
            VerticalCover = g.GetString("images", "verticalCover"),
            Genres = (IReadOnlyList<string>?) g.GetStrings("meta", "genres")?.ToList() ?? Array.Empty<string>(),
            Themes = (IReadOnlyList<string>?) g.GetStrings("meta", "themes")?.ToList() ?? Array.Empty<string>(),
            Summary = g.GetString("summary", "summary"),
            ReleaseDate = g.GetProperty("meta", "releaseDate")?.TryGetInt64(out long ts) == true
                          && DateTimeOffset.FromUnixTimeSeconds(ts) is {Year: > 1970} releaseDate
                ? releaseDate
                : null,
            Platforms = rs.Select(r => r.Platform).Order().Distinct().ToList(),
            Screenshots = (IReadOnlyList<string>?) g.GetStrings("media", "screenshots")
                ?.Select(url => url.Replace("{formatter}", "").Replace("{ext}", "jpg"))
                .ToList() ?? Array.Empty<string>()
        };
    }
}