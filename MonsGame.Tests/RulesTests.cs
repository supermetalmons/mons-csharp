// ∅ 2024 super-metal-mons

using System.Text.Json;
using System.IO;
using MonsGame;

public class RulesTests
{
    [Fact]
    public void ShouldContainExpectedNumberOfFiles()
    {
        // Arrange
        string baseDirectory = AppContext.BaseDirectory;
        string? projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;

        string testDataPath = Path.Combine(projectDirectory!, "test-data");
        int expectedFileCount = 3054;

        // Act
        int actualFileCount = Directory.GetFiles(testDataPath, "*", SearchOption.AllDirectories).Length;

        // Assert
        Assert.Equal(expectedFileCount, actualFileCount);
    }
}