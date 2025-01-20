using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;

internal class Day24() : Solver(2022, 24)
{
    public override void Run()
    {
        var blizzardStates = GetAllBlizzardStates(Input.ReadAllLines(), 1000);
        var part1 = Part1(blizzardStates);
        Part1Solution = part1;
        Part2Solution = Part2(blizzardStates, part1);
    }

    private static int Part1(List<HashSet<Position>> blizzardStates)
    {
        var start = new Position(1, 0);
        var goal = new Position(120, 26);
        var min = new Position(1, 1);
        var max = new Position(120, 25);

        return GetMinStepsToGoal(start, goal, 0, blizzardStates, min, max);
    }

    private static int Part2(List<HashSet<Position>> blizzardStates, int stepsThereOnce)
    {
        var start = new Position(1, 0);
        var goal = new Position(120, 26);
        var min = new Position(1, 1);
        var max = new Position(120, 25);

        var getBackOnce = GetMinStepsToGoal(goal, start, stepsThereOnce, blizzardStates, min, max);
        return GetMinStepsToGoal(start, goal, getBackOnce, blizzardStates, min, max);
    }


    private static List<HashSet<Position>> GetAllBlizzardStates(string[] input, int maxStates)
    {
        // Generate the blizzard locations in advance to avoid the need to generate the same state multiple times later
        // TODO: consider refactoring as either:
        //       A) pre-determine a reasonable best estimate of the minimum required steps to reach the goal, we exit
        //          out of the BFS when the goal is reached so we only need to calculate until the first time point gets there
        //       B) keep a list of blizzard states, we only ADD to the list of states when a new time point is reached for the first time
        //          if another step needs to check the same time point then it can access it from the blizzardStates list already

        var blizzard = LoadBlizzard(input);
        var minX = blizzard.Min(b => b.Position.X);
        var maxX = blizzard.Max(b => b.Position.X);
        var minY = blizzard.Min(b => b.Position.Y);
        var maxY = blizzard.Max(b => b.Position.Y);

        var blizzardStatesTemp = new List<HashSet<(Position Position, char Direction)>> { blizzard };
        for (int i = 0; i < maxStates; i++)
        {
            blizzardStatesTemp.Add(GetNextBlizzard(blizzardStatesTemp[i], minX, maxX, minY, maxY));
        }

        // Convert to just positions
        var blizzardStates = new List<HashSet<Position>>();
        foreach (var item in blizzardStatesTemp)
        {
            blizzardStates.Add(item.Select(x => x.Position).ToHashSet());
        }

        return blizzardStates;
    }

    private static int GetMinStepsToGoal(Position start, Position goal, int initialTime, List<HashSet<Position>> blizzardStates, Position min, Position max)
    {
        var queue = new Queue<(Position Location, int Time)>([(start, initialTime)]);
        var timeStates = new HashSet<(Position Location, int Time)>();

        while (queue.TryDequeue(out var current))
        {
            // skip if the state has already been found
            if (timeStates.Contains(current)) continue;

            timeStates.Add(current);

            foreach (var n in current.Location.GetNeighbours())
            {
                if (n == goal)
                    return current.Time + 1;

                // skip any locations that are out of bounds
                if (n.IsOutsideBounds(min, max)) continue;

                // add the next space if not blocked by a blizzard at the next time point
                if (!blizzardStates[current.Time + 1].Contains(n))
                    queue.Enqueue((n, current.Time + 1));
            }

            // add the current space if it's not blocked by a blizzard at the next time point
            if (!blizzardStates[current.Time + 1].Contains(current.Location))
                queue.Enqueue((current.Location, current.Time + 1));
        }

        return 0;
    }

    private static HashSet<(Position Position, char Direction)> LoadBlizzard(string[] input)
        => input.IterateGrid((row, col, c) => (new Position(col, row), c), c => c is '<' or '>' or '^' or 'v')
                .ToHashSet();

    private static HashSet<(Position, char)> GetNextBlizzard(HashSet<(Position Position, char Direction)> blizzard, int minX, int maxX, int minY, int maxY)
    {
        var nextBlizzard = new HashSet<(Position Position, char Direction)>();
        foreach (var b in blizzard)
        {
            var nextB = b.Position.MoveInDirection(b.Direction);

            if (b.Direction == '<' && nextB.X < minX) nextBlizzard.Add((new(maxX, nextB.Y), b.Direction));
            else if (b.Direction == '>' && nextB.X > maxX) nextBlizzard.Add((new(minX, nextB.Y), b.Direction));
            else if (b.Direction == '^' && nextB.Y < minY) nextBlizzard.Add((new(nextB.X, maxY), b.Direction));
            else if (b.Direction == 'v' && nextB.Y > maxY) nextBlizzard.Add((new(nextB.X, minY), b.Direction));
            else nextBlizzard.Add((nextB, b.Direction));
        }
        return nextBlizzard;
    }
}
