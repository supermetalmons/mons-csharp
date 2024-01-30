// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class AvailableMoveKindTests
{
    [Fact]
    public void Enum_ContainsExpectedValues()
    {
        Assert.Equal(0, (int)AvailableMoveKind.MonMove);
        Assert.Equal(1, (int)AvailableMoveKind.ManaMove);
        Assert.Equal(2, (int)AvailableMoveKind.Action);
        Assert.Equal(3, (int)AvailableMoveKind.Potion);
    }

    [Fact]
    public void Enum_ContainsExpectedNumberOfValues()
    {
        var values = Enum.GetValues(typeof(AvailableMoveKind));
        Assert.Equal(4, values.Length);
    }

    [Fact]
    public void AvailableMoveKindEnumShouldSerializeAndDeserializeCorrectly()
    {
        var move = AvailableMoveKind.ManaMove;
        var serialized = JsonSerializer.Serialize(move);
        var deserialized = JsonSerializer.Deserialize<AvailableMoveKind>(serialized);
        Assert.Equal("\"manaMove\"", serialized);
        Assert.Equal(AvailableMoveKind.ManaMove, deserialized);
    }

}
