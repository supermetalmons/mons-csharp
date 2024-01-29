// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class MonsGameTests
{
    [Fact]
    public void ConsumableEnumShouldSerializeAndDeserializeCorrectly()
    {
        // Arrange
        var consumable = Consumable.Bomb;

        // Act
        var serialized = JsonSerializer.Serialize(consumable);
        var deserialized = JsonSerializer.Deserialize<Consumable>(serialized);

        // Assert
        Assert.Equal("\"Bomb\"", serialized); // Serialized value should be the string representation
        Assert.Equal(Consumable.Bomb, deserialized); // Deserialized value should match the original enum
    }
}
