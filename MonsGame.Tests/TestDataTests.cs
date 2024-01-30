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
        if (projectDirectory == null)
        {
            throw new InvalidOperationException("Cannot determine the project directory.");
        }
        string testDataPath = Path.Combine(projectDirectory!, "test-data");
        foreach (string file in Directory.EnumerateFiles(testDataPath, "*", SearchOption.AllDirectories))
        {
            Assert.False(string.IsNullOrWhiteSpace(File.ReadAllText(file)), $"File {file} is empty.");
        }
    }
    
}