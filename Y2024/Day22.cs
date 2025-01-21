namespace AdventOfCode.Y2024;

internal class Day22() : Solver(2024, 22)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        (Part1Solution, Part2Solution) = Solve(input);
    }

    private static (long, long) Solve(string[] input)
    {
        long endSecretSum = 0;
        Dictionary<string, int> cache = [];

        foreach (var line in input)
        {
            var secret = long.Parse(line);
            List<long> prices = [];

            for (int i = 0; i < 2000; i++)
            {
                secret = GetNextSecretNumber(secret);
                prices.Add(secret % 10);
            }

            endSecretSum += secret;

            HashSet<string> seen = [];
            var differences = prices.EnumerateDifferences().ToList();

            for (int i = 0; i < prices.Count - 4; i++)
            {
                var pattern = string.Join("|", differences.Slice(i, 4));

                if (seen.Add(pattern))
                    cache.IncrementAt(pattern, (int)prices[i + 4]);
            }
        }

        return (endSecretSum, cache.Values.Max());
    }

    private static long GetNextSecretNumber(long secret)
        => secret.MixAndPrune(x => x * 64)
                 .MixAndPrune(x => x / 32)
                 .MixAndPrune(x => x * 2048);
}

file static class Extensions
{
    internal static long MixAndPrune(this long secret, Func<long, long> step) => (secret ^ step(secret)) % 16777216;
}
