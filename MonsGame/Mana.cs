// ∅ 2024 super-metal-mons

namespace MonsGame;

public enum ManaType
{
    Regular,
    Supermana
}

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
