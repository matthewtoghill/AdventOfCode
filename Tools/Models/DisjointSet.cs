namespace AdventOfCode.Tools.Models;

public class DisjointSet<T> where T : notnull
{
    private readonly Dictionary<T, T> parent = [];
    private readonly Dictionary<T, int> size = [];
    public int ComponentCount { get; private set; }

    public DisjointSet(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public bool Contains(T item) => parent.ContainsKey(item);

    public void Add(T item)
    {
        if (parent.ContainsKey(item))
            return;

        parent[item] = item;
        size[item] = 1;
        ComponentCount++;
    }

    public bool TryFind(T x, out T root)
    {
        if (!parent.TryGetValue(x, out var p))
        {
            root = default!;
            return false;
        }

        // Path compression
        if (!p.Equals(x))
        {
            root = Find(p);
            parent[x] = root;
            return true;
        }

        root = x;
        return true;
    }

    public T Find(T x)
    {
        if (!parent.TryGetValue(x, out var p))
            throw new KeyNotFoundException($"Item not found in DisjointSet: {x}");

        if (!p.Equals(x))
        {
            parent[x] = Find(p);
        }

        return parent[x];
    }

    public bool Union(T a, T b)
    {
        // Ensure items exist
        if (!Contains(a)) Add(a);
        if (!Contains(b)) Add(b);

        var rootA = Find(a);
        var rootB = Find(b);

        if (EqualityComparer<T>.Default.Equals(rootA, rootB))
            return false;

        // Union by size (attach smaller tree under larger)
        var sizeA = size[rootA];
        var sizeB = size[rootB];

        if (sizeA < sizeB)
        {
            parent[rootA] = rootB;
            size[rootB] += sizeA;
        }
        else
        {
            parent[rootB] = rootA;
            size[rootA] += sizeB;
        }

        ComponentCount--;
        return true;
    }

    public int ComponentSize(T x)
    {
        var root = Find(x);
        return size[root];
    }

    public IEnumerable<HashSet<T>> GetComponents()
    {
        var groups = new Dictionary<T, HashSet<T>>();
        foreach (var item in parent.Keys)
        {
            var root = Find(item);
            if (!groups.TryGetValue(root, out var set))
            {
                set = [];
                groups[root] = set;
            }
            set.Add(item);
        }
        return groups.Values;
    }
}
