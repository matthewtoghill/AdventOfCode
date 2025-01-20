using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2024;
internal class Day12() : Solver(2024, 12)
{
    public override void Run()
    {
        var regions = GetAllRegions(Input.ReadAllLines().AsCharMap());
        Part1Solution = regions.Sum(x => x.Count * x.CountPerimeter());
        Part2Solution = regions.Sum(x => x.Count * x.CountCorners());
    }

    private static List<HashSet<Position>> GetAllRegions(Dictionary<Position, char> map)
    {
        HashSet<Position> visited = [];
        List<HashSet<Position>> regions = [];

        foreach (var (position, _) in map)
        {
            if (visited.Contains(position)) continue;

            var region = GetRegion(map, position);
            regions.Add(region);
            region.ForEach(p => visited.Add(p));
        }

        return regions;
    }

    private static HashSet<Position> GetRegion(Dictionary<Position, char> map, Position start)
    {
        HashSet<Position> region = [];
        Queue<Position> queue = new([start]);
        var key = map[start];

        while (queue.TryDequeue(out var current))
        {
            if (!region.Add(current)) continue;

            current.GetNeighbours().ForEach(n =>
            {
                if (map.GetValueOrDefault(n) == key)
                    queue.Enqueue(n);
            });
        }

        return region;
    }
}
