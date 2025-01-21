using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day18() : Solver(2024, 18)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        var corruptedBytes = GetCorruptedBytes(input).ToList();
        Part1Solution = Part1(corruptedBytes);
        Part2Solution = Part2(input, corruptedBytes);
    }

    private static int Part1(List<Position> corruptedBytes)
        => TryReachExit(corruptedBytes.Take(1024).ToHashSet(), out var minSteps) ? minSteps : 0;

    private static string Part2(string[] input, List<Position> corruptedBytes)
        => input[BinarySearch(0, input.Length - 1, x => !TryReachExit(corruptedBytes.Take(x).ToHashSet(), out _))];

    private static bool TryReachExit(HashSet<Position> corruptedBytes, out int minSteps)
    {
        var start = new Position(0, 0);
        var end = new Position(70, 70);

        PriorityQueue<Position, int> queue = new([(start, 0)]);
        DefaultDictionary<Position, int> positionMinSteps = new(defaultValue: int.MaxValue);

        minSteps = 0;
        bool canExit = false;

        while (queue.TryDequeue(out var current, out var steps))
        {
            if (current == end)
            {
                canExit = true;
                minSteps = positionMinSteps[end];
                continue;
            }

            foreach (var neighbour in current.GetNeighbours())
            {
                if (corruptedBytes.Contains(neighbour)) continue;
                if (!neighbour.IsBetween(start, end)) continue;

                if (positionMinSteps[neighbour] > steps + 1)
                {
                    positionMinSteps[neighbour] = steps + 1;
                    queue.Enqueue(neighbour, steps + 1);
                }
            }
        }

        return canExit;
    }

    private static int BinarySearch(int min, int max, Func<int, bool> predicate)
    {
        while (min < max)
        {
            int mid = min + ((max - min) / 2);
            if (predicate(mid))
                max = mid;
            else
                min = mid + 1;
        }

        return min - 1;
    }

    private static IEnumerable<Position> GetCorruptedBytes(string[] input)
        => input.Select(x => x.ExtractInts().ToList()).Select(x => new Position(x[0], x[1]));
}
