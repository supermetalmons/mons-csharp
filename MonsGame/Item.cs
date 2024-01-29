// ∅ 2024 super-metal-mons

namespace MonsGame;

public enum ItemType
{
    Mon,
    Mana,
    MonWithMana,
    MonWithConsumable,
    Consumable
}

public struct Item : IEquatable<Item>
{
    public ItemType Type { get; private set; }
    public Mon Mon { get; private set; }
    public Mana Mana { get; private set; }
    public Consumable Consumable { get; private set; }

    private Item(ItemType type, Mon mon, Mana mana, Consumable consumable)
    {
        Type = type;
        Mon = mon;
        Mana = mana;
        Consumable = consumable;
    }

    public static Item MonItem(Mon mon) => new Item(ItemType.Mon, mon, default, default);
    public static Item ManaItem(Mana mana) => new Item(ItemType.Mana, default, mana, default);
    public static Item MonWithManaItem(Mon mon, Mana mana) => new Item(ItemType.MonWithMana, mon, mana, default);
    public static Item MonWithConsumableItem(Mon mon, Consumable consumable) => new Item(ItemType.MonWithConsumable, mon, default, consumable);
    public static Item ConsumableItem(Consumable consumable) => new Item(ItemType.Consumable, default, default, consumable);

    public Mon? MonProperty => Type switch
    {
        ItemType.Mon => Mon,
        ItemType.MonWithMana => Mon,
        ItemType.MonWithConsumable => Mon,
        _ => null
    };

    public Mana? ManaProperty => Type switch
    {
        ItemType.Mana => Mana,
        ItemType.MonWithMana => Mana,
        _ => null
    };

    public Consumable? ConsumableProperty => Type switch
    {
        ItemType.MonWithConsumable => Consumable,
        ItemType.Consumable => Consumable,
        _ => null
    };

    public override bool Equals(object obj)
    {
        return obj is Item item && this == item;
    }

    public bool Equals(Item other)
    {
        return Type == other.Type &&
               Mon.Equals(other.Mon) &&
               Mana.Equals(other.Mana) &&
               Consumable.Equals(other.Consumable);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Mon, Mana, Consumable);
    }

    public static bool operator ==(Item left, Item right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Item left, Item right)
    {
        return !(left == right);
    }
}
