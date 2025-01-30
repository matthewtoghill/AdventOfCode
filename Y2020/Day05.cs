namespace AdventOfCode.Y2020;

internal class Day05() : Solver(2020, 5)
{
    public override void Run()
    {
        var seatIds = Input.ReadAllLines().Select(GetSeatId).ToList();
        Part1Solution = seatIds.Max();
        Part2Solution = Enumerable.Range(seatIds.Min(), seatIds.Count).Except(seatIds).First();
    }

    private static int GetSeatId(string boardingPass)
        => boardingPass.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1').BinaryToInt();
}
