namespace AdventOfCode.Y2025;

internal class Day11() : Solver(2025, 11)
{
    private Dictionary<string, HashSet<string>> _devices = [];
    private readonly Dictionary<State, long> _cache = [];

    public override void Run()
    {
        _devices = GetDevices(Input.ReadAllLines());

        Part1Solution = Solve(new("you"));
        Part2Solution = Solve(new("svr", false, false));
    }

    private long Solve(State state)
    {
        var result = 0L;

        var currentState = state with
        {
            VisitedDac = state.VisitedDac || state.Node == "dac",
            VisitedFft = state.VisitedFft || state.Node == "fft"
        };

        if (_cache.TryGetValue(currentState, out var cached))
            return cached;


        foreach (var device in _devices[state.Node])
        {
            var nextState = currentState with { Node = device };
            if (device == "out")
            {
                if (currentState.VisitedDac && currentState.VisitedFft)
                    result++;
            }
            else
            {
                result += _cache.TryGetValue(nextState, out var steps) ? steps : Solve(nextState);
            }
        }

        _cache[currentState] = result;

        return result;
    }

    private static Dictionary<string, HashSet<string>> GetDevices(string[] input)
        => input.Select(line => line.Split([" ", ":"], StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(parts => parts[0], parts => parts[1..].ToHashSet());

    private record State(string Node, bool VisitedDac = true, bool VisitedFft = true);
}
