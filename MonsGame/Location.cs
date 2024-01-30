// ∅ 2024 super-metal-mons

namespace MonsGame;

using System.Collections.Generic;
using System.Linq;

public struct Location : IEquatable<Location>
{
    private static readonly Range ValidRange = 0..Config.BoardSize;

    public int I { get; }
    public int J { get; }

    public Location(int i, int j)
    {
        I = i;
        J = j;
    }

    public IEnumerable<Location> NearbyLocations => GetNearbyLocations(1);

    public IEnumerable<Location> ReachableByBomb => GetNearbyLocations(3);

    public IEnumerable<Location> ReachableByMysticAction => new[]
    {
        (I - 2, J - 2), (I + 2, J + 2), (I - 2, J + 2), (I + 2, J - 2)
    }.Where(tuple => IsValidLocation(tuple.Item1, tuple.Item2))
      .Select(tuple => new Location(tuple.Item1, tuple.Item2));

    public IEnumerable<Location> ReachableByDemonAction => new[]
    {
        (I - 2, J), (I + 2, J), (I, J + 2), (I, J - 2)
    }.Where(tuple => IsValidLocation(tuple.Item1, tuple.Item2))
      .Select(tuple => new Location(tuple.Item1, tuple.Item2));

    public IEnumerable<Location> ReachableBySpiritAction
    {
        get
        {
            var locations = new List<Location>();
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    if (Math.Max(Math.Abs(x), Math.Abs(y)) == 2)
                    {
                        int a = I + x;
                        int b = J + y;
                        if (IsValidLocation(a, b))
                        {
                            locations.Add(new Location(a, b));
                        }
                    }
                }
            }
            return locations;
        }
    }

    public Location LocationBetween(Location another) =>
        new Location((I + another.I) / 2, (J + another.J) / 2);

    private IEnumerable<Location> GetNearbyLocations(int distance)
    {
        var locations = new List<Location>();
        for (int x = I - distance; x <= I + distance; x++)
        {
            for (int y = J - distance; y <= J + distance; y++)
            {
                if (IsValidLocation(x, y) && (x != I || y != J))
                {
                    locations.Add(new Location(x, y));
                }
            }
        }
        return locations;
    }

    private static bool IsValidLocation(int x, int y) =>
    x >= ValidRange.Start.Value && x < ValidRange.End.Value &&
    y >= ValidRange.Start.Value && y < ValidRange.End.Value;


    public int DistanceTo(Location to) =>
        Math.Max(Math.Abs(to.I - I), Math.Abs(to.J - J));

    public override bool Equals(object obj) =>
        obj is Location location && this == location;

    public bool Equals(Location other) =>
        I == other.I && J == other.J;

    public override int GetHashCode() =>
        HashCode.Combine(I, J);

    public static bool operator ==(Location left, Location right) =>
        left.Equals(right);

    public static bool operator !=(Location left, Location right) =>
        !(left == right);
}
