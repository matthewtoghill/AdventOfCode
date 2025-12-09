using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2025;

internal class Day08() : Solver(2025, 8)
{
    public override void Run()
    {
        var input = ParseInput(Input.ReadAllLines());
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(List<Position3D> input)
    {
        var edges = GetSortedBoxDistances(input);
        var set = new DisjointSet<Position3D>(input);

        for (int i = 0; i < 1000; i++)
        {
            set.Union(edges[i].BoxA, edges[i].BoxB);
        }

        return set.GetComponents().Select(x => x.Count).OrderDescending().Take(3).Product();
    }

    private static int Part2(List<Position3D> input)
    {
        var edges = GetSortedBoxDistances(input);
        var set = new DisjointSet<Position3D>(input);

        foreach (var (a, b, _) in edges)
        {
            if (set.Find(a) != set.Find(b))
            {
                set.Union(a, b);

                if (set.ComponentCount == 1)
                    return a.X * b.X;
            }
        }

        return 0;
    }

    private static List<(Position3D BoxA, Position3D BoxB, double Distance)> GetSortedBoxDistances(List<Position3D> input)
        => input.SelectAllPairs((a, b) => (a, b, a.DirectDistance(b)))
                .Select(x => (BoxA: x.a, BoxB: x.b, Distance: x.Item3))
                .OrderBy(x => x.Distance)
                .ToList();

    private static List<Position3D> ParseInput(string[] input)
    {
        var result = new List<Position3D>();
        foreach (var line in input)
        {
            var points = line.ExtractInts().ToList();
            result.Add(new(points[0], points[1], points[2]));
        }

        return result;
    }
}
