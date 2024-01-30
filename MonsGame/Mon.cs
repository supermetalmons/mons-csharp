// ∅ 2024 super-metal-mons

namespace MonsGame;

public struct Mon : IEquatable<Mon>
{
    public enum Kind
    {
        Demon,
        Drainer,
        Angel,
        Spirit,
        Mystic
    }

    public Kind kind { get; private set; }
    public Color color { get; private set; }

    private int _cooldown;
    public int cooldown
    {
        get => _cooldown;
        private set => _cooldown = value;
    }

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
