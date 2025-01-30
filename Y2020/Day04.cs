using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020;

internal class Day04() : Solver(2020, 4)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();

        var passports = input.Select(ParsePassport).ToList();
        string[] requiredKeys = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];

        Part1Solution = passports.Count(x => requiredKeys.All(x.ContainsKey));
        Part2Solution = passports.Count(IsValid);
    }

    private delegate bool Validator(string value);
    private readonly Dictionary<string, Validator> PassportRules = new()
    {
        ["byr"] = value => int.Parse(value).IsBetween(1920, 2002),
        ["iyr"] = value => int.Parse(value).IsBetween(2010, 2020),
        ["eyr"] = value => int.Parse(value).IsBetween(2020, 2030),
        ["hgt"] = value =>
        {
            if (value.EndsWith("cm"))
            {
                return int.Parse(value[..^2]).IsBetween(150, 193);
            }
            else if (value.EndsWith("in"))
            {
                return int.Parse(value[..^2]).IsBetween(59, 76);
            }
            return false;
        },
        ["hcl"] = value => Regex.IsMatch(value, "#[0-9a-f]{6}"),
        ["ecl"] = value => Regex.IsMatch(value, "amb|blu|brn|gry|grn|hzl|oth"),
        ["pid"] = value => Regex.IsMatch(value, "^[0-9]{9}$")
    };

    private bool IsValid(Dictionary<string, string> passport)
        => PassportRules.All(x => passport.TryGetValue(x.Key, out var value) && x.Value(value));

    private static Dictionary<string, string> ParsePassport(string item)
        => item.SplitLines()
               .SelectMany(x => x.Split([":", " "], StringSplitOptions.RemoveEmptyEntries))
               .Chunk(2)
               .ToDictionary(x => x[0], x => x[1]);
}
