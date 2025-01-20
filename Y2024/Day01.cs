namespace AdventOfCode.Y2024;

internal class Day01() : Solver(2024, 1)
{
    public override void Run()
    {
        var (leftIds, rightIds) = GetIdLists(Input.ReadAllLines());
        Part1Solution = Part1(leftIds, rightIds);
        Part2Solution = Part2(leftIds, rightIds.GetFrequencies());
    }

    private static (List<int>, List<int>) GetIdLists(string[] data)
    {
        List<int> left = [];
        List<int> right = [];

        foreach (var line in data)
        {
            var split = line.ExtractInts().ToList();

            left.Add(split[0]);
            right.Add(split[1]);
        }

        return (left, right);
    }

    private static int Part1(List<int> leftIds, List<int> rightIds)
        => leftIds.Order().Zip(rightIds.Order(), (l, r) => Math.Abs(l - r)).Sum();

    private static int Part2(List<int> leftIds, Dictionary<int, int> frequencies)
        => leftIds.Sum(x => frequencies.GetValueOrDefault(x, 0) * x);
}
