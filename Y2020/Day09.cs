namespace AdventOfCode.Y2020;

internal class Day09() : Solver(2020, 9)
{
    public override void Run()
    {
        var numbers = Input.ReadAllLinesTo<long>().ToArray();
        var invalidNumber = FindInvalidNumber(numbers, 25);
        Part1Solution = invalidNumber;
        Part2Solution = FindEncryptionWeakness(numbers, invalidNumber);
    }

    private static long FindInvalidNumber(long[] numbers, int preamble)
    {
        for (int i = preamble; i < numbers.Length; i++)
        {
            var number = numbers[i];
            var window = numbers[(i - preamble)..i];
            var hashSet = new HashSet<long>(window);
            var found = false;

            foreach (var num in window)
            {
                var complement = number - num;
                if (hashSet.Contains(complement) && complement != num)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                return number;
        }

        return -1;
    }

    private static long FindEncryptionWeakness(long[] numbers, long invalidNumber)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            long sum = 0;
            long min = long.MaxValue;
            long max = long.MinValue;

            for (int j = i; j < numbers.Length; j++)
            {
                sum += numbers[j];
                min = Math.Min(min, numbers[j]);
                max = Math.Max(max, numbers[j]);

                if (sum == invalidNumber)
                    return min + max;

                if (sum > invalidNumber)
                    break;
            }
        }

        return -1;
    }
}
