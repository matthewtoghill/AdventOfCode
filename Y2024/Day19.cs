namespace AdventOfCode.Y2024;

internal class Day19() : Solver(2024, 19)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        var possibleDesigns = Solve(input[0].SplitOn(", "), input[1].SplitLines());
        Part1Solution = possibleDesigns.Count;
        Part2Solution = possibleDesigns.Values.Sum();
    }

    private static Dictionary<string, long> Solve(string[] towels, string[] designs)
        => designs.Select(x => new { Design = x, Count = CalculatePossibleWays(x, towels, []) })
                  .Where(x => x.Count > 0)
                  .ToDictionary(x => x.Design, x => x.Count);

    private static long CalculatePossibleWays(string design, string[] towels, Dictionary<string, long> cache)
    {
        if (string.IsNullOrWhiteSpace(design))
            return 1;

        if (cache.TryGetValue(design, out var count))
            return count;

        long result = towels.Where(design.StartsWith).Sum(x => CalculatePossibleWays(design[x.Length..], towels, cache));

        cache[design] = result;

        return result;
    }
}
