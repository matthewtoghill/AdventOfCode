namespace AdventOfCode.Y2021;

internal class Day01() : Solver(2021, 1)
{
    public override void Run()
    {
        var input = Input.ReadAllLinesTo<int>().ToArray();
        Part1Solution = input.EnumerateDifferences().Count(x => x > 0);
        Part2Solution = Enumerable.Range(0, input.Length - 2)
                                  .Select(x => input.Skip(x).Take(3).Sum())
                                  .EnumerateDifferences()
                                  .Count(x => x > 0);
    }
}
