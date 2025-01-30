namespace AdventOfCode.Y2020;

internal class Day01() : Solver(2020, 1)
{
    public override void Run()
    {
        var input = Input.ReadAllLinesTo<int>().ToArray();
        Part1Solution = input.Where(x => input.Contains(2020 - x)).Product();
        Part2Solution = input.Where(x => input.FirstOrDefault(y => input.Contains(2020 - x - y)) != 0).Product();
    }
}
