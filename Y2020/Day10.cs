namespace AdventOfCode.Y2020;

internal class Day10() : Solver(2020, 10)
{
    public override void Run()
    {
        var adapters = Input.ReadAllLinesTo<int>().Order().Prepend(0).ToList();
        Part1Solution = FindJoltageDifferences(adapters);
        Part2Solution = CountArrangements(adapters);
    }

    private static int FindJoltageDifferences(List<int> adapters)
    {
        var diffs = adapters.EnumerateDifferences().GetFrequencies();
        return diffs[1] * (diffs[3] + 1);
    }

    private static long CountArrangements(List<int> adapters)
    {
        Dictionary<int, long> arrangements = new() { [0] = 1 };

        foreach (var adapter in adapters.Skip(1))
        {
            arrangements[adapter] = Enumerable.Range(adapter - 3, 3)
                .Where(arrangements.ContainsKey)
                .Sum(x => arrangements[x]);
        }

        return arrangements[adapters[^1]];
    }
}
