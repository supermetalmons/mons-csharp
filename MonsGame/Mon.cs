// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

public struct Mon : IEquatable<Mon>
{
    [JsonConverter(typeof(MonKindJsonConverter))]
    public enum Kind
    {
        Demon,
        Drainer,
        Angel,
        Spirit,
        Mystic
    }

    public Color color { get; set; }
    private int _cooldown;
    public int cooldown
    {
        get => _cooldown;
        private set => _cooldown = value;
    }
    public Kind kind { get; set; }

    [JsonIgnore]
    public bool isFainted => cooldown > 0;

    public Mon(Kind kind, Color color, int cooldown = 0)
    {
        this.kind = kind;
        this.color = color;
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
        return HashCode.Combine(kind, color, _cooldown);
    }

    public static bool operator ==(Mon left, Mon right)
    {
        return left.kind == right.kind && left.color == right.color && left._cooldown == right._cooldown;
    }

    public static bool operator !=(Mon left, Mon right)
    {
        return !(left == right);
    }
}

public class MonKindJsonConverter : JsonConverter<Mon.Kind>
{
    public override Mon.Kind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<Mon.Kind>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(Mon.Kind)}.");
    }

    public override void Write(Utf8JsonWriter writer, Mon.Kind value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}