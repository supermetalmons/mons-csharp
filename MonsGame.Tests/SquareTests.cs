// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class SquareTests
{
    [Fact]
    public void Regular_CreatesRegularSquare()
    {
        var square = Square.Regular;

        Assert.Equal(SquareType.Regular, square.Type);
        Assert.Equal(default(Color), square.Color);
        Assert.Equal(default(Mon.Kind), square.Kind);
    }

    [Fact]
    public void ConsumableBase_CreatesConsumableBaseSquare()
    {
        var square = Square.ConsumableBase;

        Assert.Equal(SquareType.ConsumableBase, square.Type);
        Assert.Equal(default(Color), square.Color);
        Assert.Equal(default(Mon.Kind), square.Kind);
    }

    [Fact]
    public void SupermanaBase_CreatesSupermanaBaseSquare()
    {
        var square = Square.SupermanaBase;

        Assert.Equal(SquareType.SupermanaBase, square.Type);
        Assert.Equal(default(Color), square.Color);
        Assert.Equal(default(Mon.Kind), square.Kind);
    }

    [Theory]
    [InlineData(Color.White)]
    [InlineData(Color.Black)]
    public void ManaBase_CreatesManaBaseSquare(Color color)
    {
        var square = Square.ManaBase(color);

        Assert.Equal(SquareType.ManaBase, square.Type);
        Assert.Equal(color, square.Color);
        Assert.Equal(default(Mon.Kind), square.Kind);
    }

    [Theory]
    [InlineData(Color.White)]
    [InlineData(Color.Black)]
    public void ManaPool_CreatesManaPoolSquare(Color color)
    {
        var square = Square.ManaPool(color);

        Assert.Equal(SquareType.ManaPool, square.Type);
        Assert.Equal(color, square.Color);
        Assert.Equal(default(Mon.Kind), square.Kind);
    }

    [Theory]
    [InlineData(Mon.Kind.Demon, Color.White)]
    [InlineData(Mon.Kind.Angel, Color.Black)]
    public void MonBase_CreatesMonBaseSquare(Mon.Kind kind, Color color)
    {
        var square = Square.MonBase(kind, color);

        Assert.Equal(SquareType.MonBase, square.Type);
        Assert.Equal(color, square.Color);
        Assert.Equal(kind, square.Kind);
    }

    [Fact]
    public void Equals_ReturnsTrueForEqualSquares()
    {
        var square1 = Square.MonBase(Mon.Kind.Demon, Color.White);
        var square2 = Square.MonBase(Mon.Kind.Demon, Color.White);

        Assert.True(square1.Equals(square2));
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentSquares()
    {
        var square1 = Square.MonBase(Mon.Kind.Demon, Color.White);
        var square2 = Square.ManaBase(Color.Black);

        Assert.False(square1.Equals(square2));
    }

    [Fact]
    public void EqualityOperators_WorkCorrectly()
    {
        var square1 = Square.ManaPool(Color.White);
        var square2 = Square.ManaPool(Color.White);
        var square3 = Square.ConsumableBase;

        Assert.True(square1 == square2);
        Assert.False(square1 == square3);
        Assert.False(square1 != square2);
        Assert.True(square1 != square3);
    }

    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualObjects()
    {
        var square1 = Square.ManaBase(Color.Black);
        var square2 = Square.ManaBase(Color.Black);

        Assert.Equal(square1.GetHashCode(), square2.GetHashCode());
    }
}
