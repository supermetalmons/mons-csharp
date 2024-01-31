// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class NextInputTests
{
    [Theory]
    [InlineData(NextInputKind.MonMove)]
    [InlineData(NextInputKind.ManaMove)]
    public void Constructor_CorrectlyInitializesProperties(NextInputKind kind)
    {
        var location = new Location(0, 0);
        var locationInput = new Input.LocationInput(location);
        var mon = new Mon(Mon.Kind.Spirit, Color.White);
        var item = Item.MonItem(mon);

        var nextInput = new NextInput(locationInput, kind, item);

        Assert.Equal(locationInput, nextInput.Input);
        Assert.Equal(kind, nextInput.Kind);
        Assert.Equal(item, nextInput.ActorMonItem);
    }

    [Fact]
    public void Equals_ReturnsTrueForEqualNextInputs()
    {
        var location = new Location(1, 1);
        var locationInput = new Input.LocationInput(location);
        var mon = new Mon(Mon.Kind.Spirit, Color.White);
        var item = Item.MonItem(mon);

        var nextInput1 = new NextInput(locationInput, NextInputKind.MysticAction, item);
        var nextInput2 = new NextInput(locationInput, NextInputKind.MysticAction, item);

        Assert.True(nextInput1.Equals(nextInput2));
        Assert.True(nextInput1 == nextInput2);
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentNextInputs()
    {
        var location1 = new Location(2, 2);
        var locationInput1 = new Input.LocationInput(location1);
        var location2 = new Location(3, 3);
        var locationInput2 = new Input.LocationInput(location2);
        var mon = new Mon(Mon.Kind.Spirit, Color.White);
        var item = Item.MonItem(mon);

        var nextInput1 = new NextInput(locationInput1, NextInputKind.DemonAction, item);
        var nextInput2 = new NextInput(locationInput2, NextInputKind.DemonAction, item);

        Assert.False(nextInput1.Equals(nextInput2));
        Assert.True(nextInput1 != nextInput2);
    }

    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualObjects()
    {
        var location = new Location(4, 4);
        var locationInput = new Input.LocationInput(location);
        var mon = new Mon(Mon.Kind.Spirit, Color.White);
        var item = Item.MonItem(mon);

        var nextInput1 = new NextInput(locationInput, NextInputKind.SpiritTargetCapture, item);
        var nextInput2 = new NextInput(locationInput, NextInputKind.SpiritTargetCapture, item);

        Assert.Equal(nextInput1.GetHashCode(), nextInput2.GetHashCode());
    }

    [Fact]
    public void NextInput_Serialization_Deserialization_SpiritTargetMove_Test()
    {
        string json = "{\"kind\":{\"spiritTargetMove\":{}},\"input\":{\"location\":{\"_0\":{\"i\":0,\"j\":4}}}}";
        var deserializedInput = JsonSerializer.Deserialize<NextInput>(json, JsonOptions.DefaultSerializerOptions);
        string jsonBack = JsonSerializer.Serialize(deserializedInput, JsonOptions.DefaultSerializerOptions);
        Assert.Equal(json, jsonBack);
    }

    [Fact]
    public void NextInput_Serialization_Deserialization_MonMove_Test()
    {
        string json = "{\"kind\":{\"monMove\":{}},\"input\":{\"location\":{\"_0\":{\"i\":9,\"j\":6}}}}";
        var deserializedInput = JsonSerializer.Deserialize<NextInput>(json, JsonOptions.DefaultSerializerOptions);
        string jsonBack = JsonSerializer.Serialize(deserializedInput, JsonOptions.DefaultSerializerOptions);
        Assert.Equal(json, jsonBack);
    }

    [Fact]
    public void NextInput_Serialization_Deserialization_SelectConsumable_Test()
    {
        string json = "{\"kind\":{\"selectConsumable\":{}},\"actorMonItem\":{\"mon\":{\"mon\":{\"color\":\"black\",\"cooldown\":0,\"kind\":\"mystic\"}}},\"input\":{\"modifier\":{\"_0\":\"selectBomb\"}}}";
        var deserializedInput = JsonSerializer.Deserialize<NextInput>(json, JsonOptions.DefaultSerializerOptions);
        string jsonBack = JsonSerializer.Serialize(deserializedInput, JsonOptions.DefaultSerializerOptions);
        Assert.Equal(json, jsonBack);
    }


}
