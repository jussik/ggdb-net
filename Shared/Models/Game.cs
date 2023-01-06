namespace GgdbNet.Shared.Models;

public class Game
{
    public record PlatformTitle(string Platform, string Title, string? SortingTitle);

    public string GameId { get; set; }
    public string DateAdded { get; set; }
    public IReadOnlyList<string> ReleaseIds { get; set; }
    public string? SteamAppId { get; set; }
    public string? Title { get; set; }
    public string? SortingTitle { get; set; }
    public IReadOnlyList<PlatformTitle> AllTitles { get; set; }
    public string? VerticalCover { get; set; }
    public IReadOnlyList<string> Genres { get; set; }
    public IReadOnlyList<string> Themes { get; set; }
    public string? Summary { get; set; }
    public DateTimeOffset? ReleaseDate { get; set; }
    public IReadOnlyList<string> Platforms { get; set; }
    public IReadOnlyList<string> Screenshots { get; set; }
}