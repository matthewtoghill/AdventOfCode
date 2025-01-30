namespace AdventOfCode.Y2020;

internal class Day03() : Solver(2020, 3)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = CountTreesHit(input, (3, 1));
        Part2Solution = CountTreesHit(input, (3, 1), (1, 1), (5, 1), (7, 1), (1, 2));
    }

    private static long CountTreesHit(string[] input, params (int right, int down)[] slopes)
    {
        long result = 1;

        foreach (var (right, down) in slopes)
        {
            var (x, y, trees) = (0, 0, 0);

            while (y < input.Length)
            {
                if (input[y][x] == '#')
                    trees++;

                x = (x + right) % input[0].Length;
                y += down;
            }

            result *= trees;
        }

        return result;
    }
}
