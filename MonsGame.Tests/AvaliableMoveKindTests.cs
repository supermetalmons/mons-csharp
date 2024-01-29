// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class AvailableMoveKindTests
{
    [Fact]
    public void Enum_ContainsExpectedValues()
    {
        // Asserting that all expected values are present
        Assert.Equal(0, (int)AvailableMoveKind.MonMove);
        Assert.Equal(1, (int)AvailableMoveKind.ManaMove);
        Assert.Equal(2, (int)AvailableMoveKind.Action);
        Assert.Equal(3, (int)AvailableMoveKind.Potion);
    }

    [Fact]
    public void Enum_ContainsExpectedNumberOfValues()
    {
        // Asserting the expected number of items in the enum
        var values = Enum.GetValues(typeof(AvailableMoveKind));
        Assert.Equal(4, values.Length);
    }

    [Fact]
    public void Enum_HasSerializableAttribute()
    {
        // Asserting that the enum is marked with the Serializable attribute
        var attributes = typeof(AvailableMoveKind).GetCustomAttributes(typeof(SerializableAttribute), false);
        Assert.True(attributes.Length > 0);
    }
}
