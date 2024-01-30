// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

[Serializable]
public class RulesTestCase
{
    public string FenBefore { get; set; } = string.Empty;
    public List<Input> Input { get; set; } = new List<Input>();
    // public Output Output { get; set; } = new InvalidInputOutput();
    public string FenAfter { get; set; } = string.Empty;

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