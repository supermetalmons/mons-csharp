// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class MonTests
{
    [Theory]
    [InlineData(MonKind.Demon, Color.White)]
    [InlineData(MonKind.Drainer, Color.Black)]
    public void Constructor_SetsPropertiesCorrectly(MonKind kind, Color color)
    {
        var mon = new Mon(kind, color, 1);

        Assert.Equal(kind, mon.Kind);
        Assert.Equal(color, mon.Color);
        Assert.Equal(1, mon.Cooldown);
    }

    [Fact]
    public void Faint_SetsCooldownToTwo()
    {
        var mon = new Mon(MonKind.Drainer, Color.Black);
        mon.Faint();

        Assert.Equal(2, mon.Cooldown);
    }

    [Fact]
    public void DecreaseCooldown_DecreasesCooldown()
    {
        var mon = new Mon(MonKind.Angel, Color.White, 2);
        mon.DecreaseCooldown();

        Assert.Equal(1, mon.Cooldown);
    }

    [Fact]
    public void DecreaseCooldown_DoesNotDecreaseBelowZero()
    {
        var mon = new Mon(MonKind.Spirit, Color.Black);
        mon.DecreaseCooldown();

        Assert.Equal(0, mon.Cooldown);
    }

    [Fact]
    public void IsFainted_ReturnsTrueWhenCooldownGreaterThanZero()
    {
        var mon = new Mon(MonKind.Mystic, Color.White, 1);

        Assert.True(mon.IsFainted);
    }

    [Fact]
    public void IsFainted_ReturnsFalseWhenCooldownIsZero()
    {
        var mon = new Mon(MonKind.Mystic, Color.Black);

        Assert.False(mon.IsFainted);
    }

    [Theory]
    [InlineData(MonKind.Demon, Color.White)]
    [InlineData(MonKind.Angel, Color.Black)]
    public void Equals_ReturnsTrueForEqualMons(MonKind kind, Color color)
    {
        var mon1 = new Mon(kind, color);
        var mon2 = new Mon(kind, color);

        Assert.True(mon1.Equals(mon2));
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentMons()
    {
        var mon1 = new Mon(MonKind.Demon, Color.White);
        var mon2 = new Mon(MonKind.Angel, Color.Black);

        Assert.False(mon1.Equals(mon2));
    }

    [Fact]
    public void EqualityOperators_WorkCorrectly()
    {
        var mon1 = new Mon(MonKind.Spirit, Color.White);
        var mon2 = new Mon(MonKind.Spirit, Color.White);
        var mon3 = new Mon(MonKind.Mystic, Color.Black);

        Assert.True(mon1 == mon2);
        Assert.False(mon1 == mon3);
        Assert.False(mon1 != mon2);
        Assert.True(mon1 != mon3);
    }

    [Theory]
    [InlineData(MonKind.Demon, Color.White)]
    [InlineData(MonKind.Angel, Color.Black)]
    public void GetHashCode_ReturnsSameValueForEqualObjects(MonKind kind, Color color)
    {
        var mon1 = new Mon(kind, color);
        var mon2 = new Mon(kind, color);

        Assert.Equal(mon1.GetHashCode(), mon2.GetHashCode());
    }

[Fact]
    public void Deserialize_Mon_FromJson()
    {
        string json = "{\"color\":\"white\",\"cooldown\":0,\"kind\":\"mystic\"}";
        var mon = JsonSerializer.Deserialize<Mon>(json, JsonOptions.DefaultSerializerOptions);

        Assert.Equal(MonKind.Mystic, mon.Kind);
        Assert.Equal(Color.White, mon.Color);
        Assert.Equal(0, mon.Cooldown);
        Assert.False(mon.IsFainted);
    }

    [Fact]
    public void Serialize_Mon_ToJson()
    {
        var mon = new Mon(MonKind.Mystic, Color.White, 2);
        string json = JsonSerializer.Serialize(mon, JsonOptions.DefaultSerializerOptions);
        Assert.Equal("{\"color\":\"white\",\"cooldown\":2,\"kind\":\"mystic\"}", json);
    }
        
        
        

}
