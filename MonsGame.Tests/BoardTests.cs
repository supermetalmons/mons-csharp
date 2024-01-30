// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class BoardTests
{
    [Fact]
    public void Constructor_WithNullItems_SetsInitialItems()
    {
        var board = new Board(null);
        Assert.Equal(Config.InitialItems, board.Items);
    }

    [Fact]
    public void SquareAt_ReturnsCorrectSquare()
    {
        var board = new Board();
        var location = new Location(4, 4);
        var expectedSquare = Square.Regular;
        Assert.Equal(expectedSquare, board.SquareAt(location));
    }

    [Fact]
    public void AllMonsBases_ReturnsAllMonBaseLocations()
    {
        var board = new Board();
        var monBaseLocations = board.AllMonsBases;
        Assert.Contains(new Location(0, 5), monBaseLocations);
    }

    [Fact]
    public void FindMana_WithInitialBoard_ReturnsLocationForWhite()
    {
        var board = new Board();
        var location = board.FindMana(Color.White);
        Assert.NotNull(location);
    }

    [Fact]
    public void FindMana_WithInitialBoard_ReturnsLocationForBlack()
    {
        var board = new Board();
        var location = board.FindMana(Color.Black);
        Assert.NotNull(location);
    }

    [Fact]
    public void FindAwakeAngel_WithInitialBoard_ReturnsLocationForWhite()
    {
        var board = new Board();
        var location = board.FindAwakeAngel(Color.White);
        Assert.NotNull(location);
    }

    [Fact]
    public void FindAwakeAngel_WithInitialBoard_ReturnsLocationForBlack()
    {
        var board = new Board();
        var location = board.FindAwakeAngel(Color.Black);
        Assert.NotNull(location);
    }

}