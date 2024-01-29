// ∅ 2024 super-metal-mons

namespace MonsGame;

public enum NextInputKind
{
    MonMove,
    ManaMove,
    MysticAction,
    DemonAction,
    DemonAdditionalStep,
    SpiritTargetCapture,
    SpiritTargetMove,
    SelectConsumable,
    BombAttack
}

public struct NextInput : IEquatable<NextInput>
{
    public Input Input { get; }
    public NextInputKind Kind { get; }
    public Item? ActorMonItem { get; }

    public NextInput(Input input, NextInputKind kind, Item? actorMonItem = null)
    {
        Input = input;
        Kind = kind;
        ActorMonItem = actorMonItem;
    }

    public bool Equals(NextInput other)
    {
        return Input.Equals(other.Input) && Kind == other.Kind && Equals(ActorMonItem, other.ActorMonItem);
    }

    public override bool Equals(object obj)
    {
        return obj is NextInput other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Input, Kind, ActorMonItem);
    }

    public static bool operator ==(NextInput left, NextInput right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NextInput left, NextInput right)
    {
        return !(left == right);
    }
}
