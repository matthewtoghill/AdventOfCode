namespace AdventOfCode.Y2025;

internal class Day03() : Solver(2025, 3)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 2);
        Part2Solution = Solve(input, 12);
    }

    private static long Solve(string[] input, int batteryCapacity)
    {
        long result = 0;

        foreach (var line in input)
        {
            List<char> batteries = [];

            var lastBatteryIndex = -1;
            while (batteries.Count < batteryCapacity)
            {
                var currentBattery = '1';
                for (int i = lastBatteryIndex + 1; i < line.Length; i++)
                {
                    // ensure there are enough characters left to fill the remaining batteries
                    if ((line.Length - i) < (batteryCapacity - batteries.Count))
                        break;

                    if (line[i] > currentBattery)
                    {
                        currentBattery = line[i];
                        lastBatteryIndex = i;
                    }
                }

                batteries.Add(currentBattery);
            }

            result += batteries.ParseTo<long>();
        }

        return result;
    }
}
