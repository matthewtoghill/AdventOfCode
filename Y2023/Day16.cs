using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2023;

internal class Day16() : GridSolver(2023, 16)
{
    private record Photon(Position Position, char Direction);

    public override void Run()
    {
        Part1Solution = CountEnergized(0, -1, 'E');
        Part2Solution = Part2();
    }

    private int Part2()
    {
        var energizedCounts = new List<int>();

        // down the rows, top to bottom, start on left and right edges
        for (int row = 0; row < Grid.Length; row++)
        {
            energizedCounts.Add(CountEnergized(row, -1, 'E'));
            energizedCounts.Add(CountEnergized(row, Grid.Length, 'W'));
        }

        // along the cols, left to right, start on top and bottom edges
        for (int col = 0; col < Grid[0].Length; col++)
        {
            energizedCounts.Add(CountEnergized(0, col, 'S'));
            energizedCounts.Add(CountEnergized(Grid[0].Length, col, 'N'));
        }

        return energizedCounts.Max();
    }

    private int CountEnergized(int startRow, int startCol, char direction)
    {
        var start = new Photon(new Position(startCol, startRow), direction);
        var queue = new Queue<Photon>([start]);
        var visited = new HashSet<Photon>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var nextPos = current.Position.MoveInDirection(current.Direction);

            if (nextPos.IsOutsideBounds(new(0, 0), MaxPos))
                continue;

            var nextDirections = GetNextDirections(GridMap[nextPos], current.Direction);
            foreach (var dir in nextDirections)
            {
                var nextPhoton = new Photon(nextPos, dir);
                if (visited.Add(nextPhoton))
                    queue.Enqueue(nextPhoton);
            }
        }

        return visited.DistinctBy(x => x.Position).Count();
    }

    private static char[] GetNextDirections(char tile, char direction)
        => (tile, direction) switch
        {
            ('.', _) or ('|', 'N') or ('|', 'S') or ('-', 'E') or ('-', 'W') => [direction],
            ('|', 'E') or ('|', 'W') => ['N', 'S'],
            ('-', 'N') or ('-', 'S') => ['E', 'W'],
            ('\\', 'N') or ('/', 'S') => ['W'],
            ('\\', 'S') or ('/', 'N') => ['E'],
            ('\\', 'E') or ('/', 'W') => ['S'],
            ('\\', 'W') or ('/', 'E') => ['N'],
            _ => throw new NotSupportedException()
        };

}
