namespace AdventOfCode.Y2023;

internal class Day01() : Solver(2023, 1)
{
    public override void Run()
    {
        var input = Input.ReadAll();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(string input) => input.SplitLines().Sum(CalibrationValue);
    private static int CalibrationValue(string line) => int.Parse($"{line.First(char.IsNumber)}{line.Last(char.IsNumber)}");

    private static int Part2(string input)
        => input.Replace("one", "o1e")      //"one1one"
                .Replace("two", "t2o")      //"two2two"
                .Replace("three", "t3e")    //"three3three"
                .Replace("four", "f4r")     //"four4four"
                .Replace("five", "f5e")     //"five5five"
                .Replace("six", "s6x")      //"six6six"
                .Replace("seven", "s7n")    //"seven7seven"
                .Replace("eight", "e8t")    //"eight8eight"
                .Replace("nine", "n9e")     //"nine9nine"
                .SplitLines()
                .Sum(Part1);
}
