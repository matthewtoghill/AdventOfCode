using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2021;

internal class Day09() : GridSolver(2021, 9)
{
    public override void Run()
    {
        var lowPoints = GetLowPoints().ToList();
        Part1Solution = lowPoints.Sum(x => GridMap[x].ToInt() + 1);
        Part2Solution = lowPoints.Select(GetBasinSize).OrderDescending().Take(3).Product();
    }

    private int GetBasinSize(Position lowPoint)
    {
        Queue<Position> queue = new([lowPoint]);
        HashSet<Position> visited = [lowPoint];

        while (queue.TryDequeue(out var current))
        {
            foreach (var neighbour in current.GetNeighbours())
            {
                if (GridMap.TryGetValue(neighbour, out var height) && height != '9' && visited.Add(neighbour))
                    queue.Enqueue(neighbour);
            }
        }

        return visited.Count;
    }

    private IEnumerable<Position> GetLowPoints()
    {
        foreach (var (position, c) in GridMap)
        {
            if (position.GetNeighbours().All(n => !GridMap.TryGetValue(n, out var val) || c < val))
                yield return position;
        }
    }
}
