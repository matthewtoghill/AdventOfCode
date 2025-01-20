namespace AdventOfCode.Y2022;

internal class Day04() : Solver(2022, 4)
{
    public override void Run()
    {
        var input = Input.ReadAllLines()
                         .SelectMany(x => x.SplitOn('-', ',').Select(int.Parse))
                         .Chunk(4);

        Part1Solution = input.Count(x => IsContainedWithin(x[0], x[1], x[2], x[3]));
        Part2Solution = input.Count(x => HasOverlap(x[0], x[1], x[2], x[3]));
    }

    private static bool IsContainedWithin(int startA, int endA, int startB, int endB)
        => (startA >= startB && endA <= endB) || (startB >= startA && endB <= endA);

    private static bool HasOverlap(int startA, int endA, int startB, int endB)
        => endA >= startB && endB >= startA;
}
