// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class MonTests
{
    [Theory]
    [InlineData(Mon.Kind.Demon, Color.White)]
    [InlineData(Mon.Kind.Drainer, Color.Black)]
    public void Constructor_SetsPropertiesCorrectly(Mon.Kind kind, Color color)
    {
        var mon = new Mon(kind, color, 1);

        Assert.Equal(kind, mon.kind);
        Assert.Equal(color, mon.color);
        Assert.Equal(1, mon.cooldown);
    }

    [Fact]
    public void Faint_SetsCooldownToTwo()
    {
        var mon = new Mon(Mon.Kind.Drainer, Color.Black);
        mon.Faint();

        Assert.Equal(2, mon.cooldown);
    }

    [Fact]
    public void DecreaseCooldown_DecreasesCooldown()
    {
        var mon = new Mon(Mon.Kind.Angel, Color.White, 2);
        mon.DecreaseCooldown();

        Assert.Equal(1, mon.cooldown);
    }

    [Fact]
    public void DecreaseCooldown_DoesNotDecreaseBelowZero()
    {
        var mon = new Mon(Mon.Kind.Spirit, Color.Black);
        mon.DecreaseCooldown();

        Assert.Equal(0, mon.cooldown);
    }

    [Fact]
    public void IsFainted_ReturnsTrueWhenCooldownGreaterThanZero()
    {
        var mon = new Mon(Mon.Kind.Mystic, Color.White, 1);

        Assert.True(mon.isFainted);
    }

    [Fact]
    public void IsFainted_ReturnsFalseWhenCooldownIsZero()
    {
        var mon = new Mon(Mon.Kind.Mystic, Color.Black);

        Assert.False(mon.isFainted);
    }

    [Theory]
    [InlineData(Mon.Kind.Demon, Color.White)]
    [InlineData(Mon.Kind.Angel, Color.Black)]
    public void Equals_ReturnsTrueForEqualMons(Mon.Kind kind, Color color)
    {
        var mon1 = new Mon(kind, color);
        var mon2 = new Mon(kind, color);

        Assert.True(mon1.Equals(mon2));
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentMons()
    {
        var mon1 = new Mon(Mon.Kind.Demon, Color.White);
        var mon2 = new Mon(Mon.Kind.Angel, Color.Black);

        Assert.False(mon1.Equals(mon2));
    }

    [Fact]
    public void EqualityOperators_WorkCorrectly()
    {
        var mon1 = new Mon(Mon.Kind.Spirit, Color.White);
        var mon2 = new Mon(Mon.Kind.Spirit, Color.White);
        var mon3 = new Mon(Mon.Kind.Mystic, Color.Black);

        Assert.True(mon1 == mon2);
        Assert.False(mon1 == mon3);
        Assert.False(mon1 != mon2);
        Assert.True(mon1 != mon3);
    }

    [Theory]
    [InlineData(Mon.Kind.Demon, Color.White)]
    [InlineData(Mon.Kind.Angel, Color.Black)]
    public void GetHashCode_ReturnsSameValueForEqualObjects(Mon.Kind kind, Color color)
    {
        var mon1 = new Mon(kind, color);
        var mon2 = new Mon(kind, color);

        Assert.Equal(mon1.GetHashCode(), mon2.GetHashCode());
    }

[Fact]
    public void Deserialize_Mon_FromJson()
    {
        string json = "{\"color\":\"white\",\"cooldown\":0,\"kind\":\"mystic\"}";
        var mon = JsonSerializer.Deserialize<Mon>(json);

        Assert.Equal(Mon.Kind.Mystic, mon.kind);
        Assert.Equal(Color.White, mon.color);
        Assert.Equal(0, mon.cooldown);
        Assert.False(mon.isFainted);
    }

    [Fact]
    public void Serialize_Mon_ToJson()
    {
        var mon = new Mon(Mon.Kind.Mystic, Color.White, 2);
        string json = JsonSerializer.Serialize(mon);
        Assert.Equal("{\"color\":\"white\",\"cooldown\":2,\"kind\":\"mystic\"}", json);
    }
        
        
        

}
