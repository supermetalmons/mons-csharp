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
            var gameBefore = Game.FromFen(testCase.FenBefore);
            Assert.Equal(gameBefore.Fen, testCase.FenBefore);

            var gameAfter = Game.FromFen(testCase.FenAfter);
            Assert.Equal(gameAfter.Fen, testCase.FenAfter);
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

        }
    }

}