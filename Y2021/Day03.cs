namespace AdventOfCode.Y2021;

internal class Day03() : Solver(2021, 3)
{
    public override void Run()
    {
        var input = Input.ReadAll();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(string input)
    {
        var bitFrequencies = input.SplitIntoColumns().Select(x => x.GetFrequencies());
        var gamma = string.Concat(bitFrequencies.Select(x => x.MaxBy(x => x.Value).Key)).BinaryToInt();
        var epsilon = string.Concat(bitFrequencies.Select(x => x.MinBy(x => x.Value).Key)).BinaryToInt();
        return gamma * epsilon;
    }

    private static int Part2(string input)
    {
        var oxygenRating = GetFilteredRating(input, 0, (count0, count1) => count1 >= count0 ? '1' : '0').BinaryToInt();
        var scrubberRating = GetFilteredRating(input, int.MaxValue, (count0, count1) => count0 <= count1 ? '0' : '1').BinaryToInt();
        return oxygenRating * scrubberRating;
    }

    private delegate char BitCriteria(int count0, int countB);
    private static string GetFilteredRating(string input, int defaultVal, BitCriteria bitCriteria)
    {
        var lines = input.SplitLines();
        for (int i = 0; i < lines[0].Length; i++)
        {
            Dictionary<char, int> frequencies = [];
            lines.ForEach(line => frequencies.IncrementAt(line[i]));
            var keepBit = bitCriteria(frequencies.GetValueOrDefault('0', defaultVal), frequencies.GetValueOrDefault('1', defaultVal));
            lines = lines.Where(b => b[i] == keepBit).ToArray();
        }
        return lines[0];
    }
}
