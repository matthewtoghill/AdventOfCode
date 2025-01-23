using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day10() : Solver(2024, 10)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        var trailheads = FindTrailheads(input);
        Part1Solution = trailheads.Sum(x => x.Value.ToHashSet().Count);
        Part2Solution = trailheads.Sum(x => x.Value.Count);
    }

    private static Dictionary<Position, List<Position>> FindTrailheads(string[] input)
    {
        DefaultDictionary<Position, List<Position>> trailheads = new(() => []);
        var map = input.AsCharMap();

        foreach (var (start, _) in map.Where(x => x.Value == '0'))
        {
            Queue<Position> queue = new([start]);

            while (queue.TryDequeue(out var current))
            {
                var height = map[current];
                if (height == '9')
                {
                    trailheads[start].Add(current);
                    continue;
                }

                foreach (var neighbour in current.GetNeighbours())
                {
                    if (map.GetValueOrDefault(neighbour) == height + 1)
                        queue.Enqueue(neighbour);
                }
            }
        }

        return trailheads;
    }
}
