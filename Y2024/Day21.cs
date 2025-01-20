using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day21() : Solver(2024, 21)
{
    private record State(char CurrentKey, char NextKey, int RobotNumber);
    private record Robot(Dictionary<char, Position> Buttons);

    private static readonly Dictionary<char, Position> NumPad = new()
    {
        ['7'] = new(0, 0), ['8'] = new(1, 0), ['9'] = new(2, 0),
        ['4'] = new(0, 1), ['5'] = new(1, 1), ['6'] = new(2, 1),
        ['1'] = new(0, 2), ['2'] = new(1, 2), ['3'] = new(2, 2),
                           ['0'] = new(1, 3), ['A'] = new(2, 3),
    };

    private static readonly Dictionary<char, Position> DirPad = new()
    {
                           ['^'] = new(1, 0), ['A'] = new(2, 0),
        ['<'] = new(0, 1), ['v'] = new(1, 1), ['>'] = new(2, 1),
    };

    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 2);
        Part2Solution = Solve(input, 25);
    }

    private long Solve(string[] input, int robotCount)
    {
        List<Robot> robots = [new(NumPad), .. Enumerable.Repeat<Robot>(new(DirPad), robotCount)];
        return input.Sum(code => int.Parse(code[..3]) * EnterCode(code, robots, []));
    }

    private long EnterCode(string code, List<Robot> robots, Dictionary<State, long> cache)
    {
        if (robots.Count == 0)
            return code.Length;

        var currentKey = 'A';
        long length = 0;

        foreach (var nextKey in code)
        {
            length += CalculateShortestPath(currentKey, nextKey, robots, cache);
            currentKey = nextKey;
        }

        return length;
    }

    private long CalculateShortestPath(char currentKey, char nextKey, List<Robot> robots, Dictionary<State, long> cache)
    {
        var state = new State(currentKey, nextKey, robots.Count);

        if (cache.TryGetValue(state, out var cost))
            return cost;

        var robot = robots[0];
        var currentPos = robot.Buttons[currentKey];
        var nextPos = robot.Buttons[nextKey];

        var (diffX, diffY) = (nextPos.X - currentPos.X, nextPos.Y - currentPos.Y);
        var horizontalSteps = new string(diffX < 0 ? '<' : '>', Math.Abs(diffX));
        var verticalSteps = new string(diffY < 0 ? '^' : 'v', Math.Abs(diffY));

        cost = long.MaxValue;

        if (robot.Buttons.ContainsValue(new Position(currentPos.X, nextPos.Y)))
            cost = Math.Min(cost, EnterCode($"{verticalSteps}{horizontalSteps}A", robots[1..], cache));

        if (robot.Buttons.ContainsValue(new Position(nextPos.X, currentPos.Y)))
            cost = Math.Min(cost, EnterCode($"{horizontalSteps}{verticalSteps}A", robots[1..], cache));

        cache[state] = cost;

        return cost;
    }
}
