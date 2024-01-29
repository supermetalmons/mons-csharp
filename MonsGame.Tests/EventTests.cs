// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class GameOverEventTests
{
    [Theory]
    [InlineData(Color.White)]
    [InlineData(Color.Black)]
    public void Constructor_CorrectlyInitializesWinner(Color expectedWinner)
    {
        var gameOverEvent = new GameOverEvent(expectedWinner);

        Assert.Equal(expectedWinner, gameOverEvent.Winner);
    }
}
