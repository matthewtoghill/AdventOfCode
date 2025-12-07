namespace AdventOfCode.Y2020;

internal class Day13() : Solver(2020, 13)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = FindEarliestBus(int.Parse(input[0]), [.. input[1].ExtractInts()]);
        Part2Solution = FindEarliestTimestamp(input[1]);
    }

    private static int FindEarliestBus(int earliest, int[] buses)
        => buses.Select(busId => (busId, wait: busId - (earliest % busId)))
                .MinBy(x => x.wait)
                .Map(x => x.busId * x.wait);

    private static long FindEarliestTimestamp(string input)
    {
        var busesWithOffsets = input.Split(',')
                                    .Select((busId, offset) => (busId, offset))
                                    .Where(x => x.busId != "x")
                                    .Select(x => (long.Parse(x.busId), x.offset))
                                    .ToArray();

        long timestamp = 0, step = 1;

        foreach (var (bus, offset) in busesWithOffsets)
        {
            while ((timestamp + offset) % bus != 0)
            {
                timestamp += step;
            }
            step *= bus;
        }

        return timestamp;
    }
}
