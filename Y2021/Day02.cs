namespace AdventOfCode.Y2021;

internal class Day02() : Solver(2021, 2)
{
    private record Submarine(int X, int Depth, int Aim);
    private delegate Submarine MoveSubmarine(Submarine sub, int units);

    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input,
                              forward: (sub, units) => sub with { X = sub.X + units },
                              up: (sub, units) => sub with { Depth = sub.Depth - units },
                              down: (sub, units) => sub with { Depth = sub.Depth + units });
        Part2Solution = Solve(input,
                              forward: (sub, units) => sub with { X = sub.X + units, Depth = sub.Depth + (sub.Aim * units) },
                              up: (sub, units) => sub with { Aim = sub.Aim - units },
                              down: (sub, units) => sub with { Aim = sub.Aim + units });
    }

    private static int Solve(string[] input, MoveSubmarine forward, MoveSubmarine up, MoveSubmarine down)
    {
        Submarine sub = new(0, 0, 0);
        foreach (var line in input)
        {
            var split = line.Split(' ');
            var units = int.Parse(split[1]);
            sub = split[0] switch
            {
                "forward" => forward(sub, units),
                "up" => up(sub, units),
                "down" => down(sub, units),
                _ => sub
            };
        }

        return sub.X * sub.Depth;
    }
}
