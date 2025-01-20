namespace AdventOfCode.Y2022;

internal class Day03() : Solver(2022, 3)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = input.Sum(x => HalveAndGetIntersections(x).First().GetPriorityScore());
        Part2Solution = input.Chunk(3).Sum(x => x[0].Intersect(x[1]).Intersect(x[2]).First().GetPriorityScore());
    }

    private static char[] HalveAndGetIntersections(string line) => GetIntersections(line[..(line.Length / 2)], line[(line.Length / 2)..]);
    private static char[] GetIntersections(string left, string right) => left.Intersect(right).ToArray();
}

file static class Extensions
{
    public static int GetPriorityScore(this char c) => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
}