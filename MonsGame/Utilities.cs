// ∅ 2024 super-metal-mons

namespace MonsGame;

public static class StringExtensions
{
    public static string LowercaseFirst(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        return char.ToLower(str[0]) + str.Substring(1);
    }
}
