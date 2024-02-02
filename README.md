`dotnet add package MonsGame`

```csharp
using MonsGame;

var game = new Game();
var suggestedInputsOutput = game.ProcessInput(new List<Input>());

var location1Input = new Input.LocationInput(new Location(0, 0));
var location2Input = new Input.LocationInput(new Location(1, 1));
var modifierInput = new Input.ModifierInput(Modifier.SelectPotion);
var inputs = new List<Input> { location1Input, location2Input, modifierInput };
var output = game.ProcessInput(inputs);

var location = new Location(4, 4);
var square = game.Board.SquareAt(location);
var item = game.Board.GetItem(location);

var activeColor = game.ActiveColor;
var remainingMonsMovesCount = game.AvailableMoveKinds[AvailableMoveKind.MonMove];
var winnerColor = game.WinnerColor();

var fen = game.Fen;
var anotherGame = Game.FromFen(fen);
var boardSize = Config.BoardSize;
```