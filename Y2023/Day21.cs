using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2023;

internal class Day21() : GridSolver(2023, 21)
{
    private record State(Position Position, int Steps);

    public override void Run()
    {
        Part1Solution = Solve(64);
        Part2Solution = Part2();
    }

    private long Part2()
    {
        var maxSteps = 26501365;
        var gridSize = Grid.Length;
        var halfLength = gridSize / 2;

        var steps = Enumerable.Range(0, 3).Select(x => (double)Solve((x * gridSize) + halfLength)).ToList();
        var diffs = steps.EnumerateDifferences().ToList();

        double x = maxSteps / gridSize;
        return (long)(steps[0] + (diffs[0] * x) + (x * (x - 1) / 2 * (diffs[1] - diffs[0])));
    }

    private int Solve(int maxSteps)
    {
        var start = GridMap.Single(x => x.Value == 'S').Key;
        Queue<State> queue = new([new State(start, 0)]);
        HashSet<State> visited = [];
        var gridSize = Grid.Length;

        while (queue.TryDequeue(out var current))
        {
            if (!visited.Add(new(current.Position, current.Steps % 2)) || current.Steps == maxSteps)
                continue;

            foreach (var neighbour in current.Position.GetNeighbours())
            {
                if (Grid[neighbour.Row.Mod(gridSize)][neighbour.Col.Mod(gridSize)] != '#')
                    queue.Enqueue(new State(neighbour, current.Steps + 1));
            }
        }

        return visited.Count(x => x.Steps == (maxSteps % 2));
    }
}
