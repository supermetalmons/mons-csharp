// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class GameOverEventTests
{
    [Theory]
    [InlineData(Color.White)]
    [InlineData(Color.Black)]
    public void Constructor_CorrectlyInitializesWinner(Color expectedWinner)
    {
        var gameOverEvent = new GameOverEvent(expectedWinner);

        Assert.Equal(expectedWinner, gameOverEvent.Winner);
    }

    [Fact]
    public void MonMoveEvent_Serialization_Deserialization_Test()
    {
        string json = "{\"monMove\":{\"from\":{\"i\":7,\"j\":4},\"to\":{\"i\":5,\"j\":7},\"item\":{\"mon\":{\"mon\":{\"color\":\"black\",\"cooldown\":0,\"kind\":\"spirit\"}}}}}";
        ValidateSerializationAndDeserialization<Event>(json);
    }

    [Fact]
    public void MysticActionEvent_Serialization_Deserialization_Test()
    {
        string json = "{\"mysticAction\":{\"mystic\":{\"color\":\"white\",\"cooldown\":0,\"kind\":\"mystic\"},\"from\":{\"i\":10,\"j\":3},\"to\":{\"i\":1,\"j\":8}}}";
        ValidateSerializationAndDeserialization<Event>(json);
    }

    [Fact]
    public void MonFaintedEvent_Serialization_Deserialization_Test()
    {
        string json = "{\"monFainted\":{\"mon\":{\"color\":\"black\",\"cooldown\":0,\"kind\":\"demon\"},\"from\":{\"i\":1,\"j\":8},\"to\":{\"i\":0,\"j\":7}}}";
        ValidateSerializationAndDeserialization<Event>(json);
    }

    [Fact]
    public void ManaMoveEvent_Serialization_Deserialization_Test()
    {
        string json = "{\"manaMove\":{\"mana\":{\"regular\":{\"color\":\"black\"}},\"from\":{\"i\":4,\"j\":7},\"to\":{\"i\":8,\"j\":3}}}";
        ValidateSerializationAndDeserialization<Event>(json);
    }

    [Fact]
    public void NextTurnEvent_Serialization_Deserialization_Test()
    {
        string json = "{\"nextTurn\":{\"color\":\"white\"}}";
        ValidateSerializationAndDeserialization<Event>(json);
    }

    [Fact]
    public void PickupManaEvent_Serialization_Deserialization_Test()
    {
        string json = "{\"pickupMana\":{\"mana\":{\"regular\":{\"color\":\"white\"}},\"by\":{\"color\":\"white\",\"cooldown\":0,\"kind\":\"drainer\"},\"at\":{\"i\":7,\"j\":7}}}";
        ValidateSerializationAndDeserialization<Event>(json);
    }

    [Fact]
    public void DemonAdditionalStepEvent_Serialization_Deserialization_Test()
    {
        string json = "{\"demonAdditionalStep\":{\"demon\":{\"color\":\"white\",\"cooldown\":0,\"kind\":\"demon\"},\"from\":{\"i\":4,\"j\":6},\"to\":{\"i\":6,\"j\":5}}}";
        ValidateSerializationAndDeserialization<Event>(json);
    }

    private void ValidateSerializationAndDeserialization<T>(string json)
    {
        var deserialized = JsonSerializer.Deserialize<T>(json, JsonOptions.DefaultSerializerOptions);
        var serializedBack = JsonSerializer.Serialize(deserialized, JsonOptions.DefaultSerializerOptions);
        Assert.Equal(json, serializedBack);
    }

}
