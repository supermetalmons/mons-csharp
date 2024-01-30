// ∅ 2024 super-metal-mons

namespace MonsGame;

public abstract class Event
{
    public enum EventType
    {
        MonMove,
        ManaMove,
        ManaScored,
        MysticAction,
        DemonAction,
        DemonAdditionalStep,
        SpiritTargetMove,
        PickupBomb,
        PickupPotion,
        PickupMana,
        MonFainted,
        ManaDropped,
        SupermanaBackToBase,
        BombAttack,
        MonAwake,
        BombExplosion,
        NextTurn,
        GameOver
    }
    public EventType Type { get; protected set; }
}

public class MonMoveEvent : Event
{
    public Item Item { get; }
    public Location From { get; }
    public Location To { get; }

    public MonMoveEvent(Item item, Location from, Location to)
    {
        Item = item;
        From = from;
        To = to;
        Type = EventType.MonMove;
    }
}

public class ManaMoveEvent : Event
{
    public Mana Mana { get; }
    public Location From { get; }
    public Location To { get; }

    public ManaMoveEvent(Mana mana, Location from, Location to)
    {
        Mana = mana;
        From = from;
        To = to;
        Type = EventType.ManaMove;
    }
}

public class ManaScoredEvent : Event
{
    public Mana Mana { get; }
    public Location At { get; }

    public ManaScoredEvent(Mana mana, Location at)
    {
        Mana = mana;
        At = at;
        Type = EventType.ManaScored;
    }
}

public class MysticActionEvent : Event
{
    public Mon Mystic { get; }
    public Location From { get; }
    public Location To { get; }

    public MysticActionEvent(Mon mystic, Location from, Location to)
    {
        Mystic = mystic;
        From = from;
        To = to;
        Type = EventType.MysticAction;
    }
}

public class DemonActionEvent : Event
{
    public Mon Demon { get; }
    public Location From { get; }
    public Location To { get; }

    public DemonActionEvent(Mon demon, Location from, Location to)
    {
        Demon = demon;
        From = from;
        To = to;
        Type = EventType.DemonAction;
    }
}

public class DemonAdditionalStepEvent : Event
{
    public Mon Demon { get; }
    public Location From { get; }
    public Location To { get; }

    public DemonAdditionalStepEvent(Mon demon, Location from, Location to)
    {
        Demon = demon;
        From = from;
        To = to;
        Type = EventType.DemonAdditionalStep;
    }
}

public class SpiritTargetMoveEvent : Event
{
    public Item Item { get; }
    public Location From { get; }
    public Location To { get; }

    public SpiritTargetMoveEvent(Item item, Location from, Location to)
    {
        Item = item;
        From = from;
        To = to;
        Type = EventType.SpiritTargetMove;
    }
}

public class PickupBombEvent : Event
{
    public Mon By { get; }
    public Location At { get; }

    public PickupBombEvent(Mon by, Location at)
    {
        By = by;
        At = at;
        Type = EventType.PickupBomb;
    }
}

public class PickupPotionEvent : Event
{
    public Item By { get; }
    public Location At { get; }

    public PickupPotionEvent(Item by, Location at)
    {
        By = by;
        At = at;
        Type = EventType.PickupPotion;
    }
}

public class PickupManaEvent : Event
{
    public Mana Mana { get; }
    public Mon By { get; }
    public Location At { get; }

    public PickupManaEvent(Mana mana, Mon by, Location at)
    {
        Mana = mana;
        By = by;
        At = at;
        Type = EventType.PickupMana;
    }
}

public class MonFaintedEvent : Event
{
    public Mon Mon { get; }
    public Location From { get; }
    public Location To { get; }

    public MonFaintedEvent(Mon mon, Location from, Location to)
    {
        Mon = mon;
        From = from;
        To = to;
        Type = EventType.MonFainted;
    }
}

public class ManaDroppedEvent : Event
{
    public Mana Mana { get; }
    public Location At { get; }

    public ManaDroppedEvent(Mana mana, Location at)
    {
        Mana = mana;
        At = at;
        Type = EventType.ManaDropped;
    }
}

public class SupermanaBackToBaseEvent : Event
{
    public Location From { get; }
    public Location To { get; }

    public SupermanaBackToBaseEvent(Location from, Location to)
    {
        From = from;
        To = to;
        Type = EventType.SupermanaBackToBase;
    }
}

public class BombAttackEvent : Event
{
    public Mon By { get; }
    public Location From { get; }
    public Location To { get; }

    public BombAttackEvent(Mon by, Location from, Location to)
    {
        By = by;
        From = from;
        To = to;
        Type = EventType.BombAttack;
    }
}

public class MonAwakeEvent : Event
{
    public Mon Mon { get; }
    public Location At { get; }

    public MonAwakeEvent(Mon mon, Location at)
    {
        Mon = mon;
        At = at;
        Type = EventType.MonAwake;
    }
}

public class BombExplosionEvent : Event
{
    public Location At { get; }

    public BombExplosionEvent(Location at)
    {
        At = at;
        Type = EventType.BombExplosion;
    }
}

public class NextTurnEvent : Event
{
    public Color Color { get; }

    public NextTurnEvent(Color color)
    {
        Color = color;
        Type = EventType.NextTurn;
    }
}

public class GameOverEvent : Event
{
    public Color Winner { get; }

    public GameOverEvent(Color winner)
    {
        Winner = winner;
        Type = EventType.GameOver;
    }
}
