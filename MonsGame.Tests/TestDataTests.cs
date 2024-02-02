// ∅ 2024 super-metal-mons

using System.Text.Json;
using System.IO;
using MonsGame;

public class TestDataTests
{

    static bool JsonElementEquals(JsonElement elem1, JsonElement elem2)
    {
        if (elem1.ValueKind != elem2.ValueKind) return false;

        switch (elem1.ValueKind)
        {
            case JsonValueKind.Object:
                var properties1 = elem1.EnumerateObject();
                var properties2 = elem2.EnumerateObject();
                var dict1 = properties1.ToDictionary(p => p.Name, p => p.Value);
                var dict2 = properties2.ToDictionary(p => p.Name, p => p.Value);

                if (dict1.Count != dict2.Count) return false;

                foreach (var kvp in dict1)
                {
                    if (!dict2.ContainsKey(kvp.Key)) return false;
                    if (!JsonElementEquals(kvp.Value, dict2[kvp.Key])) return false;
                }
                return true;

            case JsonValueKind.Array:
                var arr1 = elem1.EnumerateArray().ToList();
                var arr2 = elem2.EnumerateArray().ToList();

                if (arr1.Count != arr2.Count) return false;

                foreach (var item1 in arr1)
                {
                    bool matchFound = false;
                    foreach (var item2 in arr2)
                    {
                        if (JsonElementEquals(item1, item2))
                        {
                            arr2.Remove(item2);
                            matchFound = true;
                            break;
                        }
                    }
                    if (!matchFound) return false;
                }
                return true;

            case JsonValueKind.String:
            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
                return elem1.ToString() == elem2.ToString();

            default:
                throw new ArgumentException("Unsupported JsonValueKind");
        }
    }

    [Fact]
    public void RulesOK()
    {
        string baseDirectory = AppContext.BaseDirectory;
        string? projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string testDataPath = Path.Combine(projectDirectory!, "test-data");
        foreach (string file in Directory.EnumerateFiles(testDataPath, "*", SearchOption.AllDirectories))
        {
            string jsonContent = File.ReadAllText(file);
            RulesTestCase testCase = JsonSerializer.Deserialize<RulesTestCase>(jsonContent, JsonOptions.DefaultSerializerOptions)!;
            var newGame = Game.FromFen(testCase.FenBefore);
            var actualOutput = newGame.ProcessInput(testCase.Input);
            string serializedActualOutput = JsonSerializer.Serialize(actualOutput, JsonOptions.DefaultSerializerOptions);
            string serializedExpectedOutput = JsonSerializer.Serialize(testCase.Output, JsonOptions.DefaultSerializerOptions);
            JsonDocument actualDoc = JsonDocument.Parse(serializedActualOutput);
            JsonDocument expectedDoc = JsonDocument.Parse(serializedExpectedOutput);
            Assert.Equal(testCase.FenAfter, newGame.Fen);
            Assert.True(JsonElementEquals(expectedDoc.RootElement, actualDoc.RootElement), "JSON structures are not equal.");
        }
    }

}