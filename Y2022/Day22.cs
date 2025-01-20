using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;

internal class Day22() : Solver(2022, 22)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs().ToArray();
        var map = LoadMap(input[0]);
        var instructions = input[1].Trim();

        Part1Solution = Part1(map, instructions);
        Part2Solution = Part2(map, instructions);
    }

    private static int Part1(Dictionary<Position, char> map, string instructions)
    {
        var direction = 'r';
        var location = map.First().Key;
        var stepsVal = "";

        for (int i = 0; i < instructions.Length; i++)
        {
            var instructionVal = instructions[i];
            if (char.IsDigit(instructionVal))
            {
                stepsVal += instructionVal;
                continue;
            }

            location = MoveInDirection_FlatMap(map, location, direction, int.Parse(stepsVal));
            direction = ChangeDirection(direction, instructionVal);
            stepsVal = "";
        }

        location = MoveInDirection_FlatMap(map, location, direction, int.Parse(stepsVal));

        return (1000 * location.Y) + (4 * location.X) + ScoreDirection(direction);
    }

    private static int Part2(Dictionary<Position, char> map, string instructions)
    {
        var direction = 'r';
        var location = map.First().Key;
        var stepsVal = "";

        for (int i = 0; i < instructions.Length; i++)
        {
            var instructionVal = instructions[i];
            if (char.IsDigit(instructionVal))
            {
                stepsVal += instructionVal;
                continue;
            }

            (location, direction) = MoveInDirection_CubeMap(map, location, direction, int.Parse(stepsVal));
            direction = ChangeDirection(direction, instructionVal);
            stepsVal = "";
        }

        (location, direction) = MoveInDirection_CubeMap(map, location, direction, int.Parse(stepsVal));

        return (1000 * location.Y) + (4 * location.X) + ScoreDirection(direction);
    }

    private static Position MoveInDirection_FlatMap(Dictionary<Position, char> map, Position location, char direction, int steps)
    {
        for (int s = 0; s < steps; s++)
        {
            location = location.MoveInDirection(direction);
            if (map.ContainsKey(location))
            {
                if (map[location] == '#')
                {
                    location = location.MoveInOppositeDirection(direction); // go back 1 step
                    break;
                }
            }
            else // wrap around the map in the current direction
            {
                var wrapLocation = location;
                switch (direction)
                {
                    case 'r': wrapLocation = map.Keys.Where(p => p.Y == location.Y).MinBy(p => p.X); break;
                    case 'l': wrapLocation = map.Keys.Where(p => p.Y == location.Y).MaxBy(p => p.X); break;
                    case 'u': wrapLocation = map.Keys.Where(p => p.X == location.X).MaxBy(p => p.Y); break;
                    case 'd': wrapLocation = map.Keys.Where(p => p.X == location.X).MinBy(p => p.Y); break;
                }

                if (map[wrapLocation] == '#')
                {
                    location = location.MoveInOppositeDirection(direction);
                    break;
                }

                location = wrapLocation;
            }
        }

        return location;
    }

    private static (Position, char) MoveInDirection_CubeMap(Dictionary<Position, char> map, Position location, char direction, int steps)
    {
        for (int s = 0; s < steps; s++)
        {
            var (X, Y) = location;
            var originalDirection = direction;

            // Logic based on 50x50x50 Cube with mesh and sides labeled as:
            //   A B
            //   C
            // D E
            // F

            // A up to F
            if (X.IsBetween(51, 100) && Y == 1 && direction == 'u')
            {
                location = new(1, X + 100);
                direction = 'r';
            }
            // A left to D
            else if (X == 51 && Y.IsBetween(1, 50) && direction == 'l')
            {
                location = new(1, 151 - Y);
                direction = 'r';
            }
            // B up to F
            else if (X.IsBetween(101, 150) && Y == 1 && direction == 'u')
            {
                location = new(X - 100, 200);
            }
            // B right to E
            else if (X == 150 && Y.IsBetween(1, 50) && direction == 'r')
            {
                location = new(100, 151 - Y);
                direction = 'l';
            }
            // B down to C
            else if (X.IsBetween(101, 150) && Y == 50 && direction == 'd')
            {
                location = new(100, X - 50);
                direction = 'l';
            }
            // C right to B
            else if (X == 100 && Y.IsBetween(51, 100) && direction == 'r')
            {
                location = new(Y + 50, 50);
                direction = 'u';
            }
            // C left to D
            else if (X == 51 && Y.IsBetween(51, 100) && direction == 'l')
            {
                location = new(Y - 50, 101);
                direction = 'd';
            }
            // D up to C
            else if (X.IsBetween(1, 50) && Y == 101 && direction == 'u')
            {
                location = new(51, X + 50);
                direction = 'r';
            }
            // D left to A
            else if (X == 1 && Y.IsBetween(101, 150) && direction == 'l')
            {
                location = new(51, 151 - Y);
                direction = 'r';
            }
            // E right to B
            else if (X == 100 && Y.IsBetween(101, 150) && direction == 'r')
            {
                location = new(150, 151 - Y);
                direction = 'l';
            }
            // E down to F
            else if (X.IsBetween(51, 100) && Y == 150 && direction == 'd')
            {
                location = new(50, X + 100);
                direction = 'l';
            }
            // F right to E
            else if (X == 50 && Y.IsBetween(151, 200) && direction == 'r')
            {
                location = new(Y - 100, 150);
                direction = 'u';
            }
            // F down to B
            else if (X.IsBetween(1, 50) && Y == 200 && direction == 'd')
            {
                location = new(X + 100, 1);
            }
            // F left to A
            else if (X == 1 && Y.IsBetween(151, 200) && direction == 'l')
            {
                location = new(Y - 100, 1);
                direction = 'd';
            }
            else // moving within the current face of the cube or moving to next face doesn't change direction
            {
                location = location.MoveInDirection(direction);
            }

            // if the new location is a wall, then return to the original location and direction values
            if (map.TryGetValue(location, out char value) && value == '#')
            {
                location = new(X, Y);
                direction = originalDirection;
                break;
            }
        }

        return (location, direction);
    }

    private static int ScoreDirection(char direction)
        => direction switch { 'd' => 1, 'l' => 2, 'u' => 3, _ => 0 };

    private static char ChangeDirection(char currentDirection, char rotate)
        => (currentDirection, rotate) switch
        {
            ('l', 'R') or ('r', 'L') => 'u',
            ('l', 'L') or ('r', 'R') => 'd',
            ('u', 'R') or ('d', 'L') => 'r',
            ('u', 'L') or ('d', 'R') => 'l',
            _ => throw new Exception()
        };

    private static Dictionary<Position, char> LoadMap(string mapData)
    {
        var mapLines = mapData.Split('\n');
        var map = new Dictionary<Position, char>();

        for (int y = 0; y < mapLines.Length; y++)
        {
            for (int x = 0; x < mapLines[y].Length; x++)
            {
                switch (mapLines[y][x])
                {
                    case '.':
                    case '#':
                        map.Add(new Position(x + 1, y + 1), mapLines[y][x]);
                        break;
                }
            }
        }

        return map;
    }
}
