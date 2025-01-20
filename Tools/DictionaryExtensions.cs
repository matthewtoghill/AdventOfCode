using System.Numerics;

namespace AdventOfCode.Tools;

public static class DictionaryExtensions
{
    public static void IncrementAt<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
        where TValue : INumber<TValue>
    {
        dictionary.TryGetValue(key, out var count);
        dictionary[key] = ++count!;
    }

    public static void IncrementAt<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue incrementAmount)
        where TKey : notnull
        where TValue : INumber<TValue>
    {
        dictionary.TryGetValue(key, out var count);
        dictionary[key] = count! + incrementAmount;
    }

    public static void AppendAt<TKey>(this Dictionary<TKey, string> dictionary, TKey key, string value)
        where TKey : notnull
    {
        dictionary.TryGetValue(key, out string? val);
        dictionary[key] = val + value;
    }
}