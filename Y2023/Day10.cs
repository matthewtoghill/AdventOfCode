using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2023;

internal class Day10() : Solver(2023, 10)
{
    private record Step(Position Pos, char Pipe, char Direction);
    public override void Run()
    {
        var input = Input.ReadAllLines();
        var loop = GetLoop(input);
        Part1Solution = loop.Count / 2;
        Part2Solution = Part2(loop);
    }

    private static List<Step> GetLoop(string[] input)
    {
        var start = FindStart(input);

        foreach (var (startPipe, startDir) in "|-LJ7F|-LJ7F".Zip("NWNNWSSESSEN"))
        {
            var path = new List<Step>();
            var current = new Step(start, startPipe, startDir);
            path.Add(current);

            while (true)
            {
                var nextPos = current.Pos.MoveInDirection(current.Direction);

                if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X >= input[nextPos.Y].Length || nextPos.Y >= input.Length)
                    continue;

                if (nextPos == start)
                    return path;

                if (!IsValid(input, current.Direction, nextPos))
                    break;

                var nextChar = input[nextPos.Y][nextPos.X];
                var nextDir = GetNextDirection(nextChar, current.Direction);

                current = new(nextPos, nextChar, nextDir);
                path.Add(current);
            }
        }

        return [];
    }

    private static Position FindStart(string[] input)
    {
        for (int row = 0; row < input.Length; row++)
            if (input[row].Contains('S'))
                return new Position(input[row].IndexOf('S'), row);

        return new Position(-1, -1);
    }

    private static char GetNextDirection(char pipe, char startDirection)
        => (pipe, startDirection) switch
        {
            ('|', 'N') or ('L', 'W') or ('J', 'E') => 'N',
            ('|', 'S') or ('7', 'E') or ('F', 'W') => 'S',
            ('-', 'E') or ('L', 'S') or ('F', 'N') => 'E',
            ('-', 'W') or ('J', 'S') or ('7', 'N') => 'W',
            _ => '?'
        };

    private static bool IsValid(string[] input, char direction, Position next)
        => (direction, input[next.Y][next.X]) switch
        {
            ('N', '7') or ('N', 'F') or ('N', '|') => true,
            ('S', 'L') or ('S', 'J') or ('S', '|') => true,
            ('E', 'J') or ('E', '7') or ('E', '-') => true,
            ('W', 'L') or ('W', 'F') or ('W', '-') => true,
            _ => false
        };

    private static int Part2(List<Step> loop)
    {
        var allPos = loop.Select(x => x.Pos).ToList();
        allPos.Add(loop[0].Pos);

        return allPos.CountPositionsInsideArea();
    }
}
