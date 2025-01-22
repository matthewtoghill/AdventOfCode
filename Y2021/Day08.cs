namespace AdventOfCode.Y2021;

internal class Day08() : Solver(2021, 8)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = input.Sum(line => line.Split('|')[1].Split().Count(x => x.Length is 2 or 3 or 4 or 7)); // digits 1, 7, 4, 8 have unique lengths 2, 3, 4, 7
        Part2Solution = Part2(input);
    }

    private static int Part2(string[] input)
        => input.Sum(line =>
        {
            var split = line.Split('|');
            var signals = split[0].Trim().Split();
            var output = split[1].Trim().Split();
            var signalLetterFrequencies = signals.SelectMany(x => x).GetFrequencies();
            return DecodeOutput(output, signalLetterFrequencies);
        });

    private static int DecodeOutput(string[] output, Dictionary<char, int> signalLetterFrequencies)
        => int.Parse(string.Concat(output.Select(x => ConvertToDigit(x, signalLetterFrequencies))));

    private static string ConvertToDigit(string digit, Dictionary<char, int> signalLetterFrequencies)
    {
        var digitSum = digit.Sum(c => signalLetterFrequencies[c]);
        return _scoreToDigitMap[digitSum].ToString();
    }

    private static readonly Dictionary<int, int> _scoreToDigitMap = new()
    {
        [42] = 0, [17] = 1, [34] = 2, [39] = 3, [30] = 4, [37] = 5, [41] = 6, [25] = 7, [49] = 8, [45] = 9
    };
}
