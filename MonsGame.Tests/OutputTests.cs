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

}
