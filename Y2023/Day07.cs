namespace AdventOfCode.Y2023;

internal class Day07() : Solver(2023, 7)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, "23456789TJQKA", false);
        Part2Solution = Solve(input, "J23456789TQKA", true);
    }

    private static int Solve(string[] input, string cardPriority, bool hasJokers)
        => input.Select(x => ParseHand(x.Split(), cardPriority, hasJokers))
                .Order()
                .Select((hand, rank) => hand.Bid * (rank + 1))
                .Sum();

    private static Hand ParseHand(string[] lineSplit, string cardPriority, bool hasJokers)
        => new(lineSplit[0], int.Parse(lineSplit[1]), cardPriority, hasJokers);

    private class Hand : IComparable<Hand>
    {
        public string CardPriority { get; }
        public string Cards { get; }
        public int Bid { get; }
        public Dictionary<char, int> CardFrequencies { get; private set; }
        public int HandScore { get; private set; }

        public Hand(string cards, int bid, string cardPriority, bool hasJokers)
        {
            Cards = cards;
            Bid = bid;
            CardPriority = cardPriority;
            CardFrequencies = cards.GetFrequencies();

            if (hasJokers && CardFrequencies.ContainsKey('J') && CardFrequencies.Count != 1)
            {
                var mostFrequent = CardFrequencies.Where(x => x.Key != 'J').MaxBy(x => x.Value).Key;
                var updatedCards = cards.Replace('J', mostFrequent);
                CardFrequencies = updatedCards.GetFrequencies();
            }

            HandScore = GetHandScore();
        }

        public int CompareTo(Hand? other)
        {
            if (other is null) return 1;

            if (HandScore > other.HandScore) return 1;
            if (HandScore < other.HandScore) return -1;

            for (int i = 0; i < Cards.Length; i++)
            {
                if (CardPriority.IndexOf(Cards[i]) > CardPriority.IndexOf(other.Cards[i])) return 1;
                if (CardPriority.IndexOf(Cards[i]) < CardPriority.IndexOf(other.Cards[i])) return -1;
            }

            return 0;
        }

        private int GetHandScore()
            => CardFrequencies.Values.OrderDescending().ToArray() switch
            {
                [5] => 6,           // Five of a Kind
                [4, 1] => 5,        // Four of a Kind
                [3, 2] => 4,        // Full House
                [3, 1, 1] => 3,     // Three of a Kind
                [2, 2, 1] => 2,     // Two Pair
                [2, 1, 1, 1] => 1,  // Pair
                _ => 0              // High Card
            };
    }
}