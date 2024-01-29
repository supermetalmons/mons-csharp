// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class LocationTests
{
    private const int BoardSize = 11;

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 5)]
    [InlineData(9, 9)]
    public void Constructor_SetsPropertiesCorrectly(int i, int j)
    {
        var location = new Location(i, j);

        Assert.Equal(i, location.I);
        Assert.Equal(j, location.J);
    }

    // Additional tests for NearbyLocations, ReachableByBomb, ReachableByMysticAction, etc.

    [Fact]
    public void LocationBetween_CalculatesCorrectMidpoint()
    {
        var location1 = new Location(0, 0);
        var location2 = new Location(4, 4);
        var midpoint = location1.LocationBetween(location2);

        Assert.Equal(new Location(2, 2), midpoint);
    }

    [Theory]
    [InlineData(0, 0, 4, 4, 4)]
    [InlineData(2, 3, 5, 7, 4)]
    public void DistanceTo_CalculatesCorrectDistance(int i1, int j1, int i2, int j2, int expectedDistance)
    {
        var location1 = new Location(i1, j1);
        var location2 = new Location(i2, j2);
        var distance = location1.DistanceTo(location2);

        Assert.Equal(expectedDistance, distance);
    }

    [Fact]
    public void Equals_ReturnsTrueForEqualLocations()
    {
        var location1 = new Location(1, 1);
        var location2 = new Location(1, 1);

        Assert.True(location1.Equals(location2));
    }

    [Fact]
    public void EqualityOperators_WorkCorrectly()
    {
        var location1 = new Location(2, 2);
        var location2 = new Location(2, 2);
        var location3 = new Location(3, 3);

        Assert.True(location1 == location2);
        Assert.False(location1 == location3);
        Assert.False(location1 != location2);
        Assert.True(location1 != location3);
    }

    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualObjects()
    {
        var location1 = new Location(5, 5);
        var location2 = new Location(5, 5);

        Assert.Equal(location1.GetHashCode(), location2.GetHashCode());
    }

    // Additional tests can be written for the validation of specific reachable and nearby location methods.
}
