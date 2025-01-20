using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;

internal class Day12() : GridSolver(2022, 12)
{
    public override void Run()
    {
        var bottom = GridMap.First(x => x.Value == 'S').Key;
        var top = GridMap.First(x => x.Value == 'E').Key;
        Part1Solution = StepsToGoalBFS(top, bottom, 0); // start from the top, get distance to bottom
        Part2Solution = StepsToGoalBFS(top, bottom, 1); // start from the top, get distance to bottom or first at height 1
    }

    private int StepsToGoalBFS(Position start, Position goal, int exitOnFirstOfHeight)
    {
        Queue<Position> queue = new([start]);
        Dictionary<Position, Position> cameFrom = [];
        var end = goal;

        while (queue.TryDequeue(out var current))
        {
            // End the search if the current location is the goal
            // or the current height is the first instance of the required height
            if (current == goal || GetHeight(GridMap[current]) == exitOnFirstOfHeight)
            {
                end = current;
                break;
            }

            // Create the list of adjacent neighbours from the current grid position
            // where the height difference is 1 or less (and neighbour is within grid)
            foreach (var neighbour in current.GetNeighbours())
            {
                if (neighbour.IsOutsideBounds(MinPos, MaxPos))
                    continue;

                int heightDiff = GetHeight(GridMap[current]) - GetHeight(GridMap[neighbour]);
                if (heightDiff <= 1 && !cameFrom.ContainsKey(neighbour))
                {
                    queue.Enqueue(neighbour);
                    cameFrom[neighbour] = current;
                }
            }
        }

        // Follow path backwards to get the path length
        List<Position> path = [];
        var step = end;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }

        return path.Count;
    }

    private static int GetHeight(char c) => c switch
    {
        'S' => 0,
        'E' => 27,
        _ => c - 'a' + 1
    };
}
