using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day08() : GridSolver(2024, 8)
{
    //private string[] _input { get; init; }
    //private Position MinPos { get; init; } = new(0, 0);
    //private Position MaxPos { get; init; }

    //public Day08() : base(2024, 8)
    //{
    //    _input = Input.ReadAllLines();
    //    MaxPos = new(_input[0].Length - 1, _input.Length - 1);
    //}

    public override void Run()
    {
        var antennas = GetAntennas(Grid);
        Part1Solution = Part1(antennas);
        Part2Solution = Part2(antennas);
    }

    private int Part1(HashSet<Antenna> antennas)
    {
        HashSet<Position> antinodes = [];

        var grouped = antennas.GroupBy(x => x.Frequency).ToDictionary(x => x.Key, x => x.Select(y => y.Position).ToList());

        foreach (var (_, positions) in grouped)
        {
            foreach (var pair in positions.GetPermutations(2).Select(x => x.ToList()))
            {
                var antinode = GetAntinode(pair[0], pair[1]);

                if (antinode.IsBetween(MinPos, MaxPos))
                    antinodes.Add(antinode);
            }
        }

        return antinodes.Count;
    }

    private int Part2(HashSet<Antenna> antennas)
    {
        HashSet<Position> antinodes = [];

        var grouped = antennas.GroupBy(x => x.Frequency).ToDictionary(x => x.Key, x => x.Select(y => y.Position).ToList());

        foreach (var (_, positions) in grouped)
        {
            foreach (var pair in positions.GetPermutations(2).Select(x => x.ToList()))
            {
                GetAllAntinodes(pair[0], pair[1]).ForEach(x => antinodes.Add(x));
            }
        }

        return antinodes.Count;
    }

    private static HashSet<Antenna> GetAntennas(string[] input)
        => input.IterateGrid((row, col, c) => new Antenna(new(col, row), c), c => c != '.').ToHashSet();

    public static Position GetAntinode(Position start, Position end)
    {
        var distance = start.DirectDistance(end);
        double directionX = (end.X - start.X) / distance;
        double directionY = (end.Y - start.Y) / distance;

        double newX = end.X + (directionX * distance);
        double newY = end.Y + (directionY * distance);

        return new Position((int)newX, (int)newY);
    }

    public List<Position> GetAllAntinodes(Position start, Position end)
    {
        List<Position> positions = [start, end];

        while (true)
        {
            var nextPos = GetAntinode(start, end);
            if (nextPos.IsOutsideBounds(MinPos, MaxPos))
                break;

            positions.Add(nextPos);
            start = end;
            end = nextPos;
        }

        return positions;
    }
}

internal record Antenna(Position Position, char Frequency);