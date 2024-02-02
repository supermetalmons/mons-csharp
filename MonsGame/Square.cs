// ∅ 2024 super-metal-mons

namespace MonsGame;

public enum SquareType
{
    Regular,
    ConsumableBase,
    SupermanaBase,
    ManaBase,
    ManaPool,
    MonBase
}

public struct Square : IEquatable<Square>
{
    public SquareType Type { get; private set; }
    public Color Color { get; private set; }
    public MonKind Kind { get; private set; }

    private Square(SquareType type, Color color, MonKind kind)
    {
        Type = type;
        Color = color;
        Kind = kind;
    }

    public static Square Regular => new(SquareType.Regular, default, default);
    public static Square ConsumableBase => new(SquareType.ConsumableBase, default, default);
    public static Square SupermanaBase => new(SquareType.SupermanaBase, default, default);
    public static Square ManaBase(Color color) => new(SquareType.ManaBase, color, default);
    public static Square ManaPool(Color color) => new(SquareType.ManaPool, color, default);
    public static Square MonBase(MonKind kind, Color color) => new(SquareType.MonBase, color, kind);

    public override bool Equals(object? obj)
    {
        return obj is Square square && this == square;
    }

    public bool Equals(Square other)
    {
        return Type == other.Type && Color == other.Color && Kind == other.Kind;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Color, Kind);
    }

    public static bool operator ==(Square left, Square right)
    {
        return left.Type == right.Type && left.Color == right.Color && left.Kind == right.Kind;
    }

    public static bool operator !=(Square left, Square right)
    {
        return !(left == right);
    }
}
