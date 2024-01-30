// ∅ 2024 super-metal-mons

using MonsGame;

using System.Text.Json;

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

    [Fact]
    public void LocationInput_Serialization_Deserialization_Test()
    {
        string json = "{\"location\":{\"_0\":{\"i\":1,\"j\":2}}}";
        var deserializedInput = JsonSerializer.Deserialize<Input>(json, JsonOptions.DefaultSerializerOptions)!;
        string jsonBack = JsonSerializer.Serialize(deserializedInput, JsonOptions.DefaultSerializerOptions)!;
        var deserializedBackInput = JsonSerializer.Deserialize<Input>(jsonBack, JsonOptions.DefaultSerializerOptions)!;
        Assert.Equal(json, jsonBack);        
        Assert.Equal(deserializedBackInput, deserializedInput);        
    }

    [Fact]
    public void ModifierInput_Serialization_Deserialization_Test()
    {
        string json = "{\"modifier\":{\"_0\":\"selectBomb\"}}";
        var deserializedInput = JsonSerializer.Deserialize<Input>(json, JsonOptions.DefaultSerializerOptions)!;
        string jsonBack = JsonSerializer.Serialize(deserializedInput, JsonOptions.DefaultSerializerOptions)!;
        Assert.Equal(jsonBack, json);
    }

}