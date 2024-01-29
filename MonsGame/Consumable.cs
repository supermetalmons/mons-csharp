namespace MonsGame;

using System;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))] // This ensures the enum is serialized as a string
public enum Consumable
{
    Potion,
    Bomb,
    BombOrPotion
}
