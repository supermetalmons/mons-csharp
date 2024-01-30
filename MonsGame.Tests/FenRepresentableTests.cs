// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class FenRepresentableTests
{
    [Fact]
    public void GameFromFen_ShouldCreateEquivalentGame()
    {
        var initialGame = new Game();
        string fen = initialGame.Fen;
        var newGame = Game.FromFen(fen);
        Assert.Equal(initialGame.WhiteScore, newGame.WhiteScore);
        Assert.Equal(initialGame.BlackScore, newGame.BlackScore);
        Assert.Equal(initialGame.ActiveColor, newGame.ActiveColor);
        Assert.Equal(initialGame.ActionsUsedCount, newGame.ActionsUsedCount);
        Assert.Equal(initialGame.ManaMovesCount, newGame.ManaMovesCount);
        Assert.Equal(initialGame.MonsMovesCount, newGame.MonsMovesCount);
        Assert.Equal(initialGame.WhitePotionsCount, newGame.WhitePotionsCount);
        Assert.Equal(initialGame.BlackPotionsCount, newGame.BlackPotionsCount);
        Assert.Equal(initialGame.TurnNumber, newGame.TurnNumber);
    }
}
