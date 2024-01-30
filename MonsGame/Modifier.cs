// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(ModifierJsonConverter))]
public enum Modifier
{
    SelectPotion,
    SelectBomb,
    Cancel
}

public class ModifierJsonConverter : JsonConverter<Modifier>
{
    public override Modifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<Modifier>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(Modifier)}.");
    }

    public override void Write(Utf8JsonWriter writer, Modifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}