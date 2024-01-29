// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Collections.Generic;

public abstract class Output
{
    // Base class for different outputs
}

public class InvalidInputOutput : Output
{
    // Specific functionality for InvalidInput, if needed.
}

public class LocationsToStartFromOutput : Output, IEquatable<LocationsToStartFromOutput>
{
    public HashSet<Location> Locations { get; }

    public LocationsToStartFromOutput(IEnumerable<Location> locations)
    {
        Locations = new HashSet<Location>(locations);
    }

    public bool Equals(LocationsToStartFromOutput other)
    {
        if (other == null) return false;
        return Locations.SetEquals(other.Locations);
    }

    public override bool Equals(object obj) => Equals(obj as LocationsToStartFromOutput);

    public override int GetHashCode()
    {
        int hash = 0;
        foreach (var location in Locations)
        {
            hash ^= location.GetHashCode();
        }
        return hash;
    }

}

public class NextInputOptionsOutput : Output, IEquatable<NextInputOptionsOutput>
{
    public HashSet<NextInput> NextInputs { get; }

    public NextInputOptionsOutput(IEnumerable<NextInput> nextInputs)
    {
        NextInputs = new HashSet<NextInput>(nextInputs);
    }

    public bool Equals(NextInputOptionsOutput other)
    {
        if (other == null) return false;
        return NextInputs.SetEquals(other.NextInputs);
    }

    public override bool Equals(object obj) => Equals(obj as NextInputOptionsOutput);

    public override int GetHashCode()
    {
        int hash = 0;
        foreach (var nextInput in NextInputs)
        {
            hash ^= nextInput.GetHashCode();
        }
        return hash;
    }
}

public class EventsOutput : Output, IEquatable<EventsOutput>
{
    public HashSet<Event> Events { get; }

    public EventsOutput(IEnumerable<Event> events)
    {
        Events = new HashSet<Event>(events);
    }

    public bool Equals(EventsOutput other)
    {
        if (other == null) return false;
        return Events.SetEquals(other.Events);
    }

    public override bool Equals(object obj) => Equals(obj as EventsOutput);

    public override int GetHashCode()
    {
        int hash = 0;
        foreach (var evt in Events)
        {
            hash ^= evt.GetHashCode();
        }
        return hash;
    }

}
