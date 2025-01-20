using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2023;

internal class Day11() : Solver(2023, 11)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 1);
        Part2Solution = Solve(input, 999_999);
    }

    private static long Solve(string[] input, int expandAmount)
    {
        var galaxies = input.IterateGrid((row, col, _) => new Position(col, row), c => c == '#').ToList();
        var emptyRows = Enumerable.Range(0, input.Length).Except(galaxies.Select(x => x.Row)).ToHashSet();
        var emptyCols = Enumerable.Range(0, input[0].Length).Except(galaxies.Select(x => x.Col)).ToHashSet();

        long totalDist = 0;

        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                totalDist += GetDistance(galaxies[i], galaxies[j], emptyRows, emptyCols, expandAmount);
            }
        }

        return totalDist;
    }

    private static long GetDistance(Position a, Position b, HashSet<int> rows, HashSet<int> cols, long expandAmount)
    {
        long distance = a.ManhattanDistance(b);

        for (int col = Math.Min(a.Col, b.Col); col < Math.Max(a.Col, b.Col); col++)
        {
            if (cols.Contains(col)) distance += expandAmount;
        }

        for (int row = Math.Min(a.Row, b.Row); row < Math.Max(a.Row, b.Row); row++)
        {
            if (rows.Contains(row)) distance += expandAmount;
        }

        return distance;
    }
}
