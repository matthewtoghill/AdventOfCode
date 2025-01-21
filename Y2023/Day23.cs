using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2023;

internal class Day23() : GridSolver(2023, 23)
{
    private record State(Position Position, int Distance);

    public override void Run()
    {
        Part1Solution = Solve(true);
        Part2Solution = Solve(false);
    }

    private int Solve(bool includeSlopes)
    {
        var start = new Position(Grid[0].IndexOf('.'), 0);
        var end = new Position(Grid[^1].IndexOf('.'), Grid.Length - 1);

        var crossroads = GetCrossroads(start, end).ToHashSet();
        var graph = BuildGraph(crossroads, includeSlopes);

        return FindLongestPath(graph, [], new State(start, 0), end);
    }

    private Dictionary<Position, List<State>> BuildGraph(HashSet<Position> crossroads, bool includeSlopes)
    {
        Dictionary<Position, List<State>> graph = [];

        foreach (var start in crossroads)
        {
            graph[start] = [];

            foreach (var end in crossroads.Where(c => c != start))
            {
                if (TryGetDistance(crossroads, start, end, includeSlopes, out int distance))
                    graph[start].Add(new State(end, distance));
            }
        }

        return graph;
    }

    private static int FindLongestPath(Dictionary<Position, List<State>> graph, HashSet<Position> visited, State current, Position end)
    {
        if (current.Position == end)
            return current.Distance;

        var maxLength = 0;

        foreach (var next in graph[current.Position])
        {
            if (!visited.Add(next.Position)) continue;
            var length = FindLongestPath(graph, visited, new(next.Position, current.Distance + next.Distance), end);
            maxLength = Math.Max(maxLength, length);

            visited.Remove(next.Position);
        }

        return maxLength;
    }

    private bool TryGetDistance(HashSet<Position> crossroads, Position start, Position end, bool includeSlopes, out int distance)
    {
        Queue<(Position Position, int Steps)> queue = new([(start, 0)]);
        HashSet<Position> visited = [];
        distance = 0;

        while (queue.TryDequeue(out var current))
        {
            if (!visited.Add(current.Position)) continue;

            if (current.Position == end)
            {
                distance = current.Steps;
                return true;
            }

            if (crossroads.Contains(current.Position) && current.Position != start) continue;

            foreach (var neighbour in GetValidNeighbours(current.Position, includeSlopes))
            {
                queue.Enqueue((neighbour, current.Steps + 1));
            }
        }

        return false;
    }

    private IEnumerable<Position> GetValidNeighbours(Position position, bool includeSlopes)
    {
        foreach (var neighbour in position.GetNeighbours())
        {
            if (neighbour.IsOutsideBounds(MinPos, MaxPos)) continue;

            char c = GridMap[neighbour];

            if (c == '#') continue;
            if (includeSlopes && !IsValidSlope(position, neighbour, c)) continue;

            yield return neighbour;
        }
    }

    private static bool IsValidSlope(Position current, Position neighbour, char slope)
    {
        if (!">v<^".Contains(slope)) return true;

        var direction = neighbour - current;
        return slope switch
        {
            '>' => direction == (1, 0),
            '<' => direction == (-1, 0),
            'v' => direction == (0, 1),
            '^' => direction == (0, -1),
            _ => true
        };
    }

    private IEnumerable<Position> GetCrossroads(Position start, Position end)
    {
        yield return start;

        for (int row = 0; row < Grid.Length; row++)
        {
            for (int col = 0; col < Grid[row].Length; col++)
            {
                if (Grid[row][col] != '.') continue;

                var current = new Position(col, row);
                int wallCount = current.GetNeighbours().Count(n => n.IsBetween(MinPos, MaxPos) && GridMap[n] == '#');

                if (wallCount < 2)
                    yield return current;
            }
        }

        yield return end;
    }
}
