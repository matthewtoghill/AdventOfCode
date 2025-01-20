using System.Text.RegularExpressions;

namespace AdventOfCode.Y2024;

internal partial class Day03() : Solver(2024, 3)
{
    public override void Run()
    {
        var input = Input.ReadAll();
        Part1Solution = Solve(input, Part1Regex());
        Part2Solution = Solve(input, Part2Regex());
    }

    private static int Solve(string input, Regex regex)
    {
        bool isEnabled = true;
        int result = 0;

        foreach (Match item in regex.Matches(input))
        {
            switch (item.Value[..3])
            {
                case "do(":
                    isEnabled = true;
                    break;
                case "don":
                    isEnabled = false;
                    break;
                case "mul":
                    if (!isEnabled) continue;
                    result += int.Parse(item.Groups[1].Value) * int.Parse(item.Groups[2].Value);
                    break;
            }
        }

        return result;
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex Part1Regex();

    [GeneratedRegex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)")]
    private static partial Regex Part2Regex();
}
