using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day16() : Solver(2024, 16)
{
    private record ReindeerState(Position Position, char Direction, int Score, HashSet<Position> Route);

    private static readonly Dictionary<char, char> TurnRight = new() { ['N'] = 'E', ['E'] = 'S', ['S'] = 'W', ['W'] = 'N' };
    private static readonly Dictionary<char, char> TurnLeft = new() { ['N'] = 'W', ['W'] = 'S', ['S'] = 'E', ['E'] = 'N' };

    public override void Run()
    {
        (Part1Solution, Part2Solution) = Solve(Input.ReadAllLines());
    }

    private static (int, int) Solve(string[] input)
    {
        var map = input.AsCharMap();
        var start = map.First(x => x.Value == 'S').Key;
        var end = map.First(x => x.Value == 'E').Key;

        var initialState = new ReindeerState(start, 'E', 0, [start]);
        PriorityQueue<ReindeerState, int> queue = new([(initialState, 0)]);

        var lowestScore = int.MaxValue;
        List<ReindeerState> endStates = [];
        DefaultDictionary<(Position, char), int> minScores = new(defaultValue: int.MaxValue);

        while (queue.TryDequeue(out var current, out var score))
        {
            if (score > lowestScore) continue;

            if (current.Position == end)
            {
                endStates.Add(current);
                lowestScore = Math.Min(score, lowestScore);
                continue;
            }

            AddStep(current, current.Direction, score + 1);
            AddStep(current, TurnRight[current.Direction], score + 1001);
            AddStep(current, TurnLeft[current.Direction], score + 1001);

            void AddStep(ReindeerState state, char direction, int nextScore)
            {
                var next = state.Position.MoveInDirection(direction);

                if (map.GetValueOrDefault(next) == '#') return;

                if (minScores[(next, direction)] >= nextScore)
                {
                    minScores[(next, direction)] = nextScore;
                    var nextState = new ReindeerState(next, direction, nextScore, [.. state.Route, next]);
                    queue.Enqueue(nextState, nextScore);
                }
            }
        }

        var tilesOnBestPaths = endStates.Where(x => x.Score == lowestScore).SelectMany(x => x.Route).ToHashSet().Count;

        return (lowestScore, tilesOnBestPaths);
    }
}
