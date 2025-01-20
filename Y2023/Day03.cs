using System.Text;
using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2023;

internal class Day03() : Solver(2023, 3)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(string[] input)
    {
        var partNumbers = new List<int>();
        HashSet<Position> checkedPositions = [];

        for (int row = 0; row < input.Length; row++)
        {
            for (int col = 0; col < input[row].Length; col++)
            {
                var c = input[row][col];
                if (c == '.' || char.IsDigit(c))
                    continue;

                foreach (var (nRow, nCol) in new Position(row, col).GetNeighbours(true))
                {
                    if (!char.IsDigit(input[nRow][nCol]))
                        continue;

                    var partNumber = RetrieveNumber(input[nRow], nRow, nCol, checkedPositions);

                    if (partNumber > 0)
                        partNumbers.Add(partNumber);
                }
            }
        }

        return partNumbers.Sum();
    }

    private static int Part2(string[] input)
    {
        var gearRatios = new List<int>();
        HashSet<Position> checkedPositions = [];

        for (int row = 0; row < input.Length; row++)
        {
            for (int col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] != '*')
                    continue;

                var partNumbers = new List<int>();

                foreach (var (nRow, nCol) in new Position(row, col).GetNeighbours(true))
                {
                    if (!char.IsDigit(input[nRow][nCol]))
                        continue;

                    var partNumber = RetrieveNumber(input[nRow], nRow, nCol, checkedPositions);

                    if (partNumber > 0)
                        partNumbers.Add(partNumber);
                }

                if (partNumbers.Count == 2)
                    gearRatios.Add(partNumbers.Product());
            }
        }

        return gearRatios.Sum();
    }

    private static int RetrieveNumber(string line, int row, int col, HashSet<Position> checkedPositions)
    {
        var result = new StringBuilder();

        var newCol = col;
        while (newCol >= 0 && char.IsDigit(line[newCol]))
        {
            Position p = new(newCol, row);
            if (checkedPositions.Contains(p)) return 0;
            result.Insert(0, line[newCol]);
            checkedPositions.Add(p);
            newCol--;
        }

        newCol = col + 1;
        while (newCol < line.Length && char.IsDigit(line[newCol]))
        {
            Position p = new(newCol, row);
            if (checkedPositions.Contains(p)) return 0;
            result.Append(line[newCol]);
            checkedPositions.Add(p);
            newCol++;
        }

        return int.Parse(result.ToString());
    }
}
