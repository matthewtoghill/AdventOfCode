namespace AdventOfCode.Y2024;

internal class Day25() : Solver(2024, 25)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs().Select(x => x.SplitLines()).ToList();
        Part1Solution = Solve(ParseSchematics('.', input), ParseSchematics('#', input));
        Part2Solution = "Happy Christmas!";
    }

    public static int Solve(IEnumerable<int[]> keys, IEnumerable<int[]> locks)
        => keys.Sum(k => locks.Count(l => Enumerable.Range(0, k.Length).All(i => k[i] + l[i] <= 7)));

    private static List<int[]> ParseSchematics(char c, IEnumerable<string[]> schematics)
        => schematics.Where(x => x[0].StartsWith(c)).Select(GetKeyPattern).ToList();

    private static int[] GetKeyPattern(string[] lines)
        => Enumerable.Range(0, lines[0].Length).Select(x => lines.Count(y => y[x] == '#')).ToArray();
}
