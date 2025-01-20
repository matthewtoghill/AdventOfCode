using System.Text.Json.Nodes;

namespace AdventOfCode.Y2022;

internal class Day13() : Solver(2022, 13)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static IEnumerable<JsonNode?> GetPackets(string[] input)
        => input.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => JsonNode.Parse(x));

    private static int Part1(string[] input)
    {
        var packetPairs = GetPackets(input).Chunk(2).ToList();
        var result = 0;

        for (int i = 0; i < packetPairs.Count; i++)
            result += CompareNodes(packetPairs[i][0]!, packetPairs[i][1]!) < 0 ? i + 1 : 0;

        return result;
    }

    private static int Part2(string[] input)
    {
        var packets = GetPackets(input).ToList();

        var firstDivider = JsonNode.Parse("[[2]]");
        var secondDivider = JsonNode.Parse("[[6]]");

        packets.AddRange([firstDivider, secondDivider]);

        packets.Sort(CompareNodes!);

        var firstIndex = packets.IndexOf(firstDivider) + 1;
        var secondIndex = packets.IndexOf(secondDivider) + 1;

        return firstIndex * secondIndex;
    }

    private static int CompareNodes(JsonNode left, JsonNode right)
        => (left, right) switch
        {
            (JsonValue x, JsonValue y) => (int)x - (int)y,
            (JsonValue, JsonArray y) => CompareNodes(new JsonArray((int)left), y),
            (JsonArray x, JsonValue) => CompareNodes(x, new JsonArray((int)right)),
            (JsonNode x, JsonNode y) => x.AsArray().Zip(y.AsArray())
                                                   .Select(a => CompareNodes(a.First!, a.Second!))
                                                   .FirstOrDefault(b => b != 0, x.AsArray().Count - y.AsArray().Count)
        };
}
