using System.Text;
using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2021;

internal class Day13() : Solver(2021, 13)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        var positions = LoadPositions(input[0].SplitLines());
        Part1Solution = Fold(positions, input[1].SplitLines()[0]).Count;
        Part2Solution = Part2(positions, input[1].SplitLines());
    }

    private static string Part2(HashSet<Position> positions, string[] instructions)
    {
        foreach (var line in instructions)
        {
            positions = Fold(positions, line);
        }

        return DrawPoints(positions).DecodeAsciiMessage();
    }

    private static HashSet<Position> Fold(HashSet<Position> positions, string instruction)
    {
        var newPositions = new HashSet<Position>();
        var split = instruction.Split(['=', ' '], StringSplitOptions.RemoveEmptyEntries);
        var axis = split[^2];
        var line = int.Parse(split[^1]);

        foreach (var pos in positions)
        {
            // New position calculated as the fold line less the gap between the position and the fold line
            var newX = axis == "x" && pos.X > line ? (line - (pos.X - line)) : pos.X;
            var newY = axis == "y" && pos.Y > line ? (line - (pos.Y - line)) : pos.Y;
            newPositions.Add(new Position(newX, newY));
        }

        return newPositions;
    }

    private static string DrawPoints(HashSet<Position> positions)
    {
        var sb = new StringBuilder();
        var maxX = positions.Max(p => p.X);
        var maxY = positions.Max(p => p.Y);

        for (int row = 0; row <= maxY; row++)
        {
            for (int col = 0; col <= maxX; col++)
            {
                sb.Append(positions.Contains(new(col,row)) ? '#' : ' ');
            }

            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static HashSet<Position> LoadPositions(string[] input)
        => input.Select(line => line.ExtractInts().ToArray())
                .Select(nums => new Position(nums[0], nums[1]))
                .ToHashSet();
}
