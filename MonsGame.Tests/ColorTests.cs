// ∅ 2024 super-metal-mons

using System.Text.Json;
using MonsGame;

public class ColorTests
{
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
