// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(EventJsonConverter))]
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

public class EventJsonConverter : JsonConverter<Event>
{
    public override Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement.EnumerateObject().First();
        var eventType = root.Name;
        var eventObj = root.Value;

        return eventType switch
        {
            "monMove" => new MonMoveEvent(
                JsonSerializer.Deserialize<Item>(eventObj.GetProperty("item").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "mysticAction" => new MysticActionEvent(
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("mystic").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "monFainted" => new MonFaintedEvent(
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("mon").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "manaMove" => new ManaMoveEvent(
                JsonSerializer.Deserialize<Mana>(eventObj.GetProperty("mana").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "demonAction" => new DemonActionEvent(
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("demon").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "demonAdditionalStep" => new DemonAdditionalStepEvent(
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("demon").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "spiritTargetMove" => new SpiritTargetMoveEvent(
                JsonSerializer.Deserialize<Item>(eventObj.GetProperty("item").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "pickupBomb" => new PickupBombEvent(
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("by").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("at").GetRawText(), options)),
            "pickupPotion" => new PickupPotionEvent(
                JsonSerializer.Deserialize<Item>(eventObj.GetProperty("by").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("at").GetRawText(), options)),
            "pickupMana" => new PickupManaEvent(
                JsonSerializer.Deserialize<Mana>(eventObj.GetProperty("mana").GetRawText(), options),
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("by").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("at").GetRawText(), options)),
            "supermanaBackToBase" => new SupermanaBackToBaseEvent(
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "bombAttack" => new BombAttackEvent(
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("by").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("from").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("to").GetRawText(), options)),
            "monAwake" => new MonAwakeEvent(
                JsonSerializer.Deserialize<Mon>(eventObj.GetProperty("mon").GetRawText(), options),
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("at").GetRawText(), options)),
            "bombExplosion" => new BombExplosionEvent(
                JsonSerializer.Deserialize<Location>(eventObj.GetProperty("at").GetRawText(), options)),
            "nextTurn" => new NextTurnEvent(
                JsonSerializer.Deserialize<Color>(eventObj.GetProperty("color").GetRawText(), options)),
            "gameOver" => new GameOverEvent(
                JsonSerializer.Deserialize<Color>(eventObj.GetProperty("winner").GetRawText(), options)),
            _ => throw new JsonException($"Event type {eventType} is not supported.")
        };
    }

    public override void Write(Utf8JsonWriter writer, Event value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(value.Type.ToString().LowercaseFirst());
        writer.WriteStartObject();

        switch (value)
        {
            case MonMoveEvent monMoveEvent:
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, monMoveEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, monMoveEvent.To, options);
                writer.WritePropertyName("item");
                JsonSerializer.Serialize(writer, monMoveEvent.Item, options);
                break;
            case MysticActionEvent mysticActionEvent:
                writer.WritePropertyName("mystic");
                JsonSerializer.Serialize(writer, mysticActionEvent.Mystic, options);
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, mysticActionEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, mysticActionEvent.To, options);
                break;
            case MonFaintedEvent monFaintedEvent:
                writer.WritePropertyName("mon");
                JsonSerializer.Serialize(writer, monFaintedEvent.Mon, options);
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, monFaintedEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, monFaintedEvent.To, options);
                break;
            case ManaMoveEvent manaMoveEvent:
                writer.WritePropertyName("mana");
                JsonSerializer.Serialize(writer, manaMoveEvent.Mana, options);
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, manaMoveEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, manaMoveEvent.To, options);
                break;
            case DemonActionEvent demonActionEvent:
                writer.WritePropertyName("demon");
                JsonSerializer.Serialize(writer, demonActionEvent.Demon, options);
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, demonActionEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, demonActionEvent.To, options);
                break;
            case DemonAdditionalStepEvent demonAdditionalStepEvent:
                writer.WritePropertyName("demon");
                JsonSerializer.Serialize(writer, demonAdditionalStepEvent.Demon, options);
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, demonAdditionalStepEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, demonAdditionalStepEvent.To, options);
                break;
            case SpiritTargetMoveEvent spiritTargetMoveEvent:
                writer.WritePropertyName("item");
                JsonSerializer.Serialize(writer, spiritTargetMoveEvent.Item, options);
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, spiritTargetMoveEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, spiritTargetMoveEvent.To, options);
                break;
            case PickupBombEvent pickupBombEvent:
                writer.WritePropertyName("by");
                JsonSerializer.Serialize(writer, pickupBombEvent.By, options);
                writer.WritePropertyName("at");
                JsonSerializer.Serialize(writer, pickupBombEvent.At, options);
                break;
            case PickupPotionEvent pickupPotionEvent:
                writer.WritePropertyName("by");
                JsonSerializer.Serialize(writer, pickupPotionEvent.By, options);
                writer.WritePropertyName("at");
                JsonSerializer.Serialize(writer, pickupPotionEvent.At, options);
                break;
            case PickupManaEvent pickupManaEvent:
                writer.WritePropertyName("mana");
                JsonSerializer.Serialize(writer, pickupManaEvent.Mana, options);
                writer.WritePropertyName("by");
                JsonSerializer.Serialize(writer, pickupManaEvent.By, options);
                writer.WritePropertyName("at");
                JsonSerializer.Serialize(writer, pickupManaEvent.At, options);
                break;
            case SupermanaBackToBaseEvent supermanaBackToBaseEvent:
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, supermanaBackToBaseEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, supermanaBackToBaseEvent.To, options);
                break;
            case BombAttackEvent bombAttackEvent:
                writer.WritePropertyName("by");
                JsonSerializer.Serialize(writer, bombAttackEvent.By, options);
                writer.WritePropertyName("from");
                JsonSerializer.Serialize(writer, bombAttackEvent.From, options);
                writer.WritePropertyName("to");
                JsonSerializer.Serialize(writer, bombAttackEvent.To, options);
                break;
            case MonAwakeEvent monAwakeEvent:
                writer.WritePropertyName("mon");
                JsonSerializer.Serialize(writer, monAwakeEvent.Mon, options);
                writer.WritePropertyName("at");
                JsonSerializer.Serialize(writer, monAwakeEvent.At, options);
                break;
            case BombExplosionEvent bombExplosionEvent:
                writer.WritePropertyName("at");
                JsonSerializer.Serialize(writer, bombExplosionEvent.At, options);
                break;
            case NextTurnEvent nextTurnEvent:
                writer.WritePropertyName("color");
                JsonSerializer.Serialize(writer, nextTurnEvent.Color, options);
                break;
            case GameOverEvent gameOverEvent:
                writer.WritePropertyName("winner");
                JsonSerializer.Serialize(writer, gameOverEvent.Winner, options);
                break;
            default:
                throw new JsonException($"Event type {value.Type} is not supported.");
        }

        writer.WriteEndObject();
        writer.WriteEndObject();
    }

}
