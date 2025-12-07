namespace AdventOfCode.Y2025;

internal class Day02() : Solver(2025, 2)
{
    public override void Run()
    {
        var input = Input.ReadAll().Split([',', '-']).Select(long.Parse).Chunk(2).ToList();
        Part1Solution = Solve(input, x => x == 2);
        Part2Solution = Solve(input, x => x > 2);
    }

    private static long Solve(List<long[]> input, Func<int, bool> repeatCriteria)
    {
        long result = 0;

        foreach (var item in input)
        {
            for (long i = item[0]; i <= item[1]; i++)
            {
                var str = i.ToString();
                var span = str.AsSpan();
                var lenStr = span.Length;

                for (int len = 1; len <= lenStr / 2; len++)
                {
                    if (lenStr % len != 0)
                        continue;

                    var repeatCount = str.Length / len;

                    if (!repeatCriteria(repeatCount))
                        continue;

                    var pattern = str[..len];
                    var isRepeated = true;

                    for (int pos = len; pos < lenStr; pos += len)
                    {
                        if (!span.Slice(pos, len).SequenceEqual(pattern))
                        {
                            isRepeated = false;
                            break;
                        }
                    }

                    if (isRepeated)
                    {
                        result += i;
                        break;
                    }
                }
            }
        }

        return result;
    }
}
