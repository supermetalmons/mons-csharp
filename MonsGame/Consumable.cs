// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))] // This ensures the enum is serialized as a string
public enum Consumable
{
    Potion,
    Bomb,
    BombOrPotion
}
