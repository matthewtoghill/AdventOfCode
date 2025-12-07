namespace AdventOfCode.Y2020;

internal class Day15() : Solver(2020, 15)
{
    public override void Run()
    {
        var input = Input.ReadAll().ExtractInts().ToArray();
        Part1Solution = PlayMemoryGame(input, 2020);
        Part2Solution = PlayMemoryGame(input, 30_000_000);
    }

    private static int PlayMemoryGame(int[] startingNumbers, int turns)
    {
        var memory = new int[turns];
        for (int i = 0; i < startingNumbers.Length - 1; i++)
        {
            memory[startingNumbers[i]] = i + 1;
        }

        int lastNumber = startingNumbers[^1];

        for (int i = startingNumbers.Length; i < turns; i++)
        {
            var number = memory[lastNumber];
            memory[lastNumber] = i;
            lastNumber = number == 0 ? 0 : i - number;
        }

        return lastNumber;
    }
}
