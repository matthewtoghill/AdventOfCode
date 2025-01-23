namespace AdventOfCode.Tools;

public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{
    private readonly Func<TValue> _defaultValueFactory;

    public DefaultDictionary()
    {
        _defaultValueFactory = () => default!;
    }

    public DefaultDictionary(TValue defaultValue)
    {
        _defaultValueFactory = () => defaultValue is ICloneable cloneable
            ? (TValue)cloneable.Clone()
            : defaultValue;
    }

    public DefaultDictionary(Func<TValue> defaultValueFactory)
    {
        _defaultValueFactory = defaultValueFactory ?? throw new ArgumentNullException(nameof(defaultValueFactory));
    }

    public new TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out var val))
                return val;

            val = _defaultValueFactory();
            Add(key, val);

            return val;
        }
        set => base[key] = value;
    }
}
