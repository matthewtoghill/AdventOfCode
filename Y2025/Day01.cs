namespace AdventOfCode.Y2025;

internal class Day01() : Solver(2025, 1)
{
    public override void Run()
    {
        var input = Input.ReadAllLines().Select(line => (line[0], int.Parse(line[1..]))).ToList();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(List<(char, int)> input)
    {
        var result = 0;
        var current = 50;

        foreach (var (c, num) in input)
        {
            current = (current + (c == 'R' ? num : -num)) % 100;

            if (current == 0)
                result++;
        }

        return result;
    }

    private static int Part2(List<(char, int)> data)
    {
        var result = 0;
        var current = 50;

        foreach (var (c, num) in data)
        {
            for (int i = 0; i < num; i++)
            {
                current += (c == 'R' ? 1 : -1);

                if (current % 100 == 0)
                    result++;
            }
        }

        return result;
    }
}
