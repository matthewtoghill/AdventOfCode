using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;
internal class Day14() : Solver(2022, 14)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(string[] input)
    {
        var cave = GenerateCave(input, false);
        var rocks = cave.Count;
        var floor = cave.Max(p => p.Y);

        // Add sand to the cave, exit when the sand has fallen below the floor height
        while (!AddSand(cave, s => s.Y > floor)) { }

        return cave.Count - rocks;
    }

    private static int Part2(string[] input)
    {
        var cave = GenerateCave(input, true);
        var rocks = cave.Count;
        var source = new Position(500, 0);

        // Add sand to the cave, exit when the cave is 'full' as it contains sand at the source
        while (!AddSand(cave, _ => cave.Contains(source))) { }

        return cave.Count - rocks;
    }

    private static bool AddSand(HashSet<Position> cave, Func<Position, bool> stopAddingSandWhen)
    {
        var sand = new Position(500, 0);

        while (true)
        {
            if (stopAddingSandWhen(sand))
                return true;

            sand += (0, 1); // sand falls down 1 position
            if (!cave.Contains(sand)) continue; // check if position is blocked (exists in cave already)
            if (!cave.Contains(sand + (-1, 0))) { sand += (-1, 0); continue; } // try moving left if blocked below
            if (!cave.Contains(sand + (1, 0))) { sand += (1, 0); continue; }   // try moving right if blocked left and below

            cave.Add(sand + (0, -1)); // add sand to cave at the available position
            break;
        }

        return false; // sand added but condition not met to stop adding sand yet
    }

    private static HashSet<Position> GenerateCave(string[] input, bool includeFloor)
    {
        var cave = new HashSet<Position>();
        foreach (var line in input)
        {
            var points = line.SplitTo<int>(",", " -> ").Chunk(2).ToArray();

            for (int i = 1; i < points.Length; i++)
            {
                int startX = points[i - 1][0];
                int startY = points[i - 1][1];
                int endX = points[i][0];
                int endY = points[i][1];

                if (startX > endX)
                    (startX, endX) = (endX, startX);

                if (startY > endY)
                    (startY, endY) = (endY, startY);

                for (int x = startX; x <= endX; x++)
                    for (int y = startY; y <= endY; y++)
                        cave.Add(new Position(x, y));
            }
        }

        if (includeFloor)
        {
            var minX = cave.Min(p => p.X);
            var maxX = cave.Max(p => p.X);
            var height = cave.Max(p => p.Y) + 2;

            // add floor, use the height to determine the width needed
            for (int x = minX - height; x < maxX + height; x++)
                cave.Add(new Position(x, height));
        }

        return cave;
    }
}
