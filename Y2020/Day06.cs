namespace AdventOfCode.Y2020;

internal class Day06() : Solver(2020, 6)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        Part1Solution = input.Sum(CountYes);
        Part2Solution = input.Sum(CountAllYes);
    }

    private static int CountYes(string group)
        => group.Replace("\n", "").ToCharArray().Distinct().Count();

    private static int CountAllYes(string group)
    {
        var answers = group.SplitLines().Select(x => x.ToCharArray()).ToArray();
        return answers.Skip(1).Aggregate(answers[0].ToHashSet(), (acc, x) => acc.Intersect(x).ToHashSet()).Count;
    }
}
