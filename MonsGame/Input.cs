// ∅ 2024 super-metal-mons

namespace MonsGame;

public abstract class Input
{
    public class LocationInput : Input, IEquatable<LocationInput>
    {
        public Location Location { get; }

        public LocationInput(Location location)
        {
            Location = location;
        }

        public bool Equals(LocationInput? other)
        {
            if (other is null) return false;
            return Location.Equals(other.Location);
        }

        public override bool Equals(object? obj) => Equals(obj as LocationInput);

        public override int GetHashCode()
        {
            return HashCode.Combine(Location);
        }
    }

    public class ModifierInput : Input, IEquatable<ModifierInput>
    {
        public Modifier Modifier { get; }

        public ModifierInput(Modifier modifier)
        {
            Modifier = modifier;
        }

        public bool Equals(ModifierInput? other)
        {
            if (other is null) return false;
            return Modifier == other.Modifier;
        }

        public override bool Equals(object? obj) => Equals(obj as ModifierInput);

        public override int GetHashCode()
        {
            return HashCode.Combine(Modifier);
        }
    }
}
