namespace AdventOfCode.Y2023;

internal class Day18() : Solver(2023, 18)
{
    private record Dig(char Direction, long Distance);

    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input.Select(Part1Parser));
        Part2Solution = Solve(input.Select(Part2Parser));
    }

    private static long Solve(IEnumerable<Dig> instructions)
    {
        (long, long) current = (0, 0);
        List<(long, long)> grid = [current];
        long perimeter = 0;

        foreach (var item in instructions)
        {
            current = current.MoveInDirection(item.Direction, item.Distance);
            perimeter += item.Distance;
            grid.Add(current);
        }

        return 1 + ((grid.CalculateArea() + perimeter) / 2);
    }

    private static Dig Part1Parser(string line)
    {
        var split = line.Split();
        return new(split[0][0], long.Parse(split[1]));
    }

    private static Dig Part2Parser(string line)
    {
        var colour = line.Split([" ", "(", ")", "#"], StringSplitOptions.RemoveEmptyEntries)[2];
        return new(GetDirection(colour[^1]), Convert.ToInt64(colour[..5], 16));
    }

    private static char GetDirection(char val)
        => val switch
        {
            '0' => 'R',
            '1' => 'D',
            '2' => 'L',
            '3' => 'U',
            _ => throw new NotSupportedException()
        };
}
