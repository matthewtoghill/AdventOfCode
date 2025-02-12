using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020;

internal partial class Day07() : Solver(2020, 7)
{
    private record Bag(string Name, int Quantity);

    public override void Run()
    {
        var (bagContents, bagParents) = ParseBagRules(Input.ReadAllLines());
        Part1Solution = FindBagsThatCanContain("shiny gold", bagParents).Count - 1;
        Part2Solution = CountBagsWithin("shiny gold", 1, bagContents) - 1;
    }

    private static (Dictionary<string, List<Bag>>, DefaultDictionary<string, HashSet<string>>) ParseBagRules(string[] input)
    {
        Dictionary<string, List<Bag>> bagContents = [];
        DefaultDictionary<string, HashSet<string>> bagParents = new(() => []);

        foreach (var line in input)
        {
            var split = line.StripOut("bags", "bag", ".").Split("contain", StringSplitOptions.RemoveEmptyEntries);
            var bag = split[0].Trim();
            bagContents[bag] = BagContentRegex()
                .Matches(split[1])
                .Select(x => new Bag(x.Groups["name"].Value, int.Parse(x.Groups["quantity"].Value)))
                .ToList();

            foreach (var item in bagContents[bag])
            {
                bagParents[item.Name].Add(bag);
            }
        }

        return (bagContents, bagParents);
    }

    private static long CountBagsWithin(string bag, int quantity, Dictionary<string, List<Bag>> bags)
        => bags.TryGetValue(bag, out var contents)
            ? quantity + contents.Sum(x => CountBagsWithin(x.Name, x.Quantity, bags) * quantity)
            : 0;

    private static HashSet<string> FindBagsThatCanContain(string bag, DefaultDictionary<string, HashSet<string>> bagParents)
    {
        Queue<string> queue = new([bag]);
        HashSet<string> bags = [];

        while (queue.TryDequeue(out var current))
        {
            bags.Add(current);

            if (bagParents.TryGetValue(current, out var parents))
                queue.EnqueueRange(parents);
        }

        return bags;
    }

    [GeneratedRegex("(?<quantity>\\d+) (?<name>[a-z]+ [a-z]+)")]
    private static partial Regex BagContentRegex();
}
