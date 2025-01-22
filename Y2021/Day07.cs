namespace AdventOfCode.Y2021;

internal class Day07() : Solver(2021, 7)
{
    public override void Run()
    {
        var crabs = Input.ReadAll().ExtractInts().ToArray();
        Part1Solution = CalculateMinFuel(crabs, x => x);                // cost is linear
        Part2Solution = CalculateMinFuel(crabs, x => x * (x + 1) / 2);  // cost is triangular n(n+1)/2
    }

    private delegate int FuelCostCalculation(int distance);
    private static int CalculateMinFuel(int[] crabs, FuelCostCalculation calc)
    {
        int minFuelCost = int.MaxValue;

        for (int i = crabs.Min(); i <= crabs.Max(); i++)
        {
            minFuelCost = Math.Min(minFuelCost, crabs.Sum(c => calc(Math.Abs(c - i))));
        }

        return minFuelCost;
    }
}
