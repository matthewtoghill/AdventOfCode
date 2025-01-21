using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);
        if (exists)
            return val!;

        val = value;
        return value;
    }

    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> func) where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);
        if (!exists)
            val = func(key);

        return val!;
    }

    public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dictionary, key);
        if (Unsafe.IsNullRef(ref val))
            return false;

        val = value;
        return true;
    }

    public static Dictionary<TKey, TValue> WhereKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<TKey> predicate) where TKey : notnull
    {
        var newDictionary = new Dictionary<TKey, TValue>();
        foreach (var (key, value) in dictionary)
        {
            if (predicate(key))
                newDictionary[key] = value;
        }
        return newDictionary;
    }

    public static Dictionary<TKey, TValue> WhereValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<TValue> predicate) where TKey : notnull
    {
        var newDictionary = new Dictionary<TKey, TValue>();
        foreach (var (key, value) in dictionary)
        {
            if (predicate(value))
                newDictionary[key] = value;
        }
        return newDictionary;
    }
}