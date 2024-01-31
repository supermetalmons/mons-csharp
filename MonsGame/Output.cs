// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

[JsonConverter(typeof(OutputJsonConverter))]
public abstract class Output
{
    // Base class for different outputs
}

public class InvalidInputOutput : Output
{

}

public class LocationsToStartFromOutput : Output, IEquatable<LocationsToStartFromOutput>
{
    public HashSet<Location> Locations { get; }

    public LocationsToStartFromOutput(IEnumerable<Location> locations)
    {
        Locations = new HashSet<Location>(locations);
    }

    public bool Equals(LocationsToStartFromOutput? other)
    {
        if (other == null) return false;
        return Locations.SetEquals(other.Locations);
    }

    public override bool Equals(object? obj) => Equals(obj as LocationsToStartFromOutput);

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

    public bool Equals(NextInputOptionsOutput? other)
    {
        if (other == null) return false;
        return NextInputs.SetEquals(other.NextInputs);
    }

    public override bool Equals(object? obj) => Equals(obj as NextInputOptionsOutput);

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

    public bool Equals(EventsOutput? other)
    {
        if (other == null) return false;
        return Events.SetEquals(other.Events);
    }

    public override bool Equals(object? obj) => Equals(obj as EventsOutput);

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

public class OutputJsonConverter : JsonConverter<Output>
{
    public override Output Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;

            if (root.TryGetProperty("invalidInput", out _))
            {
                return new InvalidInputOutput();
            }
            else if (root.TryGetProperty("events", out var eventsProperty))
            {
                var eventArray = eventsProperty.GetProperty("_0");
                var events = JsonSerializer.Deserialize<List<Event>>(eventArray.GetRawText(), options);
                return new EventsOutput(events);
            }
            else if (root.TryGetProperty("locationsToStartFrom", out var locationsProperty))
            {
                var locationsArray = locationsProperty.GetProperty("_0");
                var locations = JsonSerializer.Deserialize<List<Location>>(locationsArray.GetRawText(), options);
                return new LocationsToStartFromOutput(locations);
            }
            else if (root.TryGetProperty("nextInputOptions", out var nextInputsProperty))
            {
                var nextInputsArray = nextInputsProperty.GetProperty("_0");
                var nextInputs = JsonSerializer.Deserialize<List<NextInput>>(nextInputsArray.GetRawText(), options);
                return new NextInputOptionsOutput(nextInputs);
            }
            else
            {
                throw new JsonException("Invalid JSON for Output");
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, Output value, JsonSerializerOptions options)
    {
        if (value is InvalidInputOutput)
        {
            writer.WriteStartObject();
            writer.WriteStartObject("invalidInput");
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        else if (value is EventsOutput eventsOutput)
        {
            writer.WriteStartObject();
            writer.WriteStartObject("events");
            writer.WritePropertyName("_0");
            JsonSerializer.Serialize(writer, eventsOutput.Events, eventsOutput.Events.GetType(), options);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        else if (value is LocationsToStartFromOutput locationsOutput)
        {
            writer.WriteStartObject();
            writer.WriteStartObject("locationsToStartFrom");
            writer.WriteStartArray("_0");
            foreach (var location in locationsOutput.Locations)
            {
                JsonSerializer.Serialize(writer, location, location.GetType(), options);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        else if (value is NextInputOptionsOutput nextInputsOutput)
        {
            writer.WriteStartObject();
            writer.WriteStartObject("nextInputOptions");
            writer.WriteStartArray("_0");
            foreach (var nextInput in nextInputsOutput.NextInputs)
            {
                JsonSerializer.Serialize(writer, nextInput, nextInput.GetType(), options);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        else
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}