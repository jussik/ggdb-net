namespace GgdbNet.Shared.Models;

public class Game
{
    public record PlatformTitle(string Platform, string Title, string? SortingTitle);

    public int CollectionId { get; init; }
    public long GameId { get; init; }
    
    public string DateAdded { get; init; } = null!;
    public IReadOnlyList<string> ReleaseIds { get; init; } = null!;
    public long? SteamAppId { get; init; }
    public string? Title { get; init; }
    public string? SortingTitle { get; init; }
    public IReadOnlyList<PlatformTitle> AllTitles { get; init; } = null!;
    public string? VerticalCover { get; init; }
    public IReadOnlyList<string> Genres { get; init; } = null!;
    public IReadOnlyList<string> Themes { get; init; } = null!;
    public string? Summary { get; init; }
    public DateTimeOffset? ReleaseDate { get; init; }
    public IReadOnlyList<string> Platforms { get; init; } = null!;
    public IReadOnlyList<string> Screenshots { get; init; } = null!;
}