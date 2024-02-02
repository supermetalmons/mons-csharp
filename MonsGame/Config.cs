// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Collections.Generic;
using System.Linq;

public static class Config
{
    public static readonly int BoardSize = 11;
    public static readonly int TargetScore = 5;

    public static readonly int MonsMovesPerTurn = 5;
    public static readonly int ManaMovesPerTurn = 1;
    public static readonly int ActionsPerTurn = 1;

    public static readonly Dictionary<Location, Square> Squares = new()
    {
        [new Location(0, 0)] = Square.ManaPool(Color.Black),
        [new Location(0, 10)] = Square.ManaPool(Color.Black),
        [new Location(10, 0)] = Square.ManaPool(Color.White),
        [new Location(10, 10)] = Square.ManaPool(Color.White),

        [new Location(0, 3)] = Square.MonBase(MonKind.Mystic, Color.Black),
        [new Location(0, 4)] = Square.MonBase(MonKind.Spirit, Color.Black),
        [new Location(0, 5)] = Square.MonBase(MonKind.Drainer, Color.Black),
        [new Location(0, 6)] = Square.MonBase(MonKind.Angel, Color.Black),
        [new Location(0, 7)] = Square.MonBase(MonKind.Demon, Color.Black),

        [new Location(10, 3)] = Square.MonBase(MonKind.Demon, Color.White),
        [new Location(10, 4)] = Square.MonBase(MonKind.Angel, Color.White),
        [new Location(10, 5)] = Square.MonBase(MonKind.Drainer, Color.White),
        [new Location(10, 6)] = Square.MonBase(MonKind.Spirit, Color.White),
        [new Location(10, 7)] = Square.MonBase(MonKind.Mystic, Color.White),

        [new Location(3, 4)] = Square.ManaBase(Color.Black),
        [new Location(3, 6)] = Square.ManaBase(Color.Black),
        [new Location(7, 4)] = Square.ManaBase(Color.White),
        [new Location(7, 6)] = Square.ManaBase(Color.White),

        [new Location(4, 3)] = Square.ManaBase(Color.Black),
        [new Location(4, 5)] = Square.ManaBase(Color.Black),
        [new Location(4, 7)] = Square.ManaBase(Color.Black),
        [new Location(6, 3)] = Square.ManaBase(Color.White),
        [new Location(6, 5)] = Square.ManaBase(Color.White),
        [new Location(6, 7)] = Square.ManaBase(Color.White),

        [new Location(5, 0)] = Square.ConsumableBase,
        [new Location(5, 10)] = Square.ConsumableBase,
        [new Location(5, 5)] = Square.SupermanaBase
    };


    public static readonly int BoardCenterIndex = BoardSize / 2;
    public static readonly int MaxLocationIndex = BoardSize - 1;

    public static readonly Dictionary<Location, Item> InitialItems = Squares
        .Where(kv => kv.Value.Type != SquareType.Regular && kv.Value.Type != SquareType.ManaPool)
        .ToDictionary(
            kv => kv.Key,
            kv => kv.Value.Type switch
            {
                SquareType.MonBase => Item.MonItem(new Mon(kv.Value.Kind, kv.Value.Color)),
                SquareType.ManaBase => Item.ManaItem(Mana.Regular(kv.Value.Color)),
                SquareType.SupermanaBase => Item.ManaItem(Mana.Supermana),
                SquareType.ConsumableBase => Item.ConsumableItem(Consumable.BombOrPotion),
                _ => throw new InvalidOperationException("Invalid square type for initial item.")
            }
        );

    public static readonly HashSet<Location> MonsBases = new(
        Squares.Where(kv => kv.Value.Type == SquareType.MonBase).Select(kv => kv.Key)
    );

}
