namespace AdventOfCode.Y2024;

internal class Day17() : Solver(2024, 17)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        var instructions = input[1].ExtractInts().ToList();
        var registers = input[0].ExtractNumeric<long>().ToList();

        Part1Solution = string.Join(",", Part1(instructions, registers));
        Part2Solution = Part2(instructions);
    }

    private static List<int> Part1(List<int> instructions, List<long> registers)
    {
        return RunProgram(registers[0], registers[1], registers[2], instructions);
    }

    private static long Part2(List<int> instructions)
    {
        var queue = new Queue<(long RegisterA, int Offset)>([(0, instructions.Count - 1)]);

        while (queue.TryDequeue(out var current))
        {
            for (int i = 0; i < 8; i++)
            {
                var registerA = (current.RegisterA << 3) + i;
                var result = RunProgram(registerA, 0, 0, instructions);

                if (result.SequenceEqual(instructions[current.Offset..]))
                {
                    if (current.Offset == 0)
                        return registerA;

                    queue.Enqueue((registerA, current.Offset - 1));
                }
            }
        }

        return 0;
    }

    private static List<int> RunProgram(long registerA, long registerB, long registerC, List<int> instructions)
    {
        List<int> outputs = [];

        long Combo(int operand) => operand switch
        {
            4 => registerA,
            5 => registerB,
            6 => registerC,
            _ => operand
        };

        long Dv(int operand) => (long)(registerA / Math.Pow(2, Combo(operand)));

        for (int i = 0; i < instructions.Count; i += 2)
        {
            var instruction = instructions[i];
            var op = instructions[i + 1];

            switch (instruction)
            {
                case 0: registerA = Dv(op);                 break; // Adv
                case 1: registerB ^= op;                    break; // Bxl
                case 2: registerB = Combo(op) % 8;          break; // Bst
                case 3: i = registerA == 0 ? i : op - 2;    break; // Jnz
                case 4: registerB ^= registerC;             break; // Bxc
                case 5: outputs.Add((int)(Combo(op) % 8));  break; // Out
                case 6: registerB = Dv(op);                 break; // Bdv
                case 7: registerC = Dv(op);                 break; // Cdv
            }
        }

        return outputs;
    }
}
