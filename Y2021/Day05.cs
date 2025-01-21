using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2021;

internal class Day05() : Solver(2021, 5)
{
    private record Line(Position Start, Position End);

    public override void Run()
    {
        var lines = Input.ReadAllLines()
                         .SelectMany(x => x.ExtractInts())
                         .Chunk(4)
                         .Select(x => new Line(new(x[0], x[1]), new(x[2], x[3])));

        Part1Solution = DrawLines(lines, false).Count(x => x.Value > 1);
        Part2Solution = DrawLines(lines, true).Count(x => x.Value > 1);
    }

    private static Dictionary<Position, int> DrawLines(IEnumerable<Line> lines, bool includeDiagonals)
    {
        Dictionary<Position, int> points = [];

        foreach (var (start, end) in lines)
        {
            if (includeDiagonals && Math.Abs(start.X - end.X) == Math.Abs(start.Y - end.Y))
            {
                DrawDiagonalLine(points, start, end);
                continue;
            }

            if (start.X == end.X) DrawVerticalLine(points, start, end);
            if (start.Y == end.Y) DrawHorizontalLine(points, start, end);
        }
        return points;
    }

    private static void DrawVerticalLine(Dictionary<Position, int> points, Position start, Position end)
    {
        for (int y = Math.Min(start.Y, end.Y); y <= Math.Max(start.Y, end.Y); y++)
            points.IncrementAt(new(start.X, y));
    }

    private static void DrawHorizontalLine(Dictionary<Position, int> points, Position start, Position end)
    {
        for (int x = Math.Min(start.X, end.X); x <= Math.Max(start.X, end.X); x++)
            points.IncrementAt(new(x, start.Y));
    }

    private static void DrawDiagonalLine(Dictionary<Position, int> points, Position start, Position end)
    {
        var step = new Position(start.X < end.X ? 1 : -1, start.Y < end.Y ? 1 : -1);
        for (int i = 0; i <= Math.Abs(start.X - end.X); i++)
            points.IncrementAt(new(start.X + (i * step.X), start.Y + (i * step.Y)));
    }
}
