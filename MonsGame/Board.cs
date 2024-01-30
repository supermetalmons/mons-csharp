// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Collections.Generic;

public partial class Board
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
    
    public IEnumerable<Location> AllMonsLocations(Color color)
    {
        return _items.Where(pair => pair.Value.Type == ItemType.Mon && pair.Value.Mon.color == color)
                     .Select(pair => pair.Key);
    }

    public IEnumerable<Location> AllFreeRegularManaLocations(Color color)
    {
        return _items.Where(pair => pair.Value.Type == ItemType.Mana && pair.Value.Mana.Type == ManaType.Regular && pair.Value.Mana.Color == color)
                     .Select(pair => pair.Key);
    }

    public Location Base(Mon mon)
    {
        var baseLocation = Config.Squares
            .FirstOrDefault(pair => pair.Value.Type == SquareType.MonBase && 
                                    pair.Value.Color == mon.color && 
                                    pair.Value.Kind == mon.kind);

        if (baseLocation.Equals(default(KeyValuePair<Location, Square>)))
        {
            throw new InvalidOperationException("Mon base not found.");
        }

        return baseLocation.Key;
    }

    public IEnumerable<Location> FaintedMonsLocations(Color color)
    {
        return _items.Where(pair => (pair.Value.Type == ItemType.Mon || pair.Value.Type == ItemType.MonWithMana || pair.Value.Type == ItemType.MonWithConsumable) && 
                                    pair.Value.Mon.color == color && 
                                    pair.Value.Mon.isFainted)
                     .Select(pair => pair.Key);
    }

    public Location? FindMana(Color color)
    {
        var locationWithItem = _items.FirstOrDefault(pair => pair.Value.Type == ItemType.Mana && pair.Value.Mana.Type == ManaType.Regular && pair.Value.Mana.Color == color);
        return locationWithItem.Key.Equals(default(Location)) ? null : locationWithItem.Key;
    }

    public Location? FindAwakeAngel(Color color)
    {
        var locationWithItem = _items.FirstOrDefault(pair => (pair.Value.Type == ItemType.Mon || pair.Value.Type == ItemType.MonWithConsumable) && 
                                                             pair.Value.Mon.color == color && 
                                                             pair.Value.Mon.kind == Mon.Kind.Angel && 
                                                             !pair.Value.Mon.isFainted);
        return locationWithItem.Key.Equals(default(Location)) ? null : locationWithItem.Key;
    }

}
