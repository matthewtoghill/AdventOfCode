namespace AdventOfCode.Y2024;

internal class Day02() : Solver(2024, 2)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = CountSafe(input, 0);
        Part2Solution = CountSafe(input, 1);
    }

    private static int CountSafe(string[] input, int maxProblems)
        => input.Count(line => line.ExtractInts()
                                   .GetSubsetVariations(maxProblems)
                                   .Any(x => IsSafe(x.EnumerateDifferences())));

    private static bool IsSafe(IEnumerable<int> differences)
        => differences.All(x => x is > 0 and <= 3)
        || differences.All(x => x is < 0 and >= -3);
}
