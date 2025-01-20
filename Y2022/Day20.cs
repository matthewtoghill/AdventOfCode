namespace AdventOfCode.Y2022;

internal class Day20() : Solver(2022, 20)
{
    public override void Run()
    {
        var input = Input.ReadAllLinesTo<long>().ToArray();
        Part1Solution = MixNumbers(input, 1, 1);
        Part2Solution = MixNumbers(input, 10, 811589153);
    }

    private static long MixNumbers(long[] numbers, int iterations, long decryptionKey)
    {
        var nums = numbers.Select(x => x * decryptionKey).ToList();
        var indexes = Enumerable.Range(0, nums.Count).ToList();

        for (int n = 0; n < iterations; n++)
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                var oldIndex = indexes.IndexOf(i);
                indexes.RemoveAt(oldIndex);
                var newIndex = (int)(oldIndex + nums[i]).Mod(indexes.Count);
                indexes.Insert(newIndex, i);
            }
        }

        var indexOfZero = indexes.IndexOf(nums.IndexOf(0));
        return Enumerable.Range(1, 3).Sum(x => nums[indexes[(indexOfZero + (1000 * x)).Mod(nums.Count)]]);
    }
}
