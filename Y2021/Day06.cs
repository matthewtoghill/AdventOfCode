namespace AdventOfCode.Y2021;

internal class Day06() : Solver(2021, 6)
{
    public override void Run()
    {
        var fish = new long[9];
        Input.ReadAll().ExtractInts().GetFrequencies().ForEach(x => fish[x.Key] = x.Value);
        Part1Solution = SimulateDays(fish, 80);
        Part2Solution = SimulateDays(fish, 256);
    }

    private static long SimulateDays(long[] fish, int days)
    {
        for (int day = 0; day < days; day++)
        {
            var newFish = new long[9];
            for (int i = 0; i < 8; i++)
            {
                newFish[i] = fish[i + 1];
            }
            newFish[8] = fish[0];
            newFish[6] += fish[0];

            fish = newFish.ToArray();
        }

        return fish.Sum();
    }
}
