namespace AdventOfCode.Y2022;

internal class Day11() : Solver(2022, 11)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static long Part1(string[] input)
    {
        var monkeys = ParseMonkeys(input);
        return MonkeyBusiness(monkeys, 20, x => x / 3);
    }

    private static long Part2(string[] input)
    {
        var monkeys = ParseMonkeys(input);
        var reliefValue = monkeys.Select(x => x.Test).Product();
        return MonkeyBusiness(monkeys, 10_000, x => x % reliefValue);
    }

    private static long MonkeyBusiness(List<Monkey> monkeys, int rounds, Func<long, long> reliefCalculation)
    {
        for (int r = 0; r < rounds; r++)
        {
            monkeys.AsParallel().ForEach(monkey =>
            {
                while (monkey.Items.Count > 0)
                {
                    var item = monkey.Items.Dequeue();
                    monkey.ItemsInspected++;
                    item = monkey.Operation(item);
                    item = reliefCalculation(item);
                    monkeys[item % monkey.Test == 0 ? monkey.TrueIndex : monkey.FalseIndex].Items.Enqueue(item);
                }
            });
        }

        return monkeys.Select(x => x.ItemsInspected).OrderDescending().Take(2).Product();
    }

    private static List<Monkey> ParseMonkeys(string[] input)
    {
        var monkeys = new List<Monkey>();
        foreach (var monkey in input)
        {
            var split = monkey.SplitLines();
            var items = split[1].ExtractNumeric<long>().ToArray();

            Func<long, long> operation = split[2][19..].Split() switch
            {
                ["old", "*", "old"] => x => x * x,
                ["old", "+", "old"] => x => x + x,
                ["old", "+", var num] => x => x + int.Parse(num),
                ["old", "*", var num] => x => x * int.Parse(num),
                _ => throw new Exception()
            };

            var test = split[3].ExtractInts().First();
            var trueIndex = split[4].ExtractInts().First();
            var falseIndex = split[5].ExtractInts().First();

            monkeys.Add(new Monkey(items, operation, test, trueIndex, falseIndex));
        }

        return monkeys;
    }

    private class Monkey(long[] items, Func<long, long> operation, int test, int trueIndex, int falseIndex)
    {
        public Queue<long> Items { get; set; } = new Queue<long>(items);
        public Func<long, long> Operation { get; set; } = operation;
        public int Test { get; set; } = test;
        public int TrueIndex { get; set; } = trueIndex;
        public int FalseIndex { get; set; } = falseIndex;
        public long ItemsInspected { get; set; } = 0;
    }
}
