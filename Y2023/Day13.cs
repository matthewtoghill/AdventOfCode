namespace AdventOfCode.Y2023;

internal class Day13() : Solver(2023, 13)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        Part1Solution = Solve(input, 0);
        Part2Solution = Solve(input, 1);
    }

    private static int Solve(string[] input, int maxSmudges) => input.Sum(x => FindMirror(x, maxSmudges));

    private static int CountSmudges(string left, string right) => left.Zip(right, (a, b) => a != b).Count(x => x);

    private static int FindMirror(string block, int maxSmudges)
    {
        if (TryCheckOrientation(block.SplitLines(), maxSmudges, out var row))
            return 100 * row;

        if (TryCheckOrientation(block.SplitIntoColumns().ToArray(), maxSmudges, out var col))
            return col;

        return 0;
    }

    private static bool TryCheckOrientation(string[] lines, int maxSmudges, out int index)
    {
        index = -1;
        for (var i = 1; i < lines.Length; i++)
        {
            var smudges = 0;
            var zip = lines.Take(i).Reverse().Zip(lines.Skip(i)).ToArray();

            zip.ForEach(x => smudges += CountSmudges(x.First, x.Second));

            if (smudges == maxSmudges)
            {
                index = i;
                return true;
            }
        }

        return false;
    }
}
