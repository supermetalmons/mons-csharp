// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(MonKindJsonConverter))]
    public enum MonKind
    {
        Demon,
        Drainer,
        Angel,
        Spirit,
        Mystic
    }

public struct Mon : IEquatable<Mon>
{
    public Color Color { get; set; }
    private int _cooldown;
    public int Cooldown
    {
        get => _cooldown;
        private set => _cooldown = value;
    }
    public MonKind Kind { get; set; }

    [JsonIgnore]
    public bool IsFainted => Cooldown > 0;

    public Mon(MonKind kind, Color color, int cooldown = 0)
    {
        this.Kind = kind;
        this.Color = color;
        _cooldown = cooldown;
    }

    public void Faint()
    {
        _cooldown = 2;
    }

    public void DecreaseCooldown()
    {
        if (_cooldown > 0)
        {
            _cooldown--;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Mon mon && this == mon;
    }

    public bool Equals(Mon other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Kind, Color, _cooldown);
    }

    public static bool operator ==(Mon left, Mon right)
    {
        return left.Kind == right.Kind && left.Color == right.Color && left._cooldown == right._cooldown;
    }

    public static bool operator !=(Mon left, Mon right)
    {
        return !(left == right);
    }
}

public class MonKindJsonConverter : JsonConverter<MonKind>
{
    public override MonKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<MonKind>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(Mon.Kind)}.");
    }

    public override void Write(Utf8JsonWriter writer, MonKind value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}