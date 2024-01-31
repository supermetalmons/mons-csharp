// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;

[JsonConverter(typeof(ItemTypeJsonConverter))]
public enum ItemType
{
    Mon,
    Mana,
    MonWithMana,
    MonWithConsumable,
    Consumable
}

[JsonConverter(typeof(ItemJsonConverter))]
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

    public static Item MonItem(Mon mon) => new(ItemType.Mon, mon, default, default);
    public static Item ManaItem(Mana mana) => new(ItemType.Mana, default, mana, default);
    public static Item MonWithManaItem(Mon mon, Mana mana) => new(ItemType.MonWithMana, mon, mana, default);
    public static Item MonWithConsumableItem(Mon mon, Consumable consumable) => new(ItemType.MonWithConsumable, mon, default, consumable);
    public static Item ConsumableItem(Consumable consumable) => new(ItemType.Consumable, default, default, consumable);

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

    public override bool Equals(object? obj)
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

public class ItemTypeJsonConverter : JsonConverter<ItemType>
{
    public override ItemType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString()!;
        return Enum.TryParse<ItemType>(value, true, out var result) ? result : throw new JsonException($"Value '{value}' is not valid for enum type {nameof(ItemType)}.");
    }

    public override void Write(Utf8JsonWriter writer, ItemType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().LowercaseFirst());
    }
}

public class ItemJsonConverter : JsonConverter<Item>
{
    public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
{
    if (reader.TokenType != JsonTokenType.StartObject)
    {
        throw new JsonException("Expected StartObject token.");
    }

    var document = JsonDocument.ParseValue(ref reader);
    var root = document.RootElement;

    Item item = default;

    foreach (var property in root.EnumerateObject())
    {
        switch (property.Name)
        {
            case "monWithConsumable":
                var mon = JsonSerializer.Deserialize<Mon>(property.Value.GetProperty("mon").GetRawText(), options);
                var consumable = JsonSerializer.Deserialize<Consumable>($"\"{property.Value.GetProperty("consumable").GetString()!}\"", options);
                item = Item.MonWithConsumableItem(mon, consumable);
                break;
            case "monWithMana":
                mon = JsonSerializer.Deserialize<Mon>(property.Value.GetProperty("mon").GetRawText(), options);
                var mana = JsonSerializer.Deserialize<Mana>(property.Value.GetProperty("mana").GetRawText(), options);
                item = Item.MonWithManaItem(mon, mana);
                break;
            case "mana":
                var manac = JsonSerializer.Deserialize<Mana>(property.Value.GetProperty("mana").GetRawText(), options);
                item = Item.ManaItem(manac);
                break;
            case "consumable":
                var consumableString = property.Value.GetProperty("consumable").GetString();
                var consumablec = JsonSerializer.Deserialize<Consumable>($"\"{consumableString}\"", options);
                item = Item.ConsumableItem(consumablec);
                break;
            case "mon":
                mon = JsonSerializer.Deserialize<Mon>(property.Value.GetProperty("mon").GetRawText(), options);
                item = Item.MonItem(mon);
                break;
            default:
                throw new JsonException($"Unknown property {property.Name}.");
        }
    }

    if (item.Equals(default(Item)))
    {
        throw new JsonException("Item deserialization failed.");
    }

    return item;
}


    public override void Write(Utf8JsonWriter writer, Item item, JsonSerializerOptions options)
{
    writer.WriteStartObject();

    switch (item.Type)
    {
        case ItemType.MonWithConsumable:
            writer.WritePropertyName("monWithConsumable");
            writer.WriteStartObject();
            writer.WritePropertyName("consumable");
            writer.WriteStringValue(item.Consumable.ToString().LowercaseFirst());
            writer.WritePropertyName("mon");
            JsonSerializer.Serialize(writer, item.Mon, options);
            writer.WriteEndObject();
            break;

        case ItemType.MonWithMana:
            writer.WritePropertyName("monWithMana");
            writer.WriteStartObject();
            writer.WritePropertyName("mana");
            JsonSerializer.Serialize(writer, item.Mana, options);
            writer.WritePropertyName("mon");
            JsonSerializer.Serialize(writer, item.Mon, options);
            writer.WriteEndObject();
            break;

        case ItemType.Mana:
            writer.WritePropertyName("mana");
            writer.WriteStartObject();
            writer.WritePropertyName("mana");
            JsonSerializer.Serialize(writer, item.Mana, options);
            writer.WriteEndObject();
            break;

        case ItemType.Consumable:
            writer.WritePropertyName("consumable");
            writer.WriteStartObject();
            writer.WritePropertyName("consumable");
            writer.WriteStringValue(item.Consumable.ToString().LowercaseFirst());
            writer.WriteEndObject();
            break;

        case ItemType.Mon:
            writer.WritePropertyName("mon");
            writer.WriteStartObject();
            writer.WritePropertyName("mon");
            JsonSerializer.Serialize(writer, item.Mon, options);
            writer.WriteEndObject();
            break;
    }

    writer.WriteEndObject();
}

}
