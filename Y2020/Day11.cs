using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2020;

internal class Day11() : Solver(2020, 11)
{
    private Position MinPos = new(0, 0);
    private Position MaxPos = new(0, 0);

    public override void Run()
    {
        var seats = Input.ReadAllLines().Select(x => x.ToCharArray()).ToArray();
        MaxPos = new Position(seats[0].Length - 1, seats.Length - 1);
        Part1Solution = CountOccupiedSeats(Stabilize(seats, 4, CountAdjacentOccupiedSeats));
        Part2Solution = CountOccupiedSeats(Stabilize(seats, 5, CountVisibleOccupiedSeats));
    }

    private static char[][] Stabilize(char[][] seats, int tolerance, Func<char[][], Position, int> countOccupied)
    {
        var rows = seats.Length;
        var cols = seats[0].Length;
        var newSeats = seats.Select(x => x.ToArray()).ToArray();
        var hasChanged = true;

        while (hasChanged)
        {
            hasChanged = false;
            var oldSeats = newSeats.Select(x => x.ToArray()).ToArray();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var pos = new Position(col, row);
                    if (oldSeats[row][col] == 'L' && countOccupied(oldSeats, pos) == 0)
                    {
                        newSeats[row][col] = '#';
                        hasChanged = true;
                    }
                    else if (oldSeats[row][col] == '#' && countOccupied(oldSeats, pos) >= tolerance)
                    {
                        newSeats[row][col] = 'L';
                        hasChanged = true;
                    }
                }
            }
        }
        return newSeats;
    }

    private static int CountOccupiedSeats(char[][] seats)
        => seats.Sum(x => x.Count(y => y == '#'));

    private int CountAdjacentOccupiedSeats(char[][] seats, Position position)
        => position.GetNeighbours(includeDiagonal: true)
                   .Where(p => p.IsBetween(MinPos, MaxPos))
                   .Count(p => seats[p.Row][p.Col] == '#');

    private int CountVisibleOccupiedSeats(char[][] seats, Position position)
        => Position.AllDirections
                   .Select(dir => FindFirstSeat(seats, position, (dir.X, dir.Y)))
                   .Count(seat => seat == '#');

    private char FindFirstSeat(char[][] seats, Position position, (int dx, int dy) direction)
    {
        while (true)
        {
            position += direction;
            if (position.IsOutsideBounds(MinPos, MaxPos))
                return '.';

            if (seats[position.Row][position.Col] != '.')
                return seats[position.Row][position.Col];
        }
    }
}
