namespace AdventOfCode.Y2020;

internal class Day02() : Solver(2020, 2)
{
    private record Policy(int Left, int Right, char C, string Password);

    public override void Run()
    {
        var policies = Input.ReadAllLines().Select(ParsePolicy);
        Part1Solution = policies.Count(x => x.Password.Count(p => p == x.C).IsBetween(x.Left, x.Right));
        Part2Solution = policies.Count(x => x.Password[x.Left - 1] == x.C ^ x.Password[x.Right - 1] == x.C);
    }

    private static Policy ParsePolicy(string line)
    {
        var split = line.Split(["-", " ", ":"], StringSplitOptions.RemoveEmptyEntries);
        return new(int.Parse(split[0]), int.Parse(split[1]), split[2][0], split[3]);
    }
}
