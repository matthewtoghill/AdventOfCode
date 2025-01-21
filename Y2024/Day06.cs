using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day06() : GridSolver(2024, 6)
{
    private record Guard(Position Position, char Direction);
    private static readonly Dictionary<char, char> TurnRight = new() { ['N'] = 'E', ['E'] = 'S', ['S'] = 'W', ['W'] = 'N' };

    public override void Run()
    {
        var input = Input.ReadAllLines();
        var (start, obstacles) = FindStartAndObstacles(input);
        var guard = new Guard(start, 'N');

        var route = Part1(guard, obstacles);
        Part1Solution = route.Count;
        Part2Solution = Part2(guard, obstacles, route);
    }

    private HashSet<Position> Part1(Guard guard, HashSet<Position> obstacles)
    {
        HashSet<Position> visited = [];

        while (true)
        {
            visited.Add(guard.Position);
            var next = guard.Position.MoveInDirection(guard.Direction);

            if (next.IsOutsideBounds(MinPos, MaxPos))
                break;

            guard = obstacles.Contains(next)
                ? new Guard(guard.Position, TurnRight[guard.Direction])
                : new Guard(next, guard.Direction);
        }

        return visited;
    }

    private int Part2(Guard start, HashSet<Position> obstacles, HashSet<Position> originalRoute)
        => originalRoute.AsParallel().Count(x => CheckLoop([.. obstacles, x], start));

    private bool CheckLoop(HashSet<Position> obstacles, Guard guard)
    {
        HashSet<Guard> corners = [];
        var lastDir = guard.Direction;

        while (true)
        {
            if (corners.Contains(guard))
                return true;

            if (lastDir != guard.Direction)
            {
                corners.Add(guard);
                lastDir = guard.Direction;
            }

            var next = guard.Position.MoveInDirection(guard.Direction);

            if (next.IsOutsideBounds(MinPos, MaxPos))
                break;

            guard = obstacles.Contains(next)
                ? new Guard(guard.Position, TurnRight[guard.Direction])
                : new Guard(next, guard.Direction);
        }

        return false;
    }

    private static (Position, HashSet<Position>) FindStartAndObstacles(string[] input)
    {
        var start = new Position(0, 0);
        HashSet<Position> obstacles = [];

        input.IterateGrid((row, col, c) =>
        {
            if (c == '^')
                start = new(col, row);
            else if (c == '#')
                obstacles.Add(new(col, row));
        });

        return (start, obstacles);
    }
}
