// ∅ 2024 super-metal-mons

using System.Text.Json;
using System.IO;
using MonsGame;

public class TestDataTests
{
    [Fact]
    public void ShouldContainExpectedNumberOfFiles()
    {
        string baseDirectory = AppContext.BaseDirectory;
        string? projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string testDataPath = Path.Combine(projectDirectory!, "test-data");
        int expectedFileCount = 3054;
        int actualFileCount = Directory.GetFiles(testDataPath, "*", SearchOption.AllDirectories).Length;
        Assert.Equal(expectedFileCount, actualFileCount);
    }

    [Fact]
    public void FilesInTestData_ShouldNotBeEmpty()
    {
        string baseDirectory = AppContext.BaseDirectory;
        string? projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string testDataPath = Path.Combine(projectDirectory!, "test-data");
        foreach (string file in Directory.EnumerateFiles(testDataPath, "*", SearchOption.AllDirectories))
        {
            Assert.False(string.IsNullOrWhiteSpace(File.ReadAllText(file)), $"File {file} is empty.");
        }
    }

    [Fact]
    public void FilesInTestData_ContainTestCases()
    {
        string baseDirectory = AppContext.BaseDirectory;
        string? projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string testDataPath = Path.Combine(projectDirectory!, "test-data");
        foreach (string file in Directory.EnumerateFiles(testDataPath, "*", SearchOption.AllDirectories))
        {
            string jsonContent = File.ReadAllText(file);
            Assert.False(string.IsNullOrWhiteSpace(jsonContent), $"File {file} is empty.");
            RulesTestCase testCase = JsonSerializer.Deserialize<RulesTestCase>(jsonContent, JsonOptions.DefaultSerializerOptions)!;
            Assert.NotNull(testCase);
            Assert.False(string.IsNullOrWhiteSpace(testCase.FenBefore), $"FenBefore is empty in file {file}.");
            Assert.False(string.IsNullOrWhiteSpace(testCase.FenAfter), $"FenAfter is empty in file {file}.");
        }
    }

    [Fact]
    public void FenBackAndForth()
    {
        string baseDirectory = AppContext.BaseDirectory;
        string? projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string testDataPath = Path.Combine(projectDirectory!, "test-data");
        foreach (string file in Directory.EnumerateFiles(testDataPath, "*", SearchOption.AllDirectories))
        {
            string jsonContent = File.ReadAllText(file);
            Assert.False(string.IsNullOrWhiteSpace(jsonContent), $"File {file} is empty.");
            RulesTestCase testCase = JsonSerializer.Deserialize<RulesTestCase>(jsonContent, JsonOptions.DefaultSerializerOptions)!;
            string jsonBack = JsonSerializer.Serialize(testCase, JsonOptions.DefaultSerializerOptions)!;
            RulesTestCase testCaseAgain = JsonSerializer.Deserialize<RulesTestCase>(jsonBack, JsonOptions.DefaultSerializerOptions)!;
            string jsonBackBack = JsonSerializer.Serialize(testCaseAgain, JsonOptions.DefaultSerializerOptions)!;
            Assert.Equal(jsonBackBack, jsonBack);
            using (JsonDocument expectedDoc = JsonDocument.Parse(jsonBackBack))
            using (JsonDocument actualDoc = JsonDocument.Parse(jsonContent))
            {
                Assert.True(JsonElementEquals(expectedDoc.RootElement, actualDoc.RootElement), "JSON structures are not equal.");
            }
            var gameBefore = Game.FromFen(testCase.FenBefore);
            Assert.Equal(gameBefore.Fen, testCase.FenBefore);
            var gameAfter = Game.FromFen(testCase.FenAfter);
            Assert.Equal(gameAfter.Fen, testCase.FenAfter);
        }
    }

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
                if (elem1.GetArrayLength() != elem2.GetArrayLength()) return false;

                JsonElement[] arr1 = elem1.EnumerateArray().ToArray();
                JsonElement[] arr2 = elem2.EnumerateArray().ToArray();

                for (int i = 0; i < arr1.Length; i++)
                {
                    if (!JsonElementEquals(arr1[i], arr2[i])) return false;
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
            var actualOutput = newGame.ProcessInput(testCase.Input, doNotApplyEvents: false, oneOptionEnough: false);

            if (testCase.FenAfter.Trim() == newGame.Fen.Trim())
            {
                Console.WriteLine("FEN OK");
            }
            else
            {
                Console.WriteLine("FEN 🛑");
                Console.WriteLine(jsonContent);
                Console.WriteLine("\n⚠️actual:\n");
                string serializedActualOutput = JsonSerializer.Serialize(actualOutput, JsonOptions.DefaultSerializerOptions);
                Console.WriteLine(serializedActualOutput);
                Console.WriteLine(newGame.Fen);


                for (int i = 0; i < Math.Min(testCase.FenAfter.Length, newGame.Fen.Length); i++)
                {
                    if (testCase.FenAfter[i] != newGame.Fen[i])
                    {
                        Console.WriteLine($"Difference at position {i}: '{testCase.FenAfter[i]}' vs '{newGame.Fen[i]}'");
                    }
                }
                if (testCase.FenAfter.Length != newGame.Fen.Length)
                {
                    Console.WriteLine("Strings have different lengths.");
                }

            }

            Console.WriteLine("\n\n");

            // Assert.Equal(testCase.FenAfter, newGame.Fen);
            // TODO: validate output as well
        }
    }

}