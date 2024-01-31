// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(NextInputKindJsonConverter))]
public enum NextInputKind
{
    MonMove,
    ManaMove,
    MysticAction,
    DemonAction,
    DemonAdditionalStep,
    SpiritTargetCapture,
    SpiritTargetMove,
    SelectConsumable,
    BombAttack
}

[JsonConverter(typeof(NextInputJsonConverter))]
public struct NextInput : IEquatable<NextInput>
{
    public Input Input { get; }
    public NextInputKind Kind { get; }
    public Item? ActorMonItem { get; }

    public NextInput(Input input, NextInputKind kind, Item? actorMonItem = null)
    {
        Input = input;
        Kind = kind;
        ActorMonItem = actorMonItem;
    }

    public bool Equals(NextInput other)
    {
        return Input.Equals(other.Input) && Kind == other.Kind && Equals(ActorMonItem, other.ActorMonItem);
    }

    public override bool Equals(object? obj)
    {
        return obj is NextInput other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Input, Kind, ActorMonItem);
    }

    public static bool operator ==(NextInput left, NextInput right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NextInput left, NextInput right)
    {
        return !(left == right);
    }
}

public class NextInputKindJsonConverter : JsonConverter<NextInputKind>
{
    public override NextInputKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<NextInputKind>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(NextInputKind)}.");
    }

    public override void Write(Utf8JsonWriter writer, NextInputKind value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}

public class NextInputJsonConverter : JsonConverter<NextInput>
{
    public override NextInput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token.");
        }

        var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        NextInputKind kind = default;
        Input input = default!;
        Item? actorMonItem = null;

        if (root.TryGetProperty("kind", out var kindElement))
        {
            foreach (var kindProperty in kindElement.EnumerateObject())
            {
                kind = Enum.Parse<NextInputKind>(kindProperty.Name, true);
                break;
            }
        }

        if (root.TryGetProperty("input", out var inputElement))
        {
            input = JsonSerializer.Deserialize<Input>(inputElement.GetRawText(), options)!;
        }

        if (root.TryGetProperty("actorMonItem", out var actorMonItemElement))
        {
            actorMonItem = JsonSerializer.Deserialize<Item>(actorMonItemElement.GetRawText(), options);
        }

        return new NextInput(input!, kind, actorMonItem);
    }

    public override void Write(Utf8JsonWriter writer, NextInput value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("kind");
        writer.WriteStartObject();
        writer.WritePropertyName(value.Kind.ToString().LowercaseFirst());
        writer.WriteStartObject();
        writer.WriteEndObject();
        writer.WriteEndObject();
        if (value.ActorMonItem.HasValue)
        {
            writer.WritePropertyName("actorMonItem");
            JsonSerializer.Serialize(writer, value.ActorMonItem.Value, options);
        }
        writer.WritePropertyName("input");
        JsonSerializer.Serialize(writer, value.Input, options);
        writer.WriteEndObject();
    }
}
