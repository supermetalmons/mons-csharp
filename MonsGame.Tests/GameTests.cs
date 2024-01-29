// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class GameTests
{
    [Fact]
    public void DefaultConstructor_InitializesPropertiesToDefaultValues()
    {
        var game = new Game();

        Assert.NotNull(game.Board);
        Assert.Equal(0, game.WhiteScore);
        Assert.Equal(0, game.BlackScore);
        Assert.Equal(Color.White, game.ActiveColor);
        Assert.Equal(0, game.ActionsUsedCount);
        Assert.Equal(0, game.ManaMovesCount);
        Assert.Equal(0, game.MonsMovesCount);
        Assert.Equal(0, game.WhitePotionsCount);
        Assert.Equal(0, game.BlackPotionsCount);
        Assert.Equal(1, game.TurnNumber);
    }

    [Fact]
    public void ParameterizedConstructor_InitializesPropertiesCorrectly()
    {
        var board = new Board(); // Assuming a default constructor for Board
        var game = new Game(board, 3, 5, Color.Black, 2, 1, 4, 1, 2, 6);

        Assert.Equal(board, game.Board);
        Assert.Equal(3, game.WhiteScore);
        Assert.Equal(5, game.BlackScore);
        Assert.Equal(Color.Black, game.ActiveColor);
        Assert.Equal(2, game.ActionsUsedCount);
        Assert.Equal(1, game.ManaMovesCount);
        Assert.Equal(4, game.MonsMovesCount);
        Assert.Equal(1, game.WhitePotionsCount);
        Assert.Equal(2, game.BlackPotionsCount);
        Assert.Equal(6, game.TurnNumber);
    }
}
