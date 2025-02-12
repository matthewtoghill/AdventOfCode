using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Tools.Models;

namespace AdventOfCode.Tools;

public static partial class StringExtensions
{
    public static string[] SplitLines(this string text, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        => text.Split(["\n", "\r\n"], splitOptions);

    public static string[] SplitParagraphs(this string text, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        => text.Split(["\n\n", "\r\n\r\n"], splitOptions);

    public static IEnumerable<T> SplitTo<T>(this string text, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries) where T : IParsable<T>
        => text.SplitLines(splitOptions)
               .Select(x => T.Parse(x, null));

    public static IEnumerable<T> SplitTo<T>(this string text, params string[] separators) where T : IParsable<T>
        => text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
               .Select(x => T.Parse(x, null));

    public static string[] SplitOn(this string text, StringSplitOptions splitOptions, params string[] separators)
        => text.Split(separators, splitOptions);

    public static string[] SplitOn(this string text, params string[] separators)
        => text.Split(separators, StringSplitOptions.None);

    public static string[] SplitOn(this string text, StringSplitOptions splitOptions, params char[] separators)
        => text.Split(separators, splitOptions);

    public static string[] SplitOn(this string text, params char[] separators)
        => text.Split(separators, StringSplitOptions.None);

    public static IEnumerable<string> SplitIntoColumns(this string text)
    {
        var rows = text.SplitLines();
        var cols = rows[0].Length;
        List<string> result = [];
        for (int i = 0; i < cols; i++)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
            {
                sb.Append(row[i]);
            }
            result.Add(sb.ToString());
        }
        return result;
    }

    public static IEnumerable<string> SplitIntoColumns(this string text, int columnWidth)
    {
        var rows = text.SplitLines();
        var cols = rows[0].Length / columnWidth;
        List<string> result = [];
        for (int i = 0; i < cols; i++)
        {
            StringBuilder sb = new();
            foreach (var row in rows)
            {
                sb.Append(row[(i*columnWidth)..((i*columnWidth)+columnWidth)]);
            }
            result.Add(sb.ToString());
        }
        return result;
    }

    public static Dictionary<Position, char> AsCharMap(this string[] lines)
        => (from row in Enumerable.Range(0, lines.Length)
            from col in Enumerable.Range(0, lines[row].Length)
            select new KeyValuePair<Position, char>(new(col, row), lines[row][col])).ToDictionary();

    public static Dictionary<Position, int> AsIntMap(this string[] lines)
        => (from row in Enumerable.Range(0, lines.Length)
            from col in Enumerable.Range(0, lines[row].Length)
            select new KeyValuePair<Position, int>(new(col, row), lines[row][col].ToInt())).ToDictionary();

    public static Dictionary<Position, TValue> AsMap<TValue>(string[] input) where TValue : IParsable<TValue>
        => (from row in Enumerable.Range(0, input.Length)
            from col in Enumerable.Range(0, input[row].Length)
            select new KeyValuePair<Position, TValue>(new(col, row), TValue.Parse(input[row][col].ToString(), null))).ToDictionary();

    public static string Left(this string input, int length)
    {
        return input.Length > length ? input[..length] : input;
    }

    public static string Right(this string input, int length)
    {
        return input.Length > length ? input[^length..] : input;
    }

    public static string LeftOf(this string input, char c)
    {
        int i = input.IndexOf(c);
        if (i >= 0) return input.Substring(0, i);
        return input;
    }

    public static string LeftOf(this string input, string s)
    {
        int i = input.IndexOf(s);
        if (i >= 0) return input[..i];
        return input;
    }

    public static string RightOf(this string input, char c)
    {
        int i = input.IndexOf(c);
        if (i == -1) return input;
        return input[(i + 1)..];
    }

    public static string RightOf(this string input, string s)
    {
        int i = input.IndexOf(s);
        if (i == -1) return input;
        return input[(i + s.Length)..];
    }

    public static string RightOfLast(this string input, char c)
    {
        int i = input.LastIndexOf(c);
        if (i == -1) return input;
        return input[(i + 1)..];
    }

    public static string RightOfLast(this string input, string s)
    {
        int i = input.LastIndexOf(s);
        if (i == -1) return input;
        return input[(i + s.Length)..];
    }

    public static string Mid(this string input, int start)
    {
        return input.Substring(Math.Min(start, input.Length));
    }

    public static string Mid(this string input, int start, int count)
    {
        return input.Substring(Math.Min(start, input.Length), Math.Min(count, Math.Max(input.Length - start, 0)));
    }

    public static string GetBetween(this string input, string before, string after)
    {
        int beforeIndex = input.IndexOf(before);
        int startIndex = beforeIndex + before.Length;
        int afterIndex = input.IndexOf(after, startIndex);

        if (beforeIndex == -1 || afterIndex == -1) return "";

        return input.Substring(startIndex, afterIndex - startIndex);
    }

    public static string StripOut(this string input, char character)
    {
        return input.Replace(character.ToString(), "");
    }

    public static string StripOut(this string input, params char[] chars)
    {
        foreach (char c in chars)
        {
            input = input.Replace(c.ToString(), "");
        }
        return input;
    }

    public static string StripOut(this string input, string subString)
    {
        return input.Replace(subString, "");
    }

    public static string StripOut(this string input, params string[] words)
    {
        foreach (var word in words)
        {
            input = input.Replace(word, "");
        }
        return input;
    }

    public static string SortString(string input)
    {
        char[] chars = input.ToCharArray();
        Array.Sort(chars);
        return new string(chars);
    }

    public static void PrintAllLines(this string[] lines, bool includeLineNums = false)
    {
        if (includeLineNums)
        {
            int count = lines.Length;
            string formatString = " {0," + count.ToString().Length + "}: {1}";
            for (int i = 0; i < count; i++)
                Console.WriteLine(formatString, i, lines[i]);
        }
        else
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }

    public static string ReplaceAtIndex(this string text, int index, char c)
    {
        StringBuilder sb = new(text);
        sb[index] = c;
        return sb.ToString();
    }

    public static string Repeat(this string text, int count, string separator = "")
        => new StringBuilder((text.Length + separator.Length) * count)
            .Insert(0, text + separator, count)
            .ToString();

    public static string Repeat(this char c, int count, string separator = "")
        => new StringBuilder((1 + separator.Length) * count)
            .Insert(0, c + separator, count)
            .ToString();

    public static void IterateGrid(this string[] lines, Action<int, int, char> action)
    {
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                action(row, col, lines[row][col]);
            }
        }
    }

    public static IEnumerable<T> IterateGrid<T>(this string[] lines, Func<int, int, char, T> func)
    {
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                yield return func(row, col, lines[row][col]);
            }
        }
    }

    public static IEnumerable<T> IterateGrid<T>(this string[] lines, Func<int, int, char, T> func, Func<char, bool> predicate)
    {
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                if (predicate(lines[row][col]))
                    yield return func(row, col, lines[row][col]);
            }
        }
    }

    public static int ToInt(this char c) => c - '0';
    public static int BinaryToInt(this string binary) => Convert.ToInt32(binary, 2);
    public static long BinaryToLong(this string binary) => Convert.ToInt64(binary, 2);

    public static bool IsNullOrWhiteSpace(this string text) => string.IsNullOrWhiteSpace(text);
    public static bool IsNotNullOrWhiteSpace(this string text) => !string.IsNullOrWhiteSpace(text);

    public static IEnumerable<int> ExtractInts(this string text) => NumbersOnlyRegex().Matches(text).Select(m => int.Parse(m.Value));
    public static IEnumerable<int> ExtractPositiveInts(this string text) => PositiveNumbersOnlyRegex().Matches(text).Select(m => int.Parse(m.Value));

    public static IEnumerable<T> ExtractNumeric<T>(this string text) where T : IParsable<T>
        => NumbersOnlyRegex().Matches(text).Select(m => T.Parse(m.Value, null));

    public static IEnumerable<T> ExtractPositiveNumeric<T>(this string text) where T : IParsable<T>
        => PositiveNumbersOnlyRegex().Matches(text).Select(m => T.Parse(m.Value, null));

    [GeneratedRegex(@"-?\d+")] private static partial Regex NumbersOnlyRegex();

    [GeneratedRegex(@"\d+")] private static partial Regex PositiveNumbersOnlyRegex();
}