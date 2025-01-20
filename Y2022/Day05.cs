namespace AdventOfCode.Y2022;

internal class Day05() : Solver(2022, 5)
{
    private record Instruction(int Quantity, int From, int To);

    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        var instructions = GetInstructions(input);

        Part1Solution = Part1(input, instructions);
        Part2Solution = Part2(input, instructions);
    }

    private static string Part1(string[] input, List<Instruction> instructions)
    {
        var stacks = GetCrateStacks(input[0].SplitLines());

        foreach (var (quantity, from, to) in instructions)
        {
            for (int i = 0; i < quantity; i++)
            {
                stacks[to].Push(stacks[from].Pop());
            }
        }

        return new string(stacks.Select(x => x.Peek()).ToArray());
    }

    private static string Part2(string[] input, List<Instruction> instructions)
    {
        var stacks = GetCrateStacks(input[0].SplitLines());

        foreach (var (quantity, from, to) in instructions)
        {
            var tempStack = new Stack<char>();
            for (int i = 0; i < quantity; i++)
                tempStack.Push(stacks[from].Pop());

            var count = tempStack.Count;
            for (int i = 0; i < count; i++)
                stacks[to].Push(tempStack.Pop());
        }

        return new string(stacks.Select(x => x.Peek()).ToArray());
    }

    private static List<Stack<char>> GetCrateStacks(string[] cratesInput)
    {
        var stackTotal = cratesInput[^1].ExtractInts().Max();
        var stacks = Enumerable.Range(0, stackTotal).Select(_ => new Stack<char>()).ToList();

        for (int i = cratesInput.Length - 2; i >= 0; i--)
        {
            var crates = cratesInput[i];
            var stackIndex = 0;
            for (int j = 1; j < crates.Length; j += 4)
            {
                if (char.IsLetter(crates[j]))
                    stacks[stackIndex].Push(crates[j]);

                stackIndex++;
            }
        }

        return stacks;
    }

    private static List<Instruction> GetInstructions(string[] input)
        => input[1].SplitLines().Select(line => {
            var nums = line.ExtractInts().ToArray();
            return new Instruction(nums[0], nums[1] - 1, nums[2] - 1);
        }).ToList();
}
