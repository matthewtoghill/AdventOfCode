namespace AdventOfCode.Y2025;

internal class Day12() : Solver(2025, 12)
{
    public override void Run()
    {
        var (presents, regions) = ParseInput(Input.ReadAsParagraphs());

        Part1Solution = regions.Count(region => region.Requirements.Zip(presents).Sum(x => x.First * x.Second) <= region.Area);
        Part2Solution = "Happy Christmas!";
    }

    private static (int[] Presents, List<Region> Regions) ParseInput(string[] input)
    {
        var presents = input[..^1].Select(x => x.Count(c => c == '#')).ToArray();

        var regions = input[^1].SplitLines().Select(line => {
            var nums = line.ExtractInts().ToList();
            return new Region(nums[0] * nums[1], nums[2..]);
        }).ToList();

        return (presents, regions);
    }

    private record Region(int Area, List<int> Requirements);
}
