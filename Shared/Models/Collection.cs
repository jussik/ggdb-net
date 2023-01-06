namespace GgdbNet.Shared.Models;

public class Collection
{
    public int Id { get; set; }
    public string PublicId { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public IReadOnlyList<Game>? Games { get; set; }
}