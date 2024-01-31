// ∅ 2024 super-metal-mons

namespace MonsGame;
public partial class Game
{
    public Board Board { get; private set; }

    public int WhiteScore { get; private set; }
    public int BlackScore { get; private set; }
    public Color ActiveColor { get; private set; }

    public int ActionsUsedCount { get; private set; }
    public int ManaMovesCount { get; private set; }
    public int MonsMovesCount { get; private set; }

    public int WhitePotionsCount { get; private set; }
    public int BlackPotionsCount { get; private set; }

    public int TurnNumber { get; private set; }

    public Game()
    {
        Board = new Board();
        WhiteScore = 0;
        BlackScore = 0;
        ActiveColor = Color.White;
        ActionsUsedCount = 0;
        ManaMovesCount = 0;
        MonsMovesCount = 0;
        WhitePotionsCount = 0;
        BlackPotionsCount = 0;
        TurnNumber = 1;
    }

    public Game(Board board,
                    int whiteScore,
                    int blackScore,
                    Color activeColor,
                    int actionsUsedCount,
                    int manaMovesCount,
                    int monsMovesCount,
                    int whitePotionsCount,
                    int blackPotionsCount,
                    int turnNumber)
    {
        Board = board;
        WhiteScore = whiteScore;
        BlackScore = blackScore;
        ActiveColor = activeColor;
        ActionsUsedCount = actionsUsedCount;
        ManaMovesCount = manaMovesCount;
        MonsMovesCount = monsMovesCount;
        WhitePotionsCount = whitePotionsCount;
        BlackPotionsCount = blackPotionsCount;
        TurnNumber = turnNumber;
    }

    public Output ProcessInput(List<Input> inputs, bool doNotApplyEvents, bool oneOptionEnough)
    {
        // TODO: implement
        return new InvalidInputOutput();
    }

    // MARK: - process step by step

    private Output SuggestedInputToStartWith()
    {
        // TODO: implement
        return new InvalidInputOutput();
    }

    private List<NextInput> SecondInputOptions(Location startLocation, Item startItem, bool onlyOne, Input? specificNext = null)
    {
        // TODO: implement
        return new List<NextInput>();
    }

    private (List<Event>, List<NextInput>)? ProcessSecondInput(NextInputKind kind, Item startItem, Location startLocation, Location targetLocation, Input? specificNext = null)
    {
        // TODO: implement
        return null;
    }

    private (List<Event>, List<NextInput>)? ProcessThirdInput(NextInput thirdInput, Item startItem, Location startLocation, Location targetLocation)
    {
        // TODO: implement
        return null;
    }

    private List<Event> ApplyAndAddResultingEvents(List<Event> events)
    {
        // TODO: implement
        return [];
    }

    // MARK: - helpers

    public List<NextInput> NextInputs(IEnumerable<Location> locations, NextInputKind kind, bool onlyOne, Location? specific, Func<Location, bool> filter)
    {
        var inputs = new List<NextInput>();
        if (specific != null)
        {
            if (locations.Contains(specific.Value) && filter(specific.Value))
            {
                var input = new Input.LocationInput(specific.Value);
                inputs.Add(new NextInput(input, kind));
            }
        }
        else if (onlyOne)
        {
            var one = locations.FirstOrDefault(loc => filter(loc));
            if (!one.Equals(default(Location)))
            {
                var input = new Input.LocationInput(one);
                inputs.Add(new NextInput(input, kind));
            }
        }
        else
        {
            inputs.AddRange(locations.Where(loc => filter(loc)).Select(loc =>
            {
                var input = new Input.LocationInput(loc);
                return new NextInput(input, kind);
            }));
        }
        return inputs;
    }


    public Dictionary<AvailableMoveKind, int> AvailableMoveKinds(int monsMovesCount, int actionsUsedCount, int manaMovesCount, int playerPotionsCount, int turnNumber)
    {
        var moves = new Dictionary<AvailableMoveKind, int>
        {
            [AvailableMoveKind.MonMove] = Config.MonsMovesPerTurn - monsMovesCount,
            [AvailableMoveKind.Action] = 0,
            [AvailableMoveKind.Potion] = 0,
            [AvailableMoveKind.ManaMove] = 0
        };

        if (turnNumber != 1)
        {
            moves[AvailableMoveKind.Action] = Config.ActionsPerTurn - actionsUsedCount;
            moves[AvailableMoveKind.Potion] = playerPotionsCount;
            moves[AvailableMoveKind.ManaMove] = Config.ManaMovesPerTurn - manaMovesCount;
        }

        return moves;
    }

    public Color? WinnerColor(int whiteScore, int blackScore)
    {
        if (whiteScore >= Config.TargetScore)
        {
            return Color.White;
        }
        else if (blackScore >= Config.TargetScore)
        {
            return Color.Black;
        }
        else
        {
            return null;
        }
    }

    public bool IsFirstTurn(int turnNumber) => turnNumber == 1;

    public int PlayerPotionsCount(Color activeColor, int whitePotionsCount, int blackPotionsCount)
        => activeColor == Color.White ? whitePotionsCount : blackPotionsCount;

    public bool PlayerCanMoveMon(int monsMovesCount) => monsMovesCount < Config.MonsMovesPerTurn;

    public bool PlayerCanMoveMana(int turnNumber, int manaMovesCount)
        => turnNumber != 1 && manaMovesCount < Config.ManaMovesPerTurn;

    public bool PlayerCanUseAction(int turnNumber, int playerPotionsCount, int actionsUsedCount)
        => turnNumber != 1 && (playerPotionsCount > 0 || actionsUsedCount < Config.ActionsPerTurn);

    public HashSet<Location> ProtectedByOpponentsAngel(Board board, Color activeColor)
    {
        var protectedLocations = new HashSet<Location>();
        var angelLocation = board.FindAwakeAngel(activeColor.Other());
        if (angelLocation != null)
        {
            protectedLocations.UnionWith(angelLocation.Value.NearbyLocations);
        }
        return protectedLocations;
    }

}
