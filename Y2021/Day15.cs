using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2021;

internal class Day15() : Solver(2021, 15)
{
    public override void Run()
    {
        var grid = Input.ReadAsGrid<int>();
        Part1Solution = Solve(grid);
        Part2Solution = Solve(ScaleGrid(grid, 5));
    }

    private static int Solve(int[,] grid)
    {
        var start = new Position(0, 0);
        var goal = new Position(grid.GetLength(1) - 1, grid.GetLength(0) - 1);

        PriorityQueue<Position, int> queue = new([(start, 0)]);
        Dictionary<Position, int> costs = new() { [start] = 0 };

        while (queue.TryDequeue(out var current, out _))
        {
            if (current == goal) break;

            foreach (var next in current.GetNeighbours())
            {
                if (next.IsOutsideBounds(start, goal)) continue;
                var cost = costs[current] + grid[next.Col, next.Row];

                if (cost < costs.GetValueOrDefault(next, int.MaxValue))
                {
                    costs[next] = cost;
                    queue.Enqueue(next, cost);
                }
            }
        }

        return costs[goal];
    }

    private static int[,] ScaleGrid(int[,] grid, int scale)
    {
        var gridRows = grid.GetLength(0);
        var gridCols = grid.GetLength(1);
        var rows = gridRows * scale;
        var cols = gridCols * scale;
        var newGrid = new int[rows, cols];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                var row = y % gridRows;
                var col = x % gridCols;
                var distance = (y / gridRows) + (x / gridCols);
                newGrid[x, y] = ((grid[col, row] + distance - 1) % 9) + 1; // new risk value
            }
        }

        return newGrid;
    }
}
