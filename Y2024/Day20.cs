using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day20() : Solver(2024, 20)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 2);
        Part2Solution = Solve(input, 20);
    }

    private static int Solve(string[] input, int maxDuration)
    {
        var map = input.AsCharMap();
        var walls = map.WhereValues(x => x == '#').Keys.ToHashSet();
        var start = map.First(x => x.Value == 'S').Key;
        var end = map.First(x => x.Value == 'E').Key;

        var stepsFromEnd = StepsFromEnd(start, end, walls);

        return stepsFromEnd.Keys.AsParallel()
                                .SelectMany(x => StepsSavedWithCheats(stepsFromEnd, x, maxDuration))
                                .Count(x => x >= 100);
    }

    private static List<int> StepsSavedWithCheats(Dictionary<Position, int> stepsFromEnd, Position start, int maxDuration)
        => stepsFromEnd.Where(x => x.Key.ManhattanDistance(start) <= maxDuration)
                       .Select(x => CalculateStepsSaved(stepsFromEnd, start, x.Key)).ToList();

    private static int CalculateStepsSaved(Dictionary<Position, int> stepsFromEnd, Position start, Position next)
        => stepsFromEnd[next] - (stepsFromEnd[start] + next.ManhattanDistance(start));

    private static Dictionary<Position, int> StepsFromEnd(Position start, Position end, HashSet<Position> walls)
    {
        PriorityQueue<Position, int> queue = new([(end, 0)]);
        DefaultDictionary<Position, int> stepsFromEnd = new(defaultValue: int.MaxValue) { [end] = 0 };

        while (queue.TryDequeue(out var current, out var steps))
        {
            if (current == start) break;

            foreach (var next in current.GetNeighbours())
            {
                if (walls.Contains(next)) continue;

                if (stepsFromEnd[next] > steps + 1)
                {
                    stepsFromEnd[next] = steps + 1;
                    queue.Enqueue(next, steps + 1);
                }
            }
        }

        return stepsFromEnd;
    }
}
