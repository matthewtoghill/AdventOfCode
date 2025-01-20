namespace AdventOfCode.Y2023;

internal class Day12() : Solver(2023, 12)
{
    private static readonly Dictionary<(int, int, int), long> _cache = [];
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 1);
        Part2Solution = Solve(input, 5);
    }

    private static long Solve(string[] input, int repeatCount)
    {
        long result = 0;
        foreach (var line in input)
        {
            _cache.Clear();
            var split = line.Split(' ');
            var pattern = Repeat(split[0], repeatCount);
            var numbers = Repeat(split[1], repeatCount).ExtractInts().ToArray();
            result += CalculateValidPermutations(pattern, numbers, 0, 0, 0);
        }

        return result;
    }

    private static string Repeat(string text, int count) => string.Join('?', Enumerable.Repeat(text, count));

    private static long CalculateValidPermutations(string pattern, int[] groups, int patternIndex, int groupIndex, int damagedLength)
    {
        var key = (patternIndex, groupIndex, damagedLength);

        if (_cache.TryGetValue(key, out var value))
            return value;

        if (patternIndex == pattern.Length)
        {
            if (groupIndex == groups.Length && damagedLength == 0) return 1;
            if (groupIndex == groups.Length - 1 && groups[groupIndex] == damagedLength) return 1;
            return 0;
        }

        long result = 0;

        foreach (var spring in ".#")
        {
            if (pattern[patternIndex] != spring && pattern[patternIndex] != '?')
                continue;

            if (spring == '.')
            {
                if (damagedLength == 0)
                    result += CalculateValidPermutations(pattern, groups, patternIndex + 1, groupIndex, 0);

                else if (groupIndex < groups.Length && groups[groupIndex] == damagedLength)
                    result += CalculateValidPermutations(pattern, groups, patternIndex + 1, groupIndex + 1, 0);
            }
            else
            {
                result += CalculateValidPermutations(pattern, groups, patternIndex + 1, groupIndex, damagedLength + 1);
            }
        }

        _cache[key] = result;
        return result;
    }
}
