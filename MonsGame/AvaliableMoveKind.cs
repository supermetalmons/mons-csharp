// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(AvailableMoveKindJsonConverter))]
public enum AvailableMoveKind
{
    MonMove,
    ManaMove,
    Action,
    Potion
}

public class AvailableMoveKindJsonConverter : JsonConverter<AvailableMoveKind>
{
    public override AvailableMoveKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<AvailableMoveKind>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(AvailableMoveKind)}.");
    }

    public override void Write(Utf8JsonWriter writer, AvailableMoveKind value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}