using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2025;

internal class Day09() : Solver(2025, 9)
{
    override public void Run()
    {
        var input = ParseInput(Input.ReadAllLines());
        Part1Solution = input.SelectAllPairs((a, b) => a.CalculateAreaLong(b)).Max();
        Part2Solution = Part2(input);
    }

    private static long Part2(List<Position> input)
    {
        var maxArea = 0L;

        var lines = Enumerable.Range(0, input.Count - 1)
                              .Select(i => new Line(input[i], input[i + 1]))
                              .Concat([new Line(input[^1], input[0])])
                              .ToArray();

        input.IterateAllPairs((a, b) =>
        {
            var area = a.CalculateAreaLong(b);
            if (area > maxArea && !lines.Any(line => line.Intersects(a, b)))
                maxArea = area;
        });

        return maxArea;
    }

    private static List<Position> ParseInput(string[] input)
    {
        var result = new List<Position>();
        foreach (var line in input)
        {
            var points = line.ExtractInts().ToList();
            result.Add(new(points[0], points[1]));
        }
        return result;
    }
}
