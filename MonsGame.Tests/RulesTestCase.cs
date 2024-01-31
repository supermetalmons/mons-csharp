// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

[Serializable]
public class RulesTestCase
{
    public string FenBefore { get; set; } = string.Empty;
    public string FenAfter { get; set; } = string.Empty;
    public Output Output { get; set; } = default!;
    public List<Input> Input { get; set; } = new List<Input>();

    public RulesTestCase()
    {
    }
}

public static class JsonOptions
{
    public static JsonSerializerOptions DefaultSerializerOptions { get; } = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}