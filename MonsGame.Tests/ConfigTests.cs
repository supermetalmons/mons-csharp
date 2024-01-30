// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;
public class ConfigTests
{
    [Fact]
    public void BoardSize_IsExpectedValue()
    {
        Assert.Equal(11, Config.BoardSize);
    }

    [Fact]
    public void TargetScore_IsExpectedValue()
    {
        Assert.Equal(5, Config.TargetScore);
    }

    [Fact]
    public void MonsBases_ContainsCorrectLocations()
    {
        Assert.Equal(10, Config.MonsBases.Count);
        Assert.Contains(new Location(0, 3), Config.MonsBases);
        Assert.Contains(new Location(10, 7), Config.MonsBases);
    }
}
