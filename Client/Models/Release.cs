using System.Text.Json;

namespace GgdbNet.Client.Models;

public record Release(string GameId, string Platform, string AddedDate)
{
    public Dictionary<string, JsonElement> Properties { get; } = new();

    public JsonElement? GetProperty(string type, string propName)
        => Properties.TryGetValue(type, out var elem)
           && elem.TryGetProperty(propName, out var prop)
           && prop.ValueKind != JsonValueKind.Null
            ? prop : null;

    public string? GetString(string type, string propName) => GetProperty(type, propName)?.GetString();

    public IEnumerable<string>? GetStrings(string type, string propName)
        => GetProperty(type, propName)?.EnumerateArray()
            .Select(v => v.GetString())
            .Where(s => s != null)
            .Cast<string>();
}