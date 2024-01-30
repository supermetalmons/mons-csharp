// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

public enum ManaType
{
    Regular,
    Supermana
}

[JsonConverter(typeof(ManaJsonConverter))]
public struct Mana : IEquatable<Mana>
{
    public ManaType Type { get; private set; }
    public Color Color { get; private set; }

    private Mana(ManaType type, Color color)
    {
        Type = type;
        Color = color;
    }

    public static Mana Regular(Color color) => new(ManaType.Regular, color);
    public static Mana Supermana => new(ManaType.Supermana, default);

    public int Score(Color player)
    {
        switch (Type)
        {
            case ManaType.Regular:
                return Color == player ? 1 : 2;
            case ManaType.Supermana:
                return 2;
            default:
                throw new InvalidOperationException("Unknown Mana Type");
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Mana mana && this == mana;
    }

    public bool Equals(Mana other)
    {
        return Type == other.Type && Color == other.Color;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Color);
    }

    public static bool operator ==(Mana left, Mana right)
    {
        return left.Type == right.Type && left.Color == right.Color;
    }

    public static bool operator !=(Mana left, Mana right)
    {
    return !(left == right);
    }
}

public class ManaJsonConverter : JsonConverter<Mana>
{
    public override Mana Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token.");
        }

        reader.Read();

        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException("Expected a PropertyName token.");
        }

        string propertyName = reader.GetString()!;
        Mana mana;

        switch (propertyName)
        {
            case "regular":
                reader.Read(); // Move to StartObject of the "regular" property value
                reader.Read(); // Move to PropertyName ("color")
                if (reader.GetString() != "color")
                    throw new JsonException("Expected 'color' property.");
                reader.Read(); // Move to PropertyValue
                Color color = JsonSerializer.Deserialize<Color>(ref reader, options);
                mana = Mana.Regular(color);
                reader.Read(); // Move past the EndObject
                break;

            case "supermana":
                mana = Mana.Supermana;
                reader.Read(); // Move to StartObject of the "supermana" property value
                reader.Read(); // Move past the EndObject
                break;

            default:
                throw new JsonException($"Unknown Mana type: {propertyName}");
        }

        reader.Read(); // Read EndObject
        return mana;
    }

    public override void Write(Utf8JsonWriter writer, Mana value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        switch (value.Type)
        {
            case ManaType.Regular:
                writer.WritePropertyName("regular");
                writer.WriteStartObject();
                writer.WriteString("color", value.Color.ToString().LowercaseFirst());
                writer.WriteEndObject();
                break;

            case ManaType.Supermana:
                writer.WritePropertyName("supermana");
                writer.WriteStartObject(); // Empty object for supermana
                writer.WriteEndObject();
                break;
        }

        writer.WriteEndObject();
    }
}