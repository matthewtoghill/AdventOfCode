namespace AdventOfCode.Y2023;

internal class Day04() : Solver(2023, 4)
{
    public override void Run()
    {
        var matches = Input.ReadAllLines().Select(ParseCard).ToArray();
        Part1Solution = Part1(matches);
        Part2Solution = Part2(matches);
    }

    private static int ParseCard(string line)
    {
        var split = line.Split(["Card", ":", "|"], StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
        var winningNumbers = split[0].SplitTo<int>(" ");
        var cardNumbers = split[1].SplitTo<int>(" ");
        return cardNumbers.Intersect(winningNumbers).Count();
    }

    private static int Part1(int[] matches) => matches.Sum(x => (int)Math.Pow(2, x - 1));

    private static int Part2(int[] matches)
    {
        var cards = new Dictionary<int, int>();
        for (int i = 0; i < matches.Length; i++)
        {
            cards.IncrementAt(i);
            Enumerable.Range(1, matches[i]).ForEach(j => cards.IncrementAt(i + j, cards[i]));
        }

        return cards.Values.Sum();
    }
}
