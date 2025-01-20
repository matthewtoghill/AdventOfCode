namespace AdventOfCode.Y2024;

internal class Day07() : Solver(2024, 7)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, false);
        Part2Solution = Solve(input, true);
    }

    private static long Solve(string[] input, bool tryConcat)
    {
        long calibrationResult = 0;
        foreach (var line in input)
        {
            var nums = line.ExtractNumeric<long>().ToList();
            var target = nums[0];
            nums = nums[1..];

            if (CheckCalculation(target, nums[0], nums[1..], tryConcat))
                calibrationResult += target;
        }

        return calibrationResult;
    }

    private static bool CheckCalculation(long target, long current, List<long> nums, bool tryConcat)
    {
        if (current > target) return false;
        if (nums.Count == 0) return current == target;

        return CheckCalculation(target, current + nums[0], nums[1..], tryConcat)
            || CheckCalculation(target, current * nums[0], nums[1..], tryConcat)
            || (tryConcat && CheckCalculation(target, current.Concat(nums[0]), nums[1..], tryConcat));
    }
}
