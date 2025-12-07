namespace AdventOfCode.Y2025;

internal class Day05() : Solver(2025, 5)
{
    public override void Run()
    {
        var (ranges, ids) = ParseInput(Input.ReadAsParagraphs());
        Part1Solution = ids.Count(id => ranges.Any(x => id.IsBetween(x.Min, x.Max)));
        Part2Solution = Part2(ranges);
    }

    private static long Part2(List<(long Min, long Max)> ranges)
    {
        ranges = ranges.OrderBy(r => r.Min).ThenBy(r => r.Max).ToList();
        var total = 0L;
        var currentStart = ranges[0].Min;
        var currentEnd = ranges[0].Max;

        foreach (var (min, max) in ranges)
        {
            if (min <= currentEnd + 1)
            {
                currentEnd = Math.Max(currentEnd, max);
            }
            else
            {
                total += currentEnd - currentStart + 1;
                currentStart = min;
                currentEnd = max;
            }
        }

        return total + (currentEnd - currentStart + 1);
    }

    private static (List<(long Min, long Max)>, List<long>) ParseInput(string[] input)
    {
        var ranges = input[0].ExtractPositiveNumeric<long>().Chunk(2).Select(x => (x[0], x[1])).ToList();
        var ids = input[1].ExtractNumeric<long>().ToList();

        return (ranges, ids);
    }
}
