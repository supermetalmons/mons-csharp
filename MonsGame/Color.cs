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

public static class ColorExtensions
{

    public static string Fen(this Color color)
    {
        return color switch
        {
            Color.White => "w",
            Color.Black => "b",
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

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

    private static readonly Random _random = new Random();

    public static Color RandomColor()
    {
        Array values = Enum.GetValues(typeof(Color));
        return (Color)values.GetValue(_random.Next(values.Length));
    }
}