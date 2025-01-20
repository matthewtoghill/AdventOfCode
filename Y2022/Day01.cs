namespace AdventOfCode.Y2022;

internal class Day01() : Solver(2022, 1)
{
    public override void Run()
    {
        var elves = Input.ReadAsParagraphs()
                         .Select(x => x.ExtractInts().Sum())
                         .OrderDescending()
                         .ToArray();

        Part1Solution = elves[0];
        Part2Solution = elves.Take(3).Sum();
    }
}
