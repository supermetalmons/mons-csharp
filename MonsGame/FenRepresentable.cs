// ∅ 2024 super-metal-mons

namespace MonsGame;

interface IFenRepresentable
{
    string Fen { get; }
}

public partial class Game : IFenRepresentable
{
    public string Fen
    {
        get
        {
            var fields = new string[]
            {
                WhiteScore.ToString(),
                BlackScore.ToString(),
                ActiveColor.Fen(),
                ActionsUsedCount.ToString(),
                ManaMovesCount.ToString(),
                MonsMovesCount.ToString(),
                WhitePotionsCount.ToString(),
                BlackPotionsCount.ToString(),
                TurnNumber.ToString(),
                Board.Fen()
            };
            return string.Join(" ", fields);
        }
    }

}

public static class ConsumableExtensions
{
    public static string Fen(this Consumable consumable)
    {
        return consumable switch
        {
            Consumable.Potion => "P",
            Consumable.Bomb => "B",
            Consumable.BombOrPotion => "Q",
            _ => throw new ArgumentOutOfRangeException(nameof(consumable), consumable, null)
        };
    }

    public static Consumable? FromFen(string fen)
    {
        return fen switch
        {
            "P" => Consumable.Potion,
            "B" => Consumable.Bomb,
            "Q" => Consumable.BombOrPotion,
            _ => null
        };
    }
}

public static class ManaExtensions
{
    public static string Fen(this Mana mana)
    {
        return mana.Type switch
        {
            ManaType.Regular => mana.Color == Color.White ? "M" : "m",
            ManaType.Supermana => "U",
            _ => throw new ArgumentOutOfRangeException(nameof(mana), mana, null)
        };
    }

    public static Mana? FromFen(string fen)
    {
        return fen switch
        {
            "U" => Mana.Supermana,
            "M" => Mana.Regular(Color.White),
            "m" => Mana.Regular(Color.Black),
            _ => null
        };
    }
}

public static class MonExtensions
{
    public static string Fen(this Mon mon)
    {
        var letter = mon.kind switch
        {
            Mon.Kind.Demon => "e",
            Mon.Kind.Drainer => "d",
            Mon.Kind.Angel => "a",
            Mon.Kind.Spirit => "s",
            Mon.Kind.Mystic => "y",
            _ => throw new ArgumentOutOfRangeException(nameof(mon.kind), mon.kind, null)
        };

        var cooldown = mon.cooldown % 10;
        return (mon.color == Color.White ? letter.ToUpper() : letter) + cooldown.ToString();
    }

    public static Mon? FromFen(string fen)
    {
        if (fen.Length != 2 || !int.TryParse(fen[1].ToString(), out int cooldown))
        {
            return null;
        }

        Mon.Kind kind;
        switch (fen[0].ToString().ToLower())
        {
            case "e": kind = Mon.Kind.Demon; break;
            case "d": kind = Mon.Kind.Drainer; break;
            case "a": kind = Mon.Kind.Angel; break;
            case "s": kind = Mon.Kind.Spirit; break;
            case "y": kind = Mon.Kind.Mystic; break;
            default: return null;
        }

        var color = char.IsUpper(fen[0]) ? Color.White : Color.Black;
        return new Mon(kind, color, cooldown);
    }
}

public static class ItemExtensions
{
    public static string Fen(this Item item)
    {
        string monFen = "xx";
        string itemFen = "x";

        switch (item.Type)
        {
            case ItemType.Mon:
                monFen = item.Mon.Fen();
                break;
            case ItemType.Mana:
                itemFen = item.Mana.Fen();
                break;
            case ItemType.MonWithMana:
                monFen = item.Mon.Fen();
                itemFen = item.Mana.Fen();
                break;
            case ItemType.MonWithConsumable:
                monFen = item.Mon.Fen();
                itemFen = item.Consumable.Fen();
                break;
            case ItemType.Consumable:
                itemFen = item.Consumable.Fen();
                break;
        }

        return monFen + itemFen;
    }

    public static Item? FromFen(string fen)
    {
        if (string.IsNullOrWhiteSpace(fen) || fen.Length != 3)
        {
            return null;
        }

        string monFen = fen.Substring(0, 2);
        string itemFen = fen.Substring(2, 1);

        Mon? mon = monFen != "xx" ? MonExtensions.FromFen(monFen) : null;
        Mana? mana = ManaExtensions.FromFen(itemFen);
        Consumable? consumable = ConsumableExtensions.FromFen(itemFen);

        if (mon.HasValue)
        {
            if (mana.HasValue)
            {
                return Item.MonWithManaItem(mon.Value, mana.Value);
            }
            else if (consumable.HasValue)
            {
                return Item.MonWithConsumableItem(mon.Value, consumable.Value);
            }
            else
            {
                return Item.MonItem(mon.Value);
            }
        }
        else
        {
            if (mana.HasValue)
            {
                return Item.ManaItem(mana.Value);
            }
            else if (consumable.HasValue)
            {
                return Item.ConsumableItem(consumable.Value);
            }
        }

        return null;
    }
}

public static partial class ColorExtensions
{
    public static string Fen(this Color color)
    {
        return color switch
        {
            Color.White => "w",
            Color.Black => "b",
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public static Color? FromFen(string fen)
    {
        return fen switch
        {
            "w" => Color.White,
            "b" => Color.Black,
            _ => null
        };
    }
}

public static class BoardExtensions
{
    public static string Fen(this Board board)
    {
        var lines = new List<string>();
        var squares = new Item?[Config.BoardSize, Config.BoardSize];

        foreach (var location in board.Items.Keys)
        {
            squares[location.I, location.J] = board.Items[location];
        }

        for (int i = 0; i < Config.BoardSize; i++)
        {
            var line = "";
            var emptySpaceCount = 0;
            for (int j = 0; j < Config.BoardSize; j++)
            {
                var item = squares[i, j];
                var itemFen = item?.Fen() ?? "";
                if (string.IsNullOrEmpty(itemFen))
                {
                    emptySpaceCount++;
                }
                else
                {
                    if (emptySpaceCount > 0)
                    {
                        line += $"n{emptySpaceCount:D2}";
                        emptySpaceCount = 0;
                    }
                    line += itemFen;
                }
            }

            if (emptySpaceCount > 0)
            {
                line += $"n{emptySpaceCount:D2}";
            }

            lines.Add(line);
        }
        return string.Join("/", lines);
    }

    public static Board FromFen(string fen)
    {
        var lines = fen.Split('/');
        if (lines.Length != Config.BoardSize)
        {
            throw new ArgumentException("Invalid FEN string for Board.");
        }

        var items = new Dictionary<Location, Item>();
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line))
            {
                throw new ArgumentException("Invalid FEN string for Board.");
            }

            int j = 0;
            while (j < line.Length)
            {
                if (line[j] == 'n')
                {
                    if (j + 2 >= line.Length || !int.TryParse(line.Substring(j + 1, 2), out int emptyCount))
                    {
                        throw new ArgumentException("Invalid FEN string for Board.");
                    }
                    j += 3; // Skip over 'n' and two digits
                }
                else
                {
                    var itemFen = line.Substring(j, 3);
                    var item = ItemExtensions.FromFen(itemFen);
                    if (item == null)
                    {
                        throw new ArgumentException("Invalid FEN string for Board.");
                    }
                    items.Add(new Location(i, j / 3), item.Value);
                    j += 3;
                }
            }
        }

        return new Board(items);
    }
}
