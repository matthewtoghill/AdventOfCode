using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day04() : GridSolver(2024, 4)
{
    public override void Run()
    {
        Part1Solution = Part1();
        Part2Solution = Part2();
    }

    private int Part1()
    {
        var count = 0;

        for (int row = 0; row < Grid.Length; row++)
        {
            for (int col = 0; col < Grid[row].Length; col++)
            {
                if (Grid[row][col] != 'X')
                    continue;

                var current = new Position(col, row);
                foreach (var direction in Position.AllDirections)
                {
                    var pos2 = current + direction;
                    var pos3 = pos2 + direction;
                    var pos4 = pos3 + direction;

                    if (pos4.IsOutsideBounds(MinPos, MaxPos))
                        continue;

                    if (new[] { pos2, pos3, pos4 }.Select(GetLetter).SequenceEqual("MAS"))
                        count++;
                }
            }
        }

        return count;
    }

    private int Part2()
    {
        var count = 0;

        for (int row = 0; row < Grid.Length; row++)
        {
            for (int col = 0; col < Grid[row].Length; col++)
            {
                if (Grid[row][col] != 'A')
                    continue;

                var current = new Position(col, row);

                var NW = current + (-1, -1);
                var SE = current + (1, 1);

                var SW = current + (1, -1);
                var NE = current + (-1, 1);

                if (new[] { NW, SE, SW, NE }.Any(p => p.IsOutsideBounds(MinPos, MaxPos)))
                    continue;

                var textA = $"{GetLetter(NW)}{GetLetter(SE)}";
                var textB = $"{GetLetter(SW)}{GetLetter(NE)}";

                if ((textA == "MS" || textA == "SM") && (textB == "MS" || textB == "SM"))
                    count++;
            }
        }

        return count;
    }

    private char GetLetter(Position pos) => GridMap[pos];
}
