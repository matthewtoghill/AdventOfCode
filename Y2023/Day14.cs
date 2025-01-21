using AdventOfCode.Tools.Models;
using System.Text;

namespace AdventOfCode.Y2023;

internal class Day14() : Solver(2023, 14)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 1, "N");
        Part2Solution = Solve(input, 1_000_000_000, "NWSE");
    }

    private static int Solve(string[] input, int maxCycles, string directions)
    {
        var map = input.AsCharMap();
        var maxRow = map.Keys.MaxBy(x => x.Row).Row;
        var maxCol = map.Keys.MaxBy(x => x.Col).Col;

        var cycle = 1;
        Dictionary<string, int> cache = [];

        while (cycle <= maxCycles)
        {
            foreach (var direction in directions)
            {
                TiltMap(map, direction, maxRow, maxCol);
            }

            string hash = GetMapHash(map);

            if (cache.TryGetValue(hash, out var value))
            {
                cycle = maxCycles - ((maxCycles - cycle) % (cycle - value));
                cache.Clear();
            }

            cache[hash] = cycle++;
        }

        return CalculateLoad(map, maxRow);
    }

    private static void TiltMap(Dictionary<Position, char> map, char direction, int maxRow, int maxCol)
    {
        if ("SE".Contains(direction))
        {
            foreach (var (pos, rock) in map.Reverse())
            {
                SlideRock(map, direction, maxRow, maxCol, pos, rock);
            }
        }
        else
        {
            foreach (var (pos, rock) in map)
            {
                SlideRock(map, direction, maxRow, maxCol, pos, rock);
            }
        }
    }

    private static void SlideRock(Dictionary<Position, char> map, char direction, int maxRow, int maxCol, Position pos, char rock)
    {
        if (rock != 'O') return;

        var currentPos = pos;
        while (true)
        {
            var newPos = currentPos.MoveInDirection(direction);
            if (direction == 'N' && newPos.Row < 0) return;
            if (direction == 'W' && newPos.Col < 0) return;
            if (direction == 'S' && newPos.Row > maxRow) return;
            if (direction == 'E' && newPos.Col > maxCol) return;
            if (map.TryGetValue(newPos, out var nextRock) && "O#".Contains(nextRock)) return;

            map[newPos] = 'O';
            map[currentPos] = '.';
            currentPos = newPos;
        }
    }

    private static string GetMapHash(Dictionary<Position, char> rocks)
    {
        var sb = new StringBuilder();
        rocks.Values.ForEach(x => sb.Append(x));
        return sb.ToString();
    }

    private static int CalculateLoad(Dictionary<Position, char> rocks, int maxRow)
        => rocks.WhereValues(x => x == 'O')
                .Select(rock => rock.Key.Row)
                .GetFrequencies()
                .Sum(row => (maxRow + 1 - row.Key) * row.Value);
}
