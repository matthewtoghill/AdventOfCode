namespace AdventOfCode.Y2023;

internal class Day05() : Solver(2023, 5)
{
    private record CategoryMap(long DestinationStart, long SourceStart, long Length, long Difference);
    private record SeedRange(long Start, long End);

    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static long Part1(string[] input) => GetLowestLocation(input[0].ExtractNumeric<long>().ToArray(), GetMaps(input));

    private static long Part2(string[] input)
    {
        var seeds = input[0].ExtractNumeric<long>().Chunk(2).Select(x => new SeedRange(x[0], x[0] + x[1])).ToArray();
        var maps = GetMaps(input);
        var candidateSeeds = new List<long>();

        for (int i = 0; i < maps.Count; i++)
        {
            var map = maps[i];
            foreach (var item in map)
            {
                var current = item.SourceStart;
                for (int j = i - 1; j >= 0; j--)
                {
                    current = ApplyReverseMap(current, maps[j]);
                }
                candidateSeeds.Add(current);
            }
        }

        return GetLowestLocation(candidateSeeds.Where(x => seeds.Any(s => x.IsBetween(s.Start, s.End))).ToArray(), maps);
    }

    private static long GetLowestLocation(long[] seeds, List<CategoryMap[]> maps)
    {
        var lowestLocation = long.MaxValue;

        foreach (var seed in seeds)
        {
            var current = seed;
            maps.ForEach(x => current = ApplyMap(current, x));
            lowestLocation = Math.Min(current, lowestLocation);
        }

        return lowestLocation;
    }

    private static List<CategoryMap[]> GetMaps(string[] input) => input.Skip(1).Select(GetMap).ToList();

    private static CategoryMap[] GetMap(string text)
        => text.SplitLines()
               .Skip(1)
               .Select(x => x.ExtractNumeric<long>().ToArray())
               .Select(x => new CategoryMap(x[0], x[1], x[2], x[0] - x[1]))
               .ToArray();

    private static long ApplyMap(long source, CategoryMap[] map)
    {
        foreach (var item in map)
        {
            if (source >= item.SourceStart && source < item.SourceStart + item.Length)
                return source + item.Difference;
        }

        return source;
    }

    private static long ApplyReverseMap(long destination, CategoryMap[] map)
    {
        foreach (var item in map)
        {
            if (destination >= item.DestinationStart && destination < item.DestinationStart + item.Length)
                return destination - item.Difference;
        }
        return destination;
    }
}
