using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2025;

internal class Day07() : GridSolver(2025, 7)
{
    public override void Run()
    {
        var start = FindStart();
        Part1Solution = Part1(start);
        Part2Solution = Part2(start);
    }

    private int Part1(Position start)
    {
        var visited = new HashSet<Position> { start };
        var splitters = new HashSet<Position>();
        var queue = new Queue<Position>();
        queue.Enqueue(start);

        while (queue.TryDequeue(out var current))
        {
            if (!GridMap.ContainsKey(current))
                continue;

            visited.Add(current);

            if (GridMap[current] == '^')
            {
                if (splitters.Add(current))
                {
                    queue.Enqueue(current.MoveInDirection('E'));
                    queue.Enqueue(current.MoveInDirection('W'));
                }

                continue;
            }

            queue.Enqueue(current.MoveInDirection('S'));
        }

        return splitters.Count;
    }

    private long Part2(Position start)
    {
        var queue = new Queue<(Position, long)>();
        queue.Enqueue((start, 1));

        var result = 0L;
        var pending = new Dictionary<Position, long>();

        while (queue.Count > 0)
        {
            var (pos, count) = queue.Dequeue();

            if (!GridMap.ContainsKey(pos))
            {
                result += count;
                continue;
            }

            if (GridMap[pos] == '^')
            {
                EnqueueOrAccumulate(pending, pos.MoveInDirection('W'), count);
                EnqueueOrAccumulate(pending, pos.MoveInDirection('E'), count);
            }
            else
            {
                EnqueueOrAccumulate(pending, pos.MoveInDirection('S'), count);
            }

            if (queue.Count == 0 && pending.Count > 0)
            {
                queue.EnqueueRange(pending.Select(kvp => (kvp.Key, kvp.Value)));

                pending.Clear();
            }
        }

        return result;
    }

    private static void EnqueueOrAccumulate(Dictionary<Position, long> dict, Position pos, long add)
        => dict[pos] = dict.TryGetValue(pos, out var existing) ? existing + add : add;

    private Position FindStart()
        => GridMap.WhereValues(x => x == 'S').Keys.First();
}
