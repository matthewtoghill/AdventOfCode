namespace AdventOfCode.Y2023;

internal class Day06() : Solver(2023, 6)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static long Part1(string[] input)
    {
        var times = input[0].ExtractNumeric<long>().ToArray();
        var distances = input[1].ExtractNumeric<long>().ToArray();

        return times.Select((time, i) => CalculateWinningRaces(time, distances[i])).Product();
    }

    private static long Part2(string[] input)
    {
        long maxTime = input[0].Replace(" ", "").ExtractNumeric<long>().First();
        long distance = input[1].Replace(" ", "").ExtractNumeric<long>().First();

        return CalculateWinningRaces(maxTime, distance);
    }

    private static long CalculateWinningRaces(long maxTime, long distance)
    {
        for (long speed = 1; speed < (maxTime / 2); speed++)
        {
            if (speed * (maxTime - speed) > distance)
            {
                return maxTime - (speed * 2) + 1;
            }
        }

        return 0;
    }
}
