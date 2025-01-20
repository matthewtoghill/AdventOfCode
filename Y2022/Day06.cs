namespace AdventOfCode.Y2022;

internal class Day06() : Solver(2022, 6)
{
    public override void Run()
    {
        var input = Input.ReadAll().AsSpan();
        Part1Solution = GetIndexAfterFirstUniqueSet(input, 4);
        Part2Solution = GetIndexAfterFirstUniqueSet(input, 14);
    }

    private static int GetIndexAfterFirstUniqueSet(ReadOnlySpan<char> span, int setLength)
    {
        for (int i = setLength; i < span.Length; i++)
        {
            var set = new HashSet<char>(span.Slice(i - setLength, setLength).ToArray());
            if (set.Count == setLength)
                return i;
        }

        return 0;
    }
}
