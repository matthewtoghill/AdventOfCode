using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;

internal class Day18() : Solver(2022, 18)
{
    public override void Run()
    {
        var cubes = GetCubes(Input.ReadAllLines());
        Part1Solution = Part1(cubes);
        Part2Solution = Part2(cubes);
    }

    private static int Part1(HashSet<Position3D> cubes) => (cubes.Count * 6) - cubes.SelectMany(c => c.GetNeighbours()).Count(cubes.Contains);

    private static int Part2(HashSet<Position3D> cubes)
    {
        var minRange = new Position3D(cubes.Min(c => c.X) - 1, cubes.Min(c => c.Y) - 1, cubes.Min(c => c.Z) - 1);
        var maxRange = new Position3D(cubes.Max(c => c.X) + 1, cubes.Max(c => c.Y) + 1, cubes.Max(c => c.Z) + 1);
        var water = FloodFill(cubes, minRange, maxRange);
        return cubes.SelectMany(c => c.GetNeighbours()).Count(water.Contains);
    }

    private static HashSet<Position3D> FloodFill(HashSet<Position3D> cubes, Position3D start, Position3D end)
    {
        HashSet<Position3D> result = [];
        Queue<Position3D> queue = new([start]);
        result.Add(start);

        while (queue.TryDequeue(out var current))
        {
            foreach (var neighbour in current.GetNeighbours())
            {
                if (!result.Contains(neighbour) && !cubes.Contains(neighbour) && neighbour.IsBetween(start, end))
                {
                    result.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }

        return result;
    }

    private static HashSet<Position3D> GetCubes(string[] input)
        => input.Select(line =>
        {
            var nums = line.ExtractInts().ToArray();
            return new Position3D(nums[0], nums[1], nums[2]);
        }).ToHashSet();
}
