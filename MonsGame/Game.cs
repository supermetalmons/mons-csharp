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

    private Output SuggestedInputToStartWith()
    {
        Func<Location, Location?> locationsFilter = location =>
        {
            var inputs = new List<Input> { new Input.LocationInput(location) };
            var output = ProcessInput(inputs, doNotApplyEvents: true, oneOptionEnough: true);

            if (output is NextInputOptionsOutput optionsOutput && optionsOutput.NextInputs.Any())
            {
                return location;
            }
            else
            {
                return null;
            }
        };

        var suggestedLocations = Board.AllMonsLocations(ActiveColor).Select(locationsFilter).Where(l => l != null).Cast<Location>().ToList();
        if (((!PlayerCanMoveMon(MonsMovesCount) && !PlayerCanUseAction(TurnNumber, PlayerPotionsCount(ActiveColor, WhitePotionsCount, BlackPotionsCount), ActionsUsedCount)) || suggestedLocations.Count == 0) && PlayerCanMoveMana(TurnNumber, ManaMovesCount))
        {
            suggestedLocations.AddRange(Board.AllFreeRegularManaLocations(ActiveColor).Select(locationsFilter).Where(l => l != null).Cast<Location>());
        }

        if (suggestedLocations.Count == 0)
        {
            return new InvalidInputOutput();
        }
        else
        {
            return new LocationsToStartFromOutput(suggestedLocations);
        }
    }

    public Output ProcessInput(List<Input> inputs, bool doNotApplyEvents, bool oneOptionEnough)
    {
        if (WinnerColor(WhiteScore, BlackScore).HasValue) return new InvalidInputOutput();
        if (!inputs.Any()) return SuggestedInputToStartWith();
        if (!(inputs[0] is Input.LocationInput locationInput)) return new InvalidInputOutput();

        var startLocation = locationInput.Location;
        var startItem = Board.GetItem(startLocation);
        if (startItem == null) return new InvalidInputOutput();

        Input? specificSecondInput = inputs.Count > 1 ? inputs[1] : null;
        var secondInputOptions = SecondInputOptions(startLocation, startItem.Value, oneOptionEnough, specificSecondInput);

        if (specificSecondInput == null)
        {
            if (!secondInputOptions.Any())
            {
                return new InvalidInputOutput();
            }
            else
            {
                return new NextInputOptionsOutput(secondInputOptions);
            }
        }

        if (!(specificSecondInput is Input.LocationInput targetLocationInput)) return new InvalidInputOutput();
        var targetLocation = targetLocationInput.Location;
        var secondInputKind = secondInputOptions.Select(opt => (NextInput?)opt).FirstOrDefault(opt => opt.HasValue && opt.Value.Input.Equals(specificSecondInput))?.Kind;
        if (!secondInputKind.HasValue) return new InvalidInputOutput();

        Input? specificThirdInput = inputs.Count > 2 ? inputs[2] : null;
        var outputForSecondInput = ProcessSecondInput(secondInputKind.Value, startItem.Value, startLocation, targetLocation, specificThirdInput);
        if (outputForSecondInput == null) return new InvalidInputOutput();
        var thirdInputOptions = outputForSecondInput.Value.Item2;
        var events = outputForSecondInput.Value.Item1;

        if (specificThirdInput == null)
        {
            if (thirdInputOptions.Any())
            {
                return new NextInputOptionsOutput(thirdInputOptions);
            }
            else if (events.Any())
            {
                return new EventsOutput(doNotApplyEvents ? events : ApplyAndAddResultingEvents(events));
            }
            else
            {
                return new InvalidInputOutput();
            }
        }

        var thirdInput = thirdInputOptions.Select(opt => (NextInput?)opt).FirstOrDefault(opt => opt.HasValue && opt.Value.Input.Equals(specificThirdInput));
        if (thirdInput == null) return new InvalidInputOutput();

        Input? specificFourthInput = inputs.Count > 3 ? inputs[3] : null;
        var outputForThirdInput = ProcessThirdInput(thirdInput!.Value, startItem.Value, startLocation, targetLocation);
        if (outputForThirdInput == null) return new InvalidInputOutput();
        var fourthInputOptions = outputForThirdInput.Value.Item2;
        events.AddRange(outputForThirdInput.Value.Item1);

        if (specificFourthInput == null)
        {
            if (fourthInputOptions.Any())
            {
                return new NextInputOptionsOutput(fourthInputOptions);
            }
            else if (events.Any())
            {
                return new EventsOutput(doNotApplyEvents ? events : ApplyAndAddResultingEvents(events));
            }
            else
            {
                return new InvalidInputOutput();
            }
        }

        if (!(specificFourthInput is Input.ModifierInput modifierInput)) return new InvalidInputOutput();
        var fourthInput = fourthInputOptions.Select(opt => (NextInput?)opt).FirstOrDefault(opt => opt.HasValue && opt.Value.Input.Equals(specificFourthInput));
        if (fourthInput == null || !(thirdInput.Value.Input is Input.LocationInput locationInputThird) || !fourthInput.Value.ActorMonItem.HasValue) return new InvalidInputOutput();

        var destinationLocation = locationInputThird.Location;
        var actorMonItem = fourthInput.Value.ActorMonItem.Value;
        var actorMon = actorMonItem.MonProperty;
        if (actorMon == null) return new InvalidInputOutput();

        switch (modifierInput.Modifier)
        {
            case Modifier.SelectBomb:
                events.Add(new PickupBombEvent(actorMon.Value, destinationLocation));
                break;
            case Modifier.SelectPotion:
                events.Add(new PickupPotionEvent(actorMonItem, destinationLocation));
                break;
            case Modifier.Cancel:
                return new InvalidInputOutput();
        }

        return new EventsOutput(doNotApplyEvents ? events : ApplyAndAddResultingEvents(events));
    }


    private List<NextInput> SecondInputOptions(Location startLocation, Item startItem, bool onlyOne, Input? specificNext)
    {
        Location? specificLocation = specificNext is Input.LocationInput locationInput ? locationInput.Location : (Location?)null;

        var secondInputOptions = new List<NextInput>();
        switch (startItem.Type)
        {
            case ItemType.Mon:
                var mon = startItem.Mon;
                if (mon.color == ActiveColor && !mon.isFainted && PlayerCanMoveMon(MonsMovesCount))
                {
                    var possibleMoves = NextInputs(startLocation.NearbyLocations, NextInputKind.MonMove, onlyOne, specificLocation, location =>
                    {
                        var item = Board.GetItem(location);
                        var square = Board.SquareAt(location);

                        if (item.HasValue)
                        {
                            switch (item.Value.Type)
                            {
                                case ItemType.Mon:
                                case ItemType.MonWithMana:
                                case ItemType.MonWithConsumable:
                                    return false;
                                case ItemType.Mana:
                                    return mon.kind == Mon.Kind.Drainer;
                                case ItemType.Consumable:
                                    return true;
                            }
                        }

                        return square.Type switch
                        {
                            SquareType.Regular => true,
                            SquareType.ConsumableBase => true,
                            SquareType.ManaBase => true,
                            SquareType.ManaPool => true,
                            SquareType.SupermanaBase => item.HasValue && item.Value.ManaProperty.HasValue && item.Value.ManaProperty.Value.Type == ManaType.Supermana && mon.kind == Mon.Kind.Drainer,
                            SquareType.MonBase => mon.kind == square.Kind && mon.color == square.Color,
                            _ => false,
                        };
                    });

                    secondInputOptions.AddRange(possibleMoves);

                    if (onlyOne && secondInputOptions.Any()) return secondInputOptions;
                }

                if (mon.color == ActiveColor && !mon.isFainted && Board.SquareAt(startLocation).Type != SquareType.MonBase && PlayerCanUseAction(TurnNumber, PlayerPotionsCount(ActiveColor, WhitePotionsCount, BlackPotionsCount), ActionsUsedCount))
                {
                    switch (mon.kind)
                    {
                        case Mon.Kind.Mystic:
                            var mysticActions = NextInputs(startLocation.ReachableByMysticAction, NextInputKind.MysticAction, onlyOne, specificLocation, location =>
                            {
                                var item = Board.GetItem(location);
                                if (!item.HasValue || ProtectedByOpponentsAngel(Board, ActiveColor).Contains(location)) return false;

                                return item.Value.Type switch
                                {
                                    ItemType.Mon or ItemType.MonWithMana or ItemType.MonWithConsumable when item.Value.Mon.color == mon.color || item.Value.Mon.isFainted => false,
                                    ItemType.Mana or ItemType.Consumable => false,
                                    _ => true,
                                };
                            });
                            secondInputOptions.AddRange(mysticActions);
                            break;

                        case Mon.Kind.Demon:
                            var demonActions = NextInputs(startLocation.ReachableByDemonAction, NextInputKind.DemonAction, onlyOne, specificLocation, location =>
                            {
                                var item = Board.GetItem(location);
                                if (!item.HasValue || ProtectedByOpponentsAngel(Board, ActiveColor).Contains(location)) return false;

                                var locationBetween = startLocation.LocationBetween(location);
                                if (Board.GetItem(locationBetween).HasValue) return false;

                                return item.Value.Type switch
                                {
                                    ItemType.Mon => item.Value.MonProperty.HasValue && item.Value.MonProperty.Value.color != mon.color && !item.Value.MonProperty.Value.isFainted,
                                    ItemType.MonWithMana => item.Value.MonProperty.HasValue && item.Value.MonProperty.Value.color != mon.color && !item.Value.MonProperty.Value.isFainted,
                                    ItemType.MonWithConsumable => item.Value.MonProperty.HasValue && item.Value.MonProperty.Value.color != mon.color && !item.Value.MonProperty.Value.isFainted,
                                    _ => false,
                                };

                            });
                            secondInputOptions.AddRange(demonActions);
                            break;

                        case Mon.Kind.Spirit:
                            var spiritActions = NextInputs(startLocation.ReachableBySpiritAction, NextInputKind.SpiritTargetCapture, onlyOne, specificLocation, location =>
                            {
                                var item = Board.GetItem(location);
                                if (!item.HasValue) return false;

                                return item.Value.Type switch
                                {
                                    ItemType.Mon => !item.Value.Mon.isFainted,
                                    _ => true,
                                };
                            });
                            secondInputOptions.AddRange(spiritActions);
                            break;
                    }
                }
                break;

            case ItemType.Mana:
                if (startItem.ManaProperty.HasValue && startItem.ManaProperty.Value.Color == ActiveColor && PlayerCanMoveMana(TurnNumber, ManaMovesCount))
                {
                    var mana = startItem.ManaProperty.Value;
                    secondInputOptions.AddRange(NextInputs(startLocation.NearbyLocations, NextInputKind.ManaMove, onlyOne, specificLocation, location =>
                    {
                        var item = Board.GetItem(location);
                        var square = Board.SquareAt(location);

                        if (item.HasValue)
                        {
                            switch (item.Value.Type)
                            {
                                case ItemType.Mon:
                                    return item.Value.Mon.kind == Mon.Kind.Drainer;
                                case ItemType.MonWithConsumable:
                                case ItemType.Consumable:
                                case ItemType.MonWithMana:
                                case ItemType.Mana:
                                    return false;
                            }
                        }

                        return square.Type switch
                        {
                            SquareType.Regular => true,
                            SquareType.ConsumableBase => true,
                            SquareType.ManaBase => true,
                            SquareType.ManaPool => true,
                            SquareType.SupermanaBase => false,
                            SquareType.MonBase => false,
                            _ => false,
                        };
                    }));
                }
                break;

            case ItemType.MonWithMana:

                if (startItem.MonProperty.HasValue && startItem.MonProperty.Value.color == ActiveColor && PlayerCanMoveMon(MonsMovesCount))
                {
                    var monWithMana = startItem.MonProperty.Value;
                    secondInputOptions.AddRange(NextInputs(startLocation.NearbyLocations, NextInputKind.MonMove, onlyOne, specificLocation, location =>
                    {
                        var item = Board.GetItem(location);
                        var square = Board.SquareAt(location);

                        if (item.HasValue)
                        {
                            switch (item.Value.Type)
                            {
                                case ItemType.Mon:
                                case ItemType.MonWithMana:
                                case ItemType.MonWithConsumable:
                                    return false;
                                case ItemType.Consumable:
                                case ItemType.Mana:
                                    break;
                            }
                        }

                        return square.Type switch
                        {
                            SquareType.Regular => true,
                            SquareType.ConsumableBase => true,
                            SquareType.ManaBase => true,
                            SquareType.ManaPool => true,
                            SquareType.SupermanaBase => startItem.ManaProperty.HasValue && startItem.ManaProperty.Value.Type == ManaType.Supermana || (item.HasValue && item.Value.ManaProperty.HasValue && item.Value.ManaProperty.Value.Type == ManaType.Supermana),
                            SquareType.MonBase => false,
                            _ => false,
                        };
                    }));
                }
                break;

            case ItemType.MonWithConsumable:

                if (startItem.MonProperty.HasValue && startItem.MonProperty.Value.color == ActiveColor)
                {
                    var monWithConsumable = startItem.MonProperty.Value;
                    if (PlayerCanMoveMon(MonsMovesCount))
                    {
                        secondInputOptions.AddRange(NextInputs(startLocation.NearbyLocations, NextInputKind.MonMove, onlyOne, specificLocation, location =>
                        {
                            var item = Board.GetItem(location);
                            var square = Board.SquareAt(location);

                            if (item.HasValue)
                            {
                                switch (item.Value.Type)
                                {
                                    case ItemType.Mon:
                                    case ItemType.Mana:
                                    case ItemType.MonWithMana:
                                    case ItemType.MonWithConsumable:
                                        return false;
                                    case ItemType.Consumable:
                                        break;
                                }
                            }

                            return square.Type switch
                            {
                                SquareType.Regular => true,
                                SquareType.ConsumableBase => true,
                                SquareType.ManaBase => true,
                                SquareType.ManaPool => true,
                                SquareType.SupermanaBase => false,
                                SquareType.MonBase => false,
                                _ => false,
                            };
                        }));

                        if (onlyOne && secondInputOptions.Any()) return secondInputOptions;
                    }

                    if (startItem.ConsumableProperty.HasValue && startItem.ConsumableProperty.Value == Consumable.Bomb)
                    {
                        secondInputOptions.AddRange(NextInputs(startLocation.ReachableByBomb, NextInputKind.BombAttack, onlyOne, specificLocation, location =>
                        {
                            var item = Board.GetItem(location);
                            if (!item.HasValue) return false;

                            switch (item.Value.Type)
                            {
                                case ItemType.Mon:
                                case ItemType.MonWithMana:
                                case ItemType.MonWithConsumable:
                                    return item.Value.Mon.color != monWithConsumable.color && !item.Value.Mon.isFainted;
                                case ItemType.Consumable:
                                case ItemType.Mana:
                                    return false;
                                default:
                                    return true;
                            }
                        }));
                    }
                }
                break;

            case ItemType.Consumable:
                return new List<NextInput>();
        }

        return secondInputOptions;
    }

    private (List<Event>, List<NextInput>)? ProcessSecondInput(NextInputKind kind, Item startItem, Location startLocation, Location targetLocation, Input? specificNext)
    {
        Location? specificLocation = null;
        if (specificNext is Input.LocationInput locationInput)
        {
            specificLocation = locationInput.Location;
        }

        var thirdInputOptions = new List<NextInput>();
        var events = new List<Event>();
        var targetSquare = Board.SquareAt(targetLocation);
        var targetItem = Board.GetItem(targetLocation);

        switch (kind)
        {
            case NextInputKind.MonMove:
                if (!startItem.MonProperty.HasValue) return null;
                var startMon = startItem.MonProperty.Value;
                events.Add(new MonMoveEvent(startItem, startLocation, targetLocation));

                if (targetItem.HasValue)
                {
                    switch (targetItem.Value.Type)
                    {
                        case ItemType.Mon:
                        case ItemType.MonWithMana:
                        case ItemType.MonWithConsumable:
                            return null;

                        case ItemType.Mana:
                            if (startItem.ManaProperty.HasValue)
                            {
                                var startMana = startItem.ManaProperty.Value;
                                if (startMana.Type == ManaType.Supermana)
                                {
                                    events.Add(new SupermanaBackToBaseEvent(startLocation, Board.SupermanaBase));
                                }
                                else
                                {
                                    events.Add(new ManaDroppedEvent(startMana, startLocation));
                                }
                            }
                            events.Add(new PickupManaEvent(targetItem.Value.Mana, startMon, targetLocation));
                            break;

                        case ItemType.Consumable:
                            var consumable = targetItem.Value.ConsumableProperty;
                            if (consumable == Consumable.BombOrPotion)
                            {
                                if (startItem.ConsumableProperty.HasValue || startItem.ManaProperty.HasValue)
                                {
                                    events.Add(new PickupPotionEvent(startItem, targetLocation));
                                }
                                else
                                {
                                    thirdInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectBomb), NextInputKind.SelectConsumable, startItem));
                                    thirdInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectPotion), NextInputKind.SelectConsumable, startItem));
                                }
                            }
                            break;
                    }
                }

                if (targetSquare.Type == SquareType.ManaPool && startItem.ManaProperty.HasValue)
                {
                    events.Add(new ManaScoredEvent(startItem.ManaProperty.Value, targetLocation));
                }
                break;

            case NextInputKind.ManaMove:
                if (!startItem.ManaProperty.HasValue) return null;
                var mana = startItem.ManaProperty.Value;
                events.Add(new ManaMoveEvent(mana, startLocation, targetLocation));

                if (targetItem.HasValue)
                {
                    switch (targetItem.Value.Type)
                    {
                        case ItemType.Mon:
                            events.Add(new PickupManaEvent(mana, targetItem.Value.Mon, targetLocation));
                            break;
                        case ItemType.Mana:
                        case ItemType.Consumable:
                        case ItemType.MonWithMana:
                        case ItemType.MonWithConsumable:
                            return null;
                    }
                }

                switch (targetSquare.Type)
                {
                    case SquareType.ManaBase:
                    case SquareType.ConsumableBase:
                    case SquareType.Regular:
                        break;
                    case SquareType.ManaPool:
                        events.Add(new ManaScoredEvent(mana, targetLocation));
                        break;
                    case SquareType.MonBase:
                    case SquareType.SupermanaBase:
                        return null;
                }
                break;

            case NextInputKind.MysticAction:
                if (!startItem.MonProperty.HasValue) return null;
                events.Add(new MysticActionEvent(startItem.Mon, startLocation, targetLocation));

                if (targetItem.HasValue)
                {
                    switch (targetItem.Value.Type)
                    {
                        case ItemType.Mon:
                            var targetMon = targetItem.Value.Mon;
                            events.Add(new MonFaintedEvent(targetMon, targetLocation, Board.Base(targetMon)));
                            break;
                        case ItemType.MonWithMana:
                            var targetMonWithMana = targetItem.Value.Mon;
                            var manaWithMon = targetItem.Value.Mana;
                            events.Add(new MonFaintedEvent(targetMonWithMana, targetLocation, Board.Base(targetMonWithMana)));
                            if (manaWithMon.Type == ManaType.Regular)
                            {
                                events.Add(new ManaDroppedEvent(manaWithMon, targetLocation));
                            }
                            else if (manaWithMon.Type == ManaType.Supermana)
                            {
                                events.Add(new SupermanaBackToBaseEvent(targetLocation, Board.SupermanaBase));
                            }
                            break;
                        case ItemType.MonWithConsumable:
                            var targetMonWithConsumable = targetItem.Value.Mon;
                            events.Add(new MonFaintedEvent(targetMonWithConsumable, targetLocation, Board.Base(targetMonWithConsumable)));
                            if (targetItem.Value.ConsumableProperty == Consumable.Bomb)
                            {
                                events.Add(new BombExplosionEvent(targetLocation));
                            }
                            break;
                        case ItemType.Consumable:
                        case ItemType.Mana:
                            return null;
                    }
                }
                break;

            case NextInputKind.DemonAction:
                if (!startItem.MonProperty.HasValue) return null;
                var startDemonMon = startItem.MonProperty.Value;
                events.Add(new DemonActionEvent(startDemonMon, startLocation, targetLocation));
                bool requiresAdditionalStep = false;

                if (targetItem.HasValue)
                {
                    switch (targetItem.Value.Type)
                    {
                        case ItemType.Mana:
                        case ItemType.Consumable:
                            return null;

                        case ItemType.Mon:
                            var targetMon = targetItem.Value.Mon;
                            events.Add(new MonFaintedEvent(targetMon, targetLocation, Board.Base(targetMon)));
                            break;

                        case ItemType.MonWithMana:
                            var targetMonWithMana = targetItem.Value.Mon;
                            var manaLost = targetItem.Value.Mana;
                            events.Add(new MonFaintedEvent(targetMonWithMana, targetLocation, Board.Base(targetMonWithMana)));
                            if (manaLost.Type == ManaType.Regular)
                            {
                                requiresAdditionalStep = true;
                                events.Add(new ManaDroppedEvent(manaLost, targetLocation));
                            }
                            else if (manaLost.Type == ManaType.Supermana)
                            {
                                events.Add(new SupermanaBackToBaseEvent(targetLocation, Board.SupermanaBase));
                            }
                            break;

                        case ItemType.MonWithConsumable:
                            var targetMonWithConsumable = targetItem.Value.Mon;
                            var consumable = targetItem.Value.ConsumableProperty;
                            events.Add(new MonFaintedEvent(targetMonWithConsumable, targetLocation, Board.Base(targetMonWithConsumable)));
                            if (consumable == Consumable.Bomb)
                            {
                                events.Add(new BombExplosionEvent(targetLocation));
                                events.Add(new MonFaintedEvent(startDemonMon, targetLocation, Board.Base(startDemonMon)));
                            }
                            break;
                    }
                }

                switch (targetSquare.Type)
                {
                    case SquareType.Regular:
                    case SquareType.ConsumableBase:
                    case SquareType.ManaBase:
                    case SquareType.ManaPool:
                        break;

                    case SquareType.SupermanaBase:
                    case SquareType.MonBase:
                        requiresAdditionalStep = true;
                        break;
                }

                if (requiresAdditionalStep)
                {
                    var nearbyLocations = targetLocation.NearbyLocations;

                    foreach (var location in nearbyLocations)
                    {
                        var item = Board.GetItem(location);
                        var square = Board.SquareAt(location);
                        bool isEligibleLocation = false;

                        if (item.HasValue)
                        {
                            switch (item.Value.Type)
                            {
                                case ItemType.Mon:
                                case ItemType.Mana:
                                case ItemType.MonWithMana:
                                case ItemType.MonWithConsumable:
                                    continue;
                                case ItemType.Consumable:
                                    break;
                            }
                        }

                        switch (square.Type)
                        {
                            case SquareType.Regular:
                            case SquareType.ConsumableBase:
                            case SquareType.ManaBase:
                            case SquareType.ManaPool:
                                isEligibleLocation = true;
                                break;
                            case SquareType.MonBase:
                                isEligibleLocation = (startDemonMon.kind == square.Kind && startDemonMon.color == square.Color);
                                break;
                            case SquareType.SupermanaBase:
                                isEligibleLocation = false;
                                break;
                        }

                        if (isEligibleLocation)
                        {
                            var nextInput = new NextInput(new Input.LocationInput(location), NextInputKind.DemonAdditionalStep);
                            thirdInputOptions.Add(nextInput);
                        }
                    }
                }

                break;

            case NextInputKind.SpiritTargetCapture:
                if (!targetItem.HasValue) return null;
                foreach (var location in targetLocation.NearbyLocations)
                {
                    var destinationItem = Board.GetItem(location);
                    var destinationSquare = Board.SquareAt(location);
                    if (destinationItem.HasValue)
                    {
                        switch (destinationItem.Value.Type)
                        {
                            case ItemType.Mon:
                                var destinationMon = destinationItem.Value.Mon;
                                if (targetItem.Value.Type == ItemType.Mon || targetItem.Value.Type == ItemType.MonWithMana || targetItem.Value.Type == ItemType.MonWithConsumable)
                                {
                                    continue;
                                }
                                if (targetItem.Value.Type == ItemType.Mana && (destinationMon.kind != Mon.Kind.Drainer || destinationMon.isFainted))
                                {
                                    continue;
                                }
                                if (targetItem.Value.Type == ItemType.Consumable && targetItem.Value.Consumable != Consumable.BombOrPotion)
                                {
                                    continue;
                                }
                                break;

                            case ItemType.Mana:
                                if (targetItem.Value.Type == ItemType.Mon && targetItem.Value.Mon.kind != Mon.Kind.Drainer || targetItem.Value.Mon.isFainted)
                                {
                                    continue;
                                }
                                if (targetItem.Value.Type == ItemType.Consumable || targetItem.Value.Type == ItemType.MonWithConsumable || targetItem.Value.Type == ItemType.MonWithMana || targetItem.Value.Type == ItemType.Mana)
                                {
                                    continue;
                                }
                                break;

                            case ItemType.MonWithMana:
                            case ItemType.MonWithConsumable:
                                if (targetItem.Value.Type == ItemType.Consumable && targetItem.Value.Consumable != Consumable.BombOrPotion)
                                {
                                    continue;
                                }
                                if (targetItem.Value.Type == ItemType.Mon || targetItem.Value.Type == ItemType.MonWithMana || targetItem.Value.Type == ItemType.MonWithConsumable || targetItem.Value.Type == ItemType.Mana)
                                {
                                    continue;
                                }
                                break;

                            case ItemType.Consumable:
                                var destinationConsumable = destinationItem.Value.ConsumableProperty;
                                if ((targetItem.Value.Type == ItemType.Mon || targetItem.Value.Type == ItemType.MonWithMana || targetItem.Value.Type == ItemType.MonWithConsumable) && destinationConsumable != Consumable.BombOrPotion)
                                {
                                    continue;
                                }
                                if (targetItem.Value.Type == ItemType.Mana || targetItem.Value.Type == ItemType.Consumable)
                                {
                                    continue;
                                }
                                break;
                        }
                    }

                    bool isValidLocation = destinationSquare.Type switch
                    {
                        SquareType.Regular => true,
                        SquareType.ConsumableBase => true,
                        SquareType.ManaBase => true,
                        SquareType.ManaPool => true,
                        SquareType.SupermanaBase => (targetItem.Value.Type == ItemType.Mana && targetItem.Value.Mana.Type == ManaType.Supermana) || (targetItem.Value.Type == ItemType.Mon && targetItem.Value.Mon.kind == Mon.Kind.Drainer && destinationItem.HasValue && destinationItem.Value.ManaProperty.HasValue && destinationItem.Value.ManaProperty.Value.Type == ManaType.Supermana),
                        SquareType.MonBase => targetItem.Value.Type == ItemType.Mon && targetItem.Value.Mon.kind == destinationSquare.Kind && targetItem.Value.Mon.color == destinationSquare.Color && !targetItem.Value.ManaProperty.HasValue && !targetItem.Value.ConsumableProperty.HasValue,
                        _ => false,
                    };


                    if (isValidLocation)
                    {
                        var nextInput = new NextInput(new Input.LocationInput(location), NextInputKind.SpiritTargetMove);
                        thirdInputOptions.Add(nextInput);
                    }
                }
                break;

            case NextInputKind.BombAttack:
                if (!startItem.MonProperty.HasValue) return null;
                var startBombMon = startItem.MonProperty.Value;
                events.Add(new BombAttackEvent(startBombMon, startLocation, targetLocation));

                if (targetItem.HasValue)
                {
                    switch (targetItem.Value.Type)
                    {
                        case ItemType.Mon:
                            var mon = targetItem.Value.Mon;
                            events.Add(new MonFaintedEvent(mon, targetLocation, Board.Base(mon)));
                            break;
                        case ItemType.MonWithMana:
                            var monWithMana = targetItem.Value.Mon;
                            var manaBombLost = targetItem.Value.Mana;
                            events.Add(new MonFaintedEvent(monWithMana, targetLocation, Board.Base(monWithMana)));
                            if (manaBombLost.Type == ManaType.Regular)
                            {
                                events.Add(new ManaDroppedEvent(manaBombLost, targetLocation));
                            }
                            else if (manaBombLost.Type == ManaType.Supermana)
                            {
                                events.Add(new SupermanaBackToBaseEvent(targetLocation, Board.SupermanaBase));
                            }
                            break;
                        case ItemType.MonWithConsumable:
                            var monWithConsumable = targetItem.Value.Mon;
                            var consumable = targetItem.Value.ConsumableProperty;
                            events.Add(new MonFaintedEvent(monWithConsumable, targetLocation, Board.Base(monWithConsumable)));
                            if (consumable == Consumable.Bomb)
                            {
                                events.Add(new BombExplosionEvent(targetLocation));
                            }
                            break;
                        case ItemType.Mana:
                        case ItemType.Consumable:
                            break;
                    }
                }

                break;

            case NextInputKind.SpiritTargetMove:
            case NextInputKind.DemonAdditionalStep:
            case NextInputKind.SelectConsumable:
                return null;

        }

        return (events, thirdInputOptions);
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

                events.Add(new SpiritTargetMoveEvent(targetItem.Value, targetLocation, destinationLocation));

                if (destinationItem.HasValue)
                {
                    if (targetItem.Value.Type == ItemType.Mon)
                    {
                        var travellingMon = targetItem.Value.Mon;
                        switch (destinationItem.Value.Type)
                        {
                            case ItemType.Mon:
                            case ItemType.MonWithMana:
                            case ItemType.MonWithConsumable:
                                return null;
                            case ItemType.Mana:
                                var destinationMana = destinationItem.Value.Mana;
                                events.Add(new PickupManaEvent(destinationMana, travellingMon, destinationLocation));
                                break;
                            case ItemType.Consumable:
                                var destinationConsumable = destinationItem.Value.Consumable;
                                if (destinationConsumable == Consumable.BombOrPotion)
                                {
                                    forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectBomb), NextInputKind.SelectConsumable, targetItem));
                                    forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectPotion), NextInputKind.SelectConsumable, targetItem));
                                }
                                else
                                {
                                    return null;
                                }
                                break;
                        }
                    }
                    else if (targetItem.Value.Type == ItemType.Mana)
                    {
                        var travellingMana = targetItem.Value.Mana;
                        if (destinationItem.Value.Type == ItemType.Mon)
                        {
                            var destinationMon = destinationItem.Value.Mon;
                            events.Add(new PickupManaEvent(travellingMana, destinationMon, destinationLocation));
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (targetItem.Value.Type == ItemType.MonWithMana)
                    {
                        switch (destinationItem.Value.Type)
                        {
                            case ItemType.Mon:
                            case ItemType.Mana:
                            case ItemType.MonWithMana:
                            case ItemType.MonWithConsumable:
                                return null;
                            case ItemType.Consumable:
                                if (destinationItem.Value.Consumable == Consumable.BombOrPotion)
                                {
                                    events.Add(new PickupPotionEvent(targetItem.Value, destinationLocation));
                                }
                                else
                                {
                                    return null;
                                }
                                break;
                        }
                    }
                    else if (targetItem.Value.Type == ItemType.MonWithConsumable)
                    {
                        switch (destinationItem.Value.Type)
                        {
                            case ItemType.Mon:
                            case ItemType.Mana:
                            case ItemType.MonWithMana:
                            case ItemType.MonWithConsumable:
                                return null;
                            case ItemType.Consumable:
                                if (destinationItem.Value.Consumable == Consumable.BombOrPotion)
                                {
                                    events.Add(new PickupPotionEvent(targetItem.Value, destinationLocation));
                                }
                                else
                                {
                                    return null;
                                }
                                break;
                        }
                    }
                    else if (targetItem.Value.Type == ItemType.Consumable)
                    {
                        var travellingConsumable = targetItem.Value.Consumable;
                        switch (destinationItem.Value.Type)
                        {
                            case ItemType.Mana:
                            case ItemType.Consumable:
                                return null;
                            case ItemType.Mon:
                                forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectBomb), NextInputKind.SelectConsumable, destinationItem));
                                forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectPotion), NextInputKind.SelectConsumable, destinationItem));
                                break;
                            case ItemType.MonWithMana:
                            case ItemType.MonWithConsumable:
                                if (travellingConsumable == Consumable.BombOrPotion)
                                {
                                    events.Add(new PickupPotionEvent(destinationItem.Value, destinationLocation));
                                }
                                else
                                {
                                    return null;
                                }
                                break;
                        }
                    }

                }

                if (destinationSquare.Type == SquareType.ManaPool && targetItem.Value.ManaProperty.HasValue)
                {
                    events.Add(new ManaScoredEvent(targetItem.Value.ManaProperty.Value, destinationLocation));
                }
                break;

            case NextInputKind.DemonAdditionalStep:
                if (!(thirdInput.Input is Input.LocationInput demonLocationInput) || !startItem.MonProperty.HasValue) return null;
                var destinationLocationDemonStep = demonLocationInput.Location;
                events.Add(new DemonAdditionalStepEvent(startItem.MonProperty.Value, targetLocation, destinationLocationDemonStep));

                var itemAtDestinationDemonStep = Board.GetItem(destinationLocationDemonStep);
                if (itemAtDestinationDemonStep.HasValue && itemAtDestinationDemonStep.Value.Type == ItemType.Consumable && itemAtDestinationDemonStep.Value.ConsumableProperty == Consumable.BombOrPotion)
                {
                    forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectBomb), NextInputKind.SelectConsumable, startItem));
                    forthInputOptions.Add(new NextInput(new Input.ModifierInput(Modifier.SelectPotion), NextInputKind.SelectConsumable, startItem));
                }
                break;

            case NextInputKind.SelectConsumable:
                if (!(thirdInput.Input is Input.ModifierInput modifierInput) || !startItem.MonProperty.HasValue) return null;
                switch (modifierInput.Modifier)
                {
                    case Modifier.SelectBomb:
                        events.Add(new PickupBombEvent(startItem.MonProperty.Value, targetLocation));
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
                    Board.Put(monMoveEvent.Item, monMoveEvent.To);
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

                    if (Board.GetItem(manaScoredEvent.At)?.MonProperty is Mon mon)
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
                    var faintedMon = monFaintedEvent.Mon;
                    faintedMon.Faint();
                    Board.Put(Item.MonItem(faintedMon), monFaintedEvent.To);
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
                if (Board.GetItem(monLocation)?.MonProperty is Mon mon)
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
