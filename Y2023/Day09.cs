namespace AdventOfCode.Y2023;

internal class Day09() : Solver(2023, 9)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        (Part1Solution, Part2Solution) = Solve(input);
    }

    private static (int, int) Solve(string[] input)
    {
        var data = input.Select(x => x.ExtractNumeric<int>().ToList()).ToList();
        List<int> firstNumbers = [];
        List<int> lastNumbers = [];

        foreach (var line in data)
        {
            List<List<int>> gapSequences = [];
            gapSequences.Add(line);
            var current = line;
            while (true)
            {
                var differences = current.EnumerateDifferences().ToList();

                gapSequences.Add(differences);
                if (differences.All(x => x == 0)) break;
                current = differences;
            }

            for (int i = gapSequences.Count - 2; i >= 0; i--)
            {
                gapSequences[i].Add(gapSequences[i][^1] + gapSequences[i + 1][^1]);
                gapSequences[i].Insert(0, gapSequences[i][0] - gapSequences[i + 1][0]);
            }

            firstNumbers.Add(gapSequences[0][0]);
            lastNumbers.Add(gapSequences[0][^1]);
        }

        return (lastNumbers.Sum(), firstNumbers.Sum());
    }
}
