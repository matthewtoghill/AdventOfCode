using System.Text;
using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;

internal class Day14() : Solver(2024, 14)
{
    private const int MapWidth = 101;
    private const int MapHeight = 103;

    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(string[] input)
    {
        var positions = new List<Position>();
        foreach (var line in input)
        {
            var nums = line.ExtractInts().ToList();
            var newX = (nums[0] + (nums[2] * 100)).Mod(MapWidth);
            var newY = (nums[1] + (nums[3] * 100)).Mod(MapHeight);
            positions.Add(new Position(newX, newY));
        }

        var quadrantWidth = (int)Math.Floor(MapWidth / 2.0);
        var quadrantHeight = (int)Math.Floor(MapHeight / 2.0);

        var topLeft = positions.Count(p => p.X < quadrantWidth && p.Y < quadrantHeight);
        var topRight = positions.Count(p => p.X > quadrantWidth && p.Y < quadrantHeight);
        var bottomLeft = positions.Count(p => p.X < quadrantWidth && p.Y > quadrantHeight);
        var bottomRight = positions.Count(p => p.X > quadrantWidth && p.Y > quadrantHeight);

        return topLeft * topRight * bottomLeft * bottomRight;
    }

    // For my input Part 2 was solvable by looking for the first iteration where the robots were all in unique positions
    // This solution may not work for all inputs and an alternative may be to look for the first iteration where
    // there are a set number of robots adjacent to each other in a line forming the box around the tree
    private static int Part2(string[] input)
    {
        var iterations = 0;
        var robots = new List<Robot>();

        foreach (var line in input)
        {
            var nums = line.ExtractInts().ToList();
            robots.Add(new Robot(new(nums[0], nums[1]), new(nums[2], nums[3])));
        }

        do
        {
            var newRobots = new List<Robot>();
            foreach (var robot in robots)
            {
                var newX = (robot.Position.X + robot.Velocity.X).Mod(MapWidth);
                var newY = (robot.Position.Y + robot.Velocity.Y).Mod(MapHeight);
                newRobots.Add(new Robot(new(newX, newY), robot.Velocity));
            }

            robots = newRobots;
            iterations++;
        }
        while (robots.Select(r => r.Position).ToHashSet().Count != robots.Count);

        //PrintGrid(MapWidth, MapHeight, robots.Select(r => r.Position).ToHashSet());

        return iterations;
    }

    //private static void PrintGrid(int width, int height, HashSet<Position> positions)
    //{
    //    for (int y = 0; y < height; y++)
    //    {
    //        var sb = new StringBuilder();
    //        for (int x = 0; x < width; x++)
    //        {
    //            if (positions.Contains(new(x, y)))
    //                sb.Append('#');
    //            else
    //                sb.Append('.');
    //        }
    //        Console.WriteLine(sb.ToString());
    //    }
    //}
}

internal record Robot(Position Position, Position Velocity);