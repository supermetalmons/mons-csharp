// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(ConsumableJsonConverter))]
public enum Consumable
{
    Potion,
    Bomb,
    BombOrPotion
}

public class ConsumableJsonConverter : JsonConverter<Consumable>
{
    public override Consumable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<Consumable>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(Consumable)}.");
    }

    public override void Write(Utf8JsonWriter writer, Consumable value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}