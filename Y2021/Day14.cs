namespace AdventOfCode.Y2021;

internal class Day14() : Solver(2021, 14)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        var template = input[0];
        var rules = input[1].SplitLines()
                            .Select(line => line.Split(" -> "))
                            .ToDictionary(x => x[0], x => x[1]);
        Part1Solution = Solve(rules, template, 10);
        Part2Solution = Solve(rules, template, 40);
   }

    private static long Solve(Dictionary<string, string> rules, string polymerTemplate, int steps)
    {
        var polymer = CreateNewPolymer(rules, polymerTemplate, steps);

        // Total the element counts using the first element of each pair
        Dictionary<char, long> elementCounts = [];
        foreach (var pair in polymer)
            elementCounts.IncrementAt(pair.Key[0], pair.Value);

        // Increase the count of the last element from the polymer template by 1
        // as it was missed off once by only counting the first element of each pair
        elementCounts.IncrementAt(polymerTemplate[^1]);

        return elementCounts.Values.Max() - elementCounts.Values.Min();
    }

    private static Dictionary<string, long> CreateNewPolymer(Dictionary<string, string> rules, string polymerTemplate, int steps)
    {
        Dictionary<string, long> pairCounts = [];

        for (int i = 0; i < polymerTemplate.Length - 1; i++)
            pairCounts.IncrementAt(polymerTemplate.Substring(i, 2));

        for (int s = 0; s < steps; s++)
        {
            Dictionary<string, long> updatedCounts = [];
            foreach (var (pair, value) in pairCounts)
            {
                updatedCounts.IncrementAt($"{pair[0]}{rules[pair]}", value);
                updatedCounts.IncrementAt($"{rules[pair]}{pair[1]}", value);
            }

            pairCounts = updatedCounts;
        }

        return pairCounts;
    }
}
