namespace AdventOfCode.Tools;

internal class DefaultDictionary<TKey, TValue>(TValue defaultValue = default!) : Dictionary<TKey, TValue> where TKey : notnull
{
    public TValue DefaultValue { get; } = defaultValue;

    public new TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out var val))
                return val;

            val = DefaultValue;
            Add(key, val);

            return val;
        }
        set => base[key] = value;
    }
}
