using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2025;

internal class Day04() : GridSolver(2025, 4)
{
    public override void Run()
    {
        Part1Solution = Solve(GridMap.ToDictionary(), false);
        Part2Solution = Solve(GridMap.ToDictionary(), true);
    }

    private static int Solve(Dictionary<Position, char> grid, bool removeAllPossible)
    {
        var result = 0;

        do
        {
            var removedAny = false;
            foreach (var (position, c) in grid)
            {
                if (c != '@')
                    continue;

                var countAdjacent = position.GetNeighbours(true).Count(n => grid.TryGetValue(n, out var val) && val == '@');
                if (countAdjacent < 4)
                {
                    removedAny = true;
                    grid[position] = '.';
                    result++;
                }
            }

            if (!removedAny)
                break;

        } while (removeAllPossible);

        return result;
    }
}
