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
        var targetItem = Board.GetItem(targetLocation);
        var forthInputOptions = new List<NextInput>();
        var events = new List<Event>();

        switch (thirdInput.Kind)
        {
            case NextInputKind.MonMove:
            case NextInputKind.ManaMove:
            case NextInputKind.MysticAction:
            case NextInputKind.DemonAction:
            case NextInputKind.SpiritTargetCapture:
            case NextInputKind.BombAttack:
                return null;

            case NextInputKind.SpiritTargetMove:
                if (!(thirdInput.Input is Input.LocationInput locationInput) || targetItem == null) return null;

                var destinationLocation = locationInput.Location;
                var destinationItem = Board.GetItem(destinationLocation);
                var destinationSquare = Board.SquareAt(destinationLocation);

                events.Add(new SpiritTargetMoveEvent(targetItem, targetLocation, destinationLocation));

                if (destinationItem != null)
                {
                    switch (targetItem.Type)
                    {
                        case ItemType.Mon when destinationItem.Type is ItemType.Mana:
                            events.Add(new PickupManaEvent(destinationItem.Mana, targetItem.Mon, destinationLocation));
                            break;

                        case ItemType.Mon when destinationItem.Type is ItemType.Consumable && destinationItem.Consumable == Consumable.BombOrPotion:
                            forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectBomb), NextInputKind.SelectConsumable, targetItem));
                            forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectPotion), NextInputKind.SelectConsumable, targetItem));
                            break;

                        case ItemType.Mana when destinationItem.Type is ItemType.Mon:
                            events.Add(new PickupManaEvent(targetItem.Mana, destinationItem.Mon, destinationLocation));
                            break;

                        case ItemType.Consumable when destinationItem.Type is ItemType.Mon && targetItem.Consumable == Consumable.BombOrPotion:
                            forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectBomb), NextInputKind.SelectConsumable, destinationItem));
                            forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectPotion), NextInputKind.SelectConsumable, destinationItem));
                            break;
                    }

                    if (destinationSquare.Type == SquareType.ManaPool && targetItem.Mana != null)
                    {
                        events.Add(new ManaScoredEvent(targetItem.Mana, destinationLocation));
                    }
                }
                break;

            case NextInputKind.DemonAdditionalStep:
                if (thirdInput.Input is not Input.LocationInput demonStepInput || startItem.Mon == null) return null;
                var destinationLocationDemonStep = demonStepInput.Location;
                events.Add(new DemonAdditionalStepEvent(startItem.Mon, targetLocation, destinationLocationDemonStep));

                var itemAtDestination = Board.GetItem(destinationLocationDemonStep);
                if (itemAtDestination.Type == ItemType.Consumable && itemAtDestination.Consumable == Consumable.BombOrPotion)
                {
                    forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectBomb), NextInputKind.SelectConsumable, startItem));
                    forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectPotion), NextInputKind.SelectConsumable, startItem));
                }
                break;


            case NextInputKind.SelectConsumable:
                if (!(thirdInput.Input is Input.ModifierInput modifierInput) || startItem.Mon == null) return null;
                switch (modifierInput.Modifier)
                {
                    case Modifier.SelectBomb:
                        events.Add(new PickupBombEvent(startItem.Mon, targetLocation));
                        break;
                    case Modifier.SelectPotion:
                        events.Add(new PickupPotionEvent(startItem, targetLocation));
                        break;
                    case Modifier.Cancel:
                        return null;
                }
                break;
        }

        return (events, forthInputOptions);
    }

    private List<Event> ApplyAndAddResultingEvents(List<Event> events)
    {
        void DidUseAction()
        {
            if (ActionsUsedCount >= Config.ActionsPerTurn)
            {
                switch (ActiveColor)
                {
                    case Color.White:
                        WhitePotionsCount--;
                        break;
                    case Color.Black:
                        BlackPotionsCount--;
                        break;
                }
            }
            else
            {
                ActionsUsedCount++;
            }
        }

        List<Event> extraEvents = new List<Event>();

        foreach (Event @event in events)
        {
            switch (@event.Type)
            {
                case Event.EventType.MonMove:
                    MonMoveEvent monMoveEvent = (MonMoveEvent)@event;
                    MonsMovesCount++;
                    Board.RemoveItem(monMoveEvent.From);
                    Board.Put(Item.MonItem(monMoveEvent.Item.Mon), monMoveEvent.To);
                    break;

                case Event.EventType.ManaMove:
                    ManaMoveEvent manaMoveEvent = (ManaMoveEvent)@event;
                    ManaMovesCount++;
                    Board.RemoveItem(manaMoveEvent.From);
                    Board.Put(Item.ManaItem(manaMoveEvent.Mana), manaMoveEvent.To);
                    break;

                case Event.EventType.ManaScored:
                    ManaScoredEvent manaScoredEvent = (ManaScoredEvent)@event;
                    if (ActiveColor == Color.Black)
                    {
                        BlackScore += manaScoredEvent.Mana.Score(Color.Black);
                    }
                    else if (ActiveColor == Color.White)
                    {
                        WhiteScore += manaScoredEvent.Mana.Score(Color.White);
                    }

                    if (Board.GetItem(manaScoredEvent.At).MonProperty is Mon mon)
                    {
                        Board.Put(Item.MonItem(mon), manaScoredEvent.At);
                    }
                    else
                    {
                        Board.RemoveItem(manaScoredEvent.At);
                    }
                    break;

                case Event.EventType.MysticAction:
                    MysticActionEvent mysticActionEvent = (MysticActionEvent)@event;
                    DidUseAction();
                    Board.RemoveItem(mysticActionEvent.To);
                    break;

                case Event.EventType.DemonAction:
                    DemonActionEvent demonActionEvent = (DemonActionEvent)@event;
                    DidUseAction();
                    Board.RemoveItem(demonActionEvent.From);
                    Board.Put(Item.MonItem(demonActionEvent.Demon), demonActionEvent.To);
                    break;

                case Event.EventType.DemonAdditionalStep:
                    DemonAdditionalStepEvent demonAdditionalStepEvent = (DemonAdditionalStepEvent)@event;
                    Board.Put(Item.MonItem(demonAdditionalStepEvent.Demon), demonAdditionalStepEvent.To);
                    break;

                case Event.EventType.SpiritTargetMove:
                    SpiritTargetMoveEvent spiritTargetMoveEvent = (SpiritTargetMoveEvent)@event;
                    DidUseAction();
                    Board.RemoveItem(spiritTargetMoveEvent.From);
                    Board.Put(spiritTargetMoveEvent.Item, spiritTargetMoveEvent.To);
                    break;

                case Event.EventType.PickupBomb:
                    PickupBombEvent pickupBombEvent = (PickupBombEvent)@event;
                    Board.Put(Item.MonWithConsumableItem(pickupBombEvent.By, Consumable.Bomb), pickupBombEvent.At);
                    break;

                case Event.EventType.PickupPotion:
                    PickupPotionEvent pickupPotionEvent = (PickupPotionEvent)@event;
                    if (pickupPotionEvent.By.MonProperty?.color == Color.Black)
                    {
                        BlackPotionsCount++;
                    }
                    else if (pickupPotionEvent.By.MonProperty?.color == Color.White)
                    {
                        WhitePotionsCount++;
                    }
                    Board.Put(pickupPotionEvent.By, pickupPotionEvent.At);
                    break;

                case Event.EventType.PickupMana:
                    PickupManaEvent pickupManaEvent = (PickupManaEvent)@event;
                    Board.Put(Item.MonWithManaItem(pickupManaEvent.By, pickupManaEvent.Mana), pickupManaEvent.At);
                    break;

                case Event.EventType.MonFainted:
                    MonFaintedEvent monFaintedEvent = (MonFaintedEvent)@event;
                    monFaintedEvent.Mon.Faint();
                    Board.Put(Item.MonItem(monFaintedEvent.Mon), monFaintedEvent.To);
                    break;

                case Event.EventType.ManaDropped:
                    ManaDroppedEvent manaDroppedEvent = (ManaDroppedEvent)@event;
                    Board.Put(Item.ManaItem(manaDroppedEvent.Mana), manaDroppedEvent.At);
                    break;

                case Event.EventType.SupermanaBackToBase:
                    SupermanaBackToBaseEvent supermanaBackToBaseEvent = (SupermanaBackToBaseEvent)@event;
                    Board.Put(Item.ManaItem(Mana.Supermana), supermanaBackToBaseEvent.To);
                    break;

                case Event.EventType.BombAttack:
                    BombAttackEvent bombAttackEvent = (BombAttackEvent)@event;
                    Board.RemoveItem(bombAttackEvent.To);
                    Board.Put(Item.MonItem(bombAttackEvent.By), bombAttackEvent.From);
                    break;

                case Event.EventType.BombExplosion:
                    BombExplosionEvent bombExplosionEvent = (BombExplosionEvent)@event;
                    Board.RemoveItem(bombExplosionEvent.At);
                    break;

                case Event.EventType.MonAwake:
                case Event.EventType.GameOver:
                case Event.EventType.NextTurn:
                    break;
            }
        }

        if (WinnerColor(WhiteScore, BlackScore) != null)
        {
            extraEvents.Add(new GameOverEvent(WinnerColor(WhiteScore, BlackScore)!.Value));
        }
        else if ((IsFirstTurn(TurnNumber) && !PlayerCanMoveMon(MonsMovesCount)) ||
                 (!IsFirstTurn(TurnNumber) && !PlayerCanMoveMana(TurnNumber, ManaMovesCount)) ||
                 (!IsFirstTurn(TurnNumber) && !PlayerCanMoveMon(MonsMovesCount) && Board.FindMana(ActiveColor) == null))
        {
            ActiveColor = (ActiveColor == Color.White) ? Color.Black : Color.White;
            TurnNumber++;
            extraEvents.Add(new NextTurnEvent(ActiveColor));
            ActionsUsedCount = 0;
            ManaMovesCount = 0;
            MonsMovesCount = 0;

            foreach (Location monLocation in Board.FaintedMonsLocations(ActiveColor))
            {
                if (Board.GetItem(monLocation).MonProperty is Mon mon)
                {
                    mon.DecreaseCooldown();
                    Board.Put(Item.MonItem(mon), monLocation);
                    if (!mon.isFainted)
                    {
                        extraEvents.Add(new MonAwakeEvent(mon, monLocation));
                    }
                }
            }
        }

        return events.Concat(extraEvents).ToList();
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
