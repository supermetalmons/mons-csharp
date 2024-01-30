// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(InputJsonConverter))]
public abstract class Input
{
    public class LocationInput : Input, IEquatable<LocationInput>
    {
        [JsonPropertyName("_0")]
        public Location Location { get; }

        [JsonConstructor]
        public LocationInput(Location location)
        {
            Location = location;
        }

        public bool Equals(LocationInput? other)
        {
            if (other is null) return false;
            return Location.Equals(other.Location);
        }

        public override bool Equals(object? obj) => Equals(obj as LocationInput);

        public override int GetHashCode()
        {
            return HashCode.Combine(Location);
        }
    }

    public class ModifierInput : Input, IEquatable<ModifierInput>
    {
        [JsonPropertyName("_0")]
        [JsonConverter(typeof(ModifierJsonConverter))]
        public Modifier Modifier { get; }

        [JsonConstructor]
        public ModifierInput(Modifier modifier)
        {
            Modifier = modifier;
        }

        public bool Equals(ModifierInput? other)
        {
            if (other is null) return false;
            return Modifier == other.Modifier;
        }

        public override bool Equals(object? obj) => Equals(obj as ModifierInput);

        public override int GetHashCode()
        {
            return HashCode.Combine(Modifier);
        }
    }

    public class InputJsonConverter : JsonConverter<Input>
    {
        public override Input Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                if (doc.RootElement.TryGetProperty("location", out JsonElement locationElement))
                {
                    return JsonSerializer.Deserialize<LocationInput>(locationElement.GetRawText(), options)!;
                }
                else if (doc.RootElement.TryGetProperty("modifier", out JsonElement modifierElement))
                {
                    return JsonSerializer.Deserialize<ModifierInput>(modifierElement.GetRawText(), options)!;
                }
                else
                {
                    throw new JsonException("Unknown Input type.");
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, Input value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        switch (value)
        {
            case ModifierInput modifierInput:
                writer.WritePropertyName("modifier");
                writer.WriteStartObject();
                writer.WritePropertyName("_0");
                JsonSerializer.Serialize(writer, modifierInput.Modifier, options);
                writer.WriteEndObject();
                break;

            case LocationInput locationInput:
                writer.WritePropertyName("location");
                writer.WriteStartObject();
                writer.WritePropertyName("_0");
                JsonSerializer.Serialize(writer, locationInput.Location, options);
                writer.WriteEndObject();
                break;

            default:
                throw new JsonException("Unknown input type.");
        }

        writer.WriteEndObject();
    }
    }
}
