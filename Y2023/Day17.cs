using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2023;

internal class Day17() : GridSolver(2023, 17)
{
    private record State(Position Position, char Direction, int RunLength);

    public override void Run()
    {
        Part1Solution = Solve([new State(new Position(0, 0), 'E', 0)], 1, 3);
        Part2Solution = Solve([new State(new Position(0, 0), 'E', 0), new State(new Position(0, 0), 'S', 0)], 4, 10);
    }

    private int Solve(State[] startStates, int minRunLength, int maxRunLength)
    {
        var priorityQueue = new PriorityQueue<State, int>();
        var distances = new Dictionary<State, int>();

        foreach (var item in startStates)
        {
            priorityQueue.Enqueue(item, 0);
            distances[item] = 0;
        }

        while (priorityQueue.TryDequeue(out var current, out var currentCost))
        {
            if (current.Position == MaxPos && current.RunLength >= minRunLength)
                return currentCost;

            foreach (var direction in "NESW")
            {
                if (IsOppositeDirection(current.Direction, direction)) continue;
                if (current.RunLength < minRunLength && direction != current.Direction) continue;
                if (current.RunLength == maxRunLength && current.Direction == direction) continue;

                var nextPosition = current.Position.MoveInDirection(direction);
                if (nextPosition.IsOutsideBounds(MinPos, MaxPos)) continue;

                var runLength = direction == current.Direction ? current.RunLength + 1 : 1;
                var nextState = new State(nextPosition, direction, runLength);
                var cost = currentCost + Grid[nextPosition.Row][nextPosition.Col].ToInt();

                if (!distances.TryGetValue(nextState, out int value) || cost < value)
                {
                    distances[nextState] = cost;
                    priorityQueue.Enqueue(nextState, cost);
                }
            }
        }

        return 0;
    }

    private static bool IsOppositeDirection(char currentDirection, char nextDirection)
        => (currentDirection, nextDirection) switch
        {
            ('N', 'S') or ('S', 'N') or ('E', 'W') or ('W', 'E') => true,
            _ => false
        };
}
