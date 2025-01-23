using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2021;

internal class Day11() : Solver(2021, 11)
{
    public override void Run()
    {
        (Part1Solution, Part2Solution) = Solve(Input.ReadAllLines());
    }

    private static (int, int) Solve(string[] input)
    {
        var grid = input.AsIntMap();
        List<int> steps = [];

        while (true)
        {
            grid.Keys.ForEach(x => grid.IncrementAt(x)); // increase energy level of every octopus by 1

            Queue<Position> queue = [];
            HashSet<Position> flashed = [];

            grid.WhereValues(x => x > 9).Keys.ForEach(queue.Enqueue); // add octopuses with energy level over 9 to queue

            while (queue.TryDequeue(out var current))
            {
                if (!flashed.Add(current)) continue;

                foreach (var next in current.GetNeighbours(true))
                {
                    if (!grid.ContainsKey(next)) continue;
                    grid.IncrementAt(next);

                    if (grid[next] > 9)
                        queue.Enqueue(next);
                }
            }

            steps.Add(flashed.Count);
            flashed.ForEach(x => grid[x] = 0); // reset energy level of flashed octopuses

            if (flashed.Count == grid.Count) // when all octopuses flashed on the same step
                break;
        }

        return (steps.Take(100).Sum(), steps.Count);
    }
}
