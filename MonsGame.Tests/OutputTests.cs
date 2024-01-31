// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class OutputTests
{
    [Fact]
    public void LocationsToStartFromOutput_ConstructorAndEqualsWorkCorrectly()
    {
        var locations = new HashSet<Location> { new Location(0, 0), new Location(1, 1) };
        var output1 = new LocationsToStartFromOutput(locations);
        var output2 = new LocationsToStartFromOutput(new List<Location> { new Location(0, 0), new Location(1, 1) });

        Assert.Equal(output1, output2);
        Assert.Equal(output1.GetHashCode(), output2.GetHashCode());
    }


    [Fact]
    public void NextInputOptionsOutput_ConstructorAndEqualsWorkCorrectly()
    {
        var nextInputs = new HashSet<NextInput>
        {
            new NextInput(new Input.LocationInput(new Location(0, 0)), NextInputKind.MonMove),
            new NextInput(new Input.LocationInput(new Location(0, 0)), NextInputKind.ManaMove)
        };
        var output1 = new NextInputOptionsOutput(nextInputs);
        var output2 = new NextInputOptionsOutput(nextInputs);

        Assert.Equal(output1, output2);
        Assert.Equal(output1.GetHashCode(), output2.GetHashCode());
    }

    [Fact]
    public void EventsOutput_ConstructorAndEqualsWorkCorrectly()
    {
        var mana = Mana.Regular(Color.Black);
        var item = Item.ManaItem(mana);
        var events = new HashSet<Event>
        {
            new MonMoveEvent(item, new Location(0, 0), new Location(1, 1))
        };
        var output1 = new EventsOutput(events);
        var output2 = new EventsOutput(new List<Event>(events));

        Assert.Equal(output1, output2);
        Assert.Equal(output1.GetHashCode(), output2.GetHashCode());
    }

    [Fact]
    public void InvalidInputOutput_Serialization_Deserialization_Test()
    {
        string json = "{\"invalidInput\":{}}";
        ValidateSerializationAndDeserialization<Output>(json);
    }

    [Fact]
    public void EventsOutput_Serialization_Deserialization_Test()
    {
        string json = "{\"events\":{\"_0\":[{\"monMove\":{\"from\":{\"i\":0,\"j\":4},\"to\":{\"i\":1,\"j\":4},\"item\":{\"mon\":{\"mon\":{\"color\":\"black\",\"cooldown\":0,\"kind\":\"spirit\"}}}}}]}}";
        ValidateSerializationAndDeserialization<Output>(json);
    }

    [Fact]
    public void LocationsToStartFromOutput_Serialization_Deserialization_Test()
    {
        string json = "{\"locationsToStartFrom\":{\"_0\":[{\"i\":7,\"j\":6},{\"i\":6,\"j\":7},{\"i\":5,\"j\":6},{\"i\":7,\"j\":4},{\"i\":3,\"j\":6}]}}";
        ValidateSerializationAndDeserialization<Output>(json);
    }

    [Fact]
    public void NextInputOptionsOutput_Serialization_Deserialization_Test()
    {
        string json = "{\"nextInputOptions\":{\"_0\":[{\"kind\":{\"monMove\":{}},\"input\":{\"location\":{\"_0\":{\"i\":3,\"j\":4}}}},"
                    + "{\"kind\":{\"monMove\":{}},\"input\":{\"location\":{\"_0\":{\"i\":4,\"j\":5}}}}]}}";
        ValidateSerializationAndDeserialization<Output>(json);
    }

    private void ValidateSerializationAndDeserialization<T>(string json)
    {
        var deserialized = JsonSerializer.Deserialize<T>(json, JsonOptions.DefaultSerializerOptions);
        var serializedBack = JsonSerializer.Serialize(deserialized, JsonOptions.DefaultSerializerOptions);
        Assert.Equal(json, serializedBack);
    }

}
