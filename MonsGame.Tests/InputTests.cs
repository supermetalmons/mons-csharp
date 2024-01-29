// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class InputTests
{
    [Fact]
    public void LocationInput_Constructor_AssignsLocation()
    {
        var location = new Location(0, 0);
        var locationInput = new Input.LocationInput(location);

        Assert.Equal(location, locationInput.Location);
    }

    [Fact]
    public void LocationInput_Equals_WithNull_ReturnsFalse()
    {
        var locationInput = new Input.LocationInput(new Location(0, 0));

        Assert.False(locationInput.Equals(null));
    }

    [Fact]
    public void LocationInput_GetHashCode_ConsistentOutput()
    {
        var location = new Location(0, 0);
        var locationInput = new Input.LocationInput(location);
        var expectedHashCode = HashCode.Combine(location);

        Assert.Equal(expectedHashCode, locationInput.GetHashCode());
    }

    [Fact]
    public void ModifierInput_Constructor_AssignsModifier()
    {
        var modifierInput = new Input.ModifierInput(Modifier.SelectPotion);

        Assert.Equal(Modifier.SelectPotion, modifierInput.Modifier);
    }

}