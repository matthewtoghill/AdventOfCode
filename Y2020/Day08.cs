namespace AdventOfCode.Y2020;

internal class Day08() : Solver(2020, 8)
{
    private record Instruction(string Operation, int Argument);

    public override void Run()
    {
        var instructions = Input.ReadAllLines().Select(ParseInstruction).ToArray();
        Part1Solution = RunProgram(instructions).Accumulator;
        Part2Solution = FixProgram(instructions);
    }

    private static Instruction ParseInstruction(string line)
    {
        var split = line.Split(' ');
        return new(split[0], int.Parse(split[1]));
    }

    private static (int Accumulator, bool) RunProgram(Instruction[] instructions)
    {
        var accumulator = 0;
        var pointer = 0;
        HashSet<int> visited = [];

        while (pointer < instructions.Length)
        {
            if (!visited.Add(pointer))
                return (accumulator, false);

            var (op, arg) = instructions[pointer];
            switch (op)
            {
                case "acc":
                    accumulator += arg;
                    pointer++;
                    break;
                case "jmp":
                    pointer += arg;
                    break;
                case "nop":
                    pointer++;
                    break;
            }
        }

        return (accumulator, true);
    }

    private static int FixProgram(Instruction[] instructions)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            var (op, arg) = instructions[i];
            if (op == "acc")
                continue;

            instructions[i] = new(op == "jmp" ? "nop" : "jmp", arg);
            var (acc, terminated) = RunProgram(instructions);
            if (terminated)
                return acc;

            instructions[i] = new(op, arg);
        }

        return -1;
    }
}
