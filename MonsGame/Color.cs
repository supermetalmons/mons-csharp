// ∅ 2024 super-metal-mons

namespace MonsGame;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

[JsonConverter(typeof(ColorJsonConverter))]
public enum Color
{
    White,
    Black
}

public static partial class ColorExtensions
{

    public static Color Other(this Color color)
    {
        switch (color)
        {
            case Color.Black:
                return Color.White;
            case Color.White:
                return Color.Black;
            default:
                throw new ArgumentOutOfRangeException(nameof(color), color, null);
        }
    }

    private static readonly Random _random = new();

    public static Color RandomColor()
    {
        Array values = Enum.GetValues(typeof(Color));
        var index = _random.Next(values.Length);
        var value = values.GetValue(index) as Color?;

        if (value.HasValue)
        {
            return value.Value;
        }

        throw new InvalidOperationException("Random value is not a valid Color.");
    }
}

public class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<Color>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(Modifier)}.");
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}