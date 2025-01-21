using System.Numerics;

namespace AdventOfCode.Tools;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (T item in items)
            action(item);
    }

    public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
    {
        var index = 0;
        foreach (T item in items)
            action(item, index++);
    }

    public static IEnumerable<bool> CompareAll<T>(this IEnumerable<T> items, Func<T, T, bool> predicate)
    {
        var itemArray = items.ToArray();
        for (int i = 0; i < itemArray.Length; i++)
            for (int j = i + 1; j < itemArray.Length; j++)
                yield return predicate(itemArray[i], itemArray[j]);
    }

    public static IEnumerable<T> ApplyEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (T item in items)
        {
            action(item);
            yield return item;
        }
    }

    public static IEnumerable<T> ApplyEach<T>(this IEnumerable<T> items, Action<T, int> action)
    {
        var index = 0;
        foreach (T item in items)
        {
            action(item, index++);
            yield return item;
        }
    }

    public static IEnumerable<T> Without<T>(this IEnumerable<T> items, T value)
        => items.Where(x => !Equals(x, value));

    public static Dictionary<T, int> GetFrequencies<T>(this IEnumerable<T> items) where T : notnull
        => items.CountBy(x => x).ToDictionary();

    public static Dictionary<T, long> GetFrequenciesAsLong<T>(this IEnumerable<T> items) where T : notnull
        => items.CountBy(x => x).ToDictionary(x => x.Key, x => (long)x.Value);

    public static int Sum<T>(this IEnumerable<T> items, Func<T, int, int> selector)
        => items.Select(selector).Sum();

    public static long Sum<T>(this IEnumerable<T> items, Func<T, int, long> selector)
        => items.Select(selector).Sum();

    public static T Product<T>(this IEnumerable<T> items) where T : INumber<T>
        => items.Aggregate((total, x) => total * x);

    public static TOut Product<T, TOut>(this IEnumerable<T> items, Func<T, TOut> selector) where TOut : INumber<TOut>
        => items.Select(selector).Product();

    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> items)
        => items.SelectMany(x => x);

    public static bool IsAnyOf<T>(this T item, params T[] items)
        => items.Contains(item);

    public static IEnumerable<T> EnumerateDifferences<T>(this IEnumerable<T> items) where T : INumber<T>
        => items.Zip(items.Skip(1)).Select(x => x.Second - x.First);

    public static T RandomElement<T>(this IEnumerable<T> items)
        => items.ElementAt(Random.Shared.Next(items.Count()));


    public static IEnumerable<IEnumerable<T>> GetSubsetVariations<T>(this IEnumerable<T> items, int excludeCount)
        => Enumerable.Range(0, 1 << items.Count())
                     .Where(mask => BitOperations.PopCount((uint)mask) == items.Count() - excludeCount)
                     .Select(mask => items.Where((_, index) => (mask & (1 << index)) != 0));

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list)
    {
        if (list.Count() > 1)
        {
            return list.SelectMany(
                 item => GetPermutations(list.Where(i => !i.Equals(item))),
                 (item, permutation) => new[] { item }.Concat(permutation));
        }

        return [list];
    }

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });

        return list.GetPermutations(length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat([t2]));
    }

    public static IEnumerable<IEnumerable<T>> GetPowerSet<T>(this List<T> list)
    {
        return from m in Enumerable.Range(0, 1 << list.Count)
               select
                    from i in Enumerable.Range(0, list.Count)
                    where (m & (1 << i)) != 0
                    select list[i];
    }

    public static IEnumerable<IEnumerable<T>> GeneratePermutations<T>(this IEnumerable<T> items, int quantity)
    {
        if (quantity == 1)
            return items.Select(item => new[] { item });

        return items.GeneratePermutations(quantity - 1)
                    .SelectMany(x => items, (x, item) => x.Append(item));
    }
}