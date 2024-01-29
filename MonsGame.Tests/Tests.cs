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

    [Fact]
    public void Other_ShouldReturnOppositeColor()
    {
        Assert.Equal(Color.White, Color.Black.Other());
        Assert.Equal(Color.Black, Color.White.Other());
    }

    [Fact]
    public void RandomColor_ShouldReturnValidColor()
    {
        var colors = new HashSet<Color>();
        for (int i = 0; i < 100; i++)
        {
            colors.Add(ColorExtensions.RandomColor());
        }

        // Check if all colors are present
        Assert.Contains(Color.Black, colors);
        Assert.Contains(Color.White, colors);
    }
}
