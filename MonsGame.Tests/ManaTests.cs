// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class ManaTests
{
    [Theory]
    [InlineData(Color.White)]
    [InlineData(Color.Black)]
    public void Regular_CreatesRegularManaWithColor(Color color)
    {
        var mana = Mana.Regular(color);

        Assert.Equal(ManaType.Regular, mana.Type);
        Assert.Equal(color, mana.Color);
    }

    [Fact]
    public void Supermana_CreatesSupermanaWithDefaultColor()
    {
        var mana = Mana.Supermana;

        Assert.Equal(ManaType.Supermana, mana.Type);
        Assert.Equal(default(Color), mana.Color);
    }

    [Theory]
    [InlineData(Color.White, Color.White, 1)]
    [InlineData(Color.White, Color.Black, 2)]
    public void Score_ReturnsCorrectScoreForRegularMana(Color manaColor, Color playerColor, int expectedScore)
    {
        var mana = Mana.Regular(manaColor);
        var score = mana.Score(playerColor);

        Assert.Equal(expectedScore, score);
    }

    [Theory]
    [InlineData(Color.White)]
    [InlineData(Color.Black)]
    public void Score_ReturnsTwoForSupermana(Color playerColor)
    {
        var mana = Mana.Supermana;
        var score = mana.Score(playerColor);

        Assert.Equal(2, score);
    }

    [Fact]
    public void Equals_ReturnsTrueForEqualMana()
    {
        var mana1 = Mana.Regular(Color.White);
        var mana2 = Mana.Regular(Color.White);

        Assert.True(mana1.Equals(mana2));
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentMana()
    {
        var mana1 = Mana.Regular(Color.White);
        var mana2 = Mana.Regular(Color.Black);

        Assert.False(mana1.Equals(mana2));
    }

    [Fact]
    public void EqualityOperators_WorkCorrectly()
    {
        var mana1 = Mana.Regular(Color.White);
        var mana2 = Mana.Regular(Color.White);
        var mana3 = Mana.Regular(Color.Black);

        Assert.True(mana1 == mana2);
        Assert.False(mana1 == mana3);
        Assert.False(mana1 != mana2);
        Assert.True(mana1 != mana3);
    }

    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualObjects()
    {
        var mana1 = Mana.Regular(Color.White);
        var mana2 = Mana.Regular(Color.White);

        Assert.Equal(mana1.GetHashCode(), mana2.GetHashCode());
    }
}
