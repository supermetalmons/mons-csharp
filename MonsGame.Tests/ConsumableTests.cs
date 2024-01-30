// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class ConsumableTests
{
    [Fact]
    public void ConsumableEnumShouldSerializeAndDeserializeCorrectly()
    {
        var consumable = Consumable.Bomb;
        var serialized = JsonSerializer.Serialize(consumable);
        var deserialized = JsonSerializer.Deserialize<Consumable>(serialized);
        Assert.Equal("\"Bomb\"", serialized);
        Assert.Equal(Consumable.Bomb, deserialized);
    }
}
