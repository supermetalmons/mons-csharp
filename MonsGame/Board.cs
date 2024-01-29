// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Collections.Generic;

public class Board
{
    private Dictionary<Location, Item> _items;

    public IReadOnlyDictionary<Location, Item> Items { get { return _items; } }

    public Board(Dictionary<Location, Item> items = null)
    {
        _items = items ?? Config.InitialItems;
    }

    public void RemoveItem(Location location)
    {
        _items.Remove(location);
    }

    public void Put(Item item, Location location)
    {
        _items[location] = item;
    }

    public Item GetItem(Location location)
    {
        _items.TryGetValue(location, out Item item);
        return item;
    }

    public Square SquareAt(Location location)
    {
        return Config.Squares.TryGetValue(location, out Square square) ? square : Square.Regular;
    }

    public IEnumerable<Location> AllMonsBases
    {
        get
        {
            return Config.Squares
                .Where(pair => pair.Value.Type == SquareType.MonBase)
                .Select(pair => pair.Key);
        }
    }

    public Location SupermanaBase
    {
        get
        {
            var baseLocation = Config.Squares
                .FirstOrDefault(pair => pair.Value.Type == SquareType.SupermanaBase);

            if (baseLocation.Equals(default(KeyValuePair<Location, Square>)))
            {
                throw new InvalidOperationException("Supermana base not found.");
            }

            return baseLocation.Key;
        }
    }

}
