using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2020;

internal class Day12() : Solver(2020, 12)
{
    public override void Run()
    {
        var instructions = Input.ReadAllLines().Select(x => (x[0], int.Parse(x[1..]))).ToArray();
        Part1Solution = Part1(instructions, 90);
        Part2Solution = Part2(instructions);
    }

    private static int Part1((char, int)[] instructions, int degrees)
    {
        var ship = Position.Default;

        foreach (var (action, value) in instructions)
        {
            switch (action)
            {
                case 'N' or 'S' or 'E' or 'W': ship = ship.MoveInDirection(action, value); break;
                case 'L': degrees = (degrees - value).Mod(360); break;
                case 'R': degrees = (degrees + value).Mod(360); break;
                case 'F': ship = ship.MoveInDirection(DegreesToDirection(degrees), value); break;
            }
        }

        return ship.ManhattanDistance(new(0,0));
    }

    private static int Part2((char, int)[] instructions)
    {
        var ship = Position.Default;
        var waypoint = new Position(10, -1);

        foreach (var (action, value) in instructions)
        {
            switch (action)
            {
                case 'N' or 'S' or 'E' or 'W': waypoint = waypoint.MoveInDirection(action, value); break;
                case 'L' or 'R': waypoint = waypoint.Rotate(action, value); break;
                case 'F': ship += (waypoint.X * value, waypoint.Y * value); break;
            }
        }

        return ship.ManhattanDistance(new(0, 0));
    }

    private static char DegreesToDirection(int degrees)
        => degrees switch
        {
            0 => 'N',
            90 => 'E',
            180 => 'S',
            270 => 'W',
            _ => throw new InvalidOperationException()
        };
}
