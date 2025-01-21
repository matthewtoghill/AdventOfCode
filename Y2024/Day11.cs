namespace AdventOfCode.Y2024;

internal class Day11() : Solver(2024, 11)
{
    public override void Run()
    {
        var input = Input.ReadAll();
        Part1Solution = Solve(input, 25);
        Part2Solution = Solve(input, 75);
    }

    private static long Solve(string input, int iterations)
    {
        var stones = input.ExtractNumeric<long>().GetFrequenciesAsLong();

        for (int i = 0; i < iterations; i++)
        {
            foreach (var (key, count) in stones.ToList())
            {
                stones.IncrementAt(key, -count);
                if (stones[key] == 0) stones.Remove(key);

                if (key == 0)
                {
                    stones.IncrementAt(1, count);
                }
                else
                {
                    var digits = key.DigitCount();
                    if (digits % 2 == 0)
                    {
                        var (left, right) = SplitNumber(key, digits);
                        stones.IncrementAt(left, count);
                        stones.IncrementAt(right, count);
                    }
                    else
                    {
                        stones.IncrementAt(key * 2024, count);
                    }
                }
            }
        }

        return stones.Values.Sum();
    }

    private static (long, long) SplitNumber(long num, int digits)
    {
        var divisor = (long)Math.Pow(10, digits / 2);
        return (num / divisor, num % divisor);
    }
}
