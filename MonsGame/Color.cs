// ∅ 2024 super-metal-mons

namespace MonsGame;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;

[JsonConverter(typeof(JsonStringEnumConverter))]
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