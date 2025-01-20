using System.Text;

namespace AdventOfCode.Y2022;

internal class Day10() : Solver(2022, 10)
{
    public override void Run()
    {
        var cycles = GetCycles(Input.ReadAllLines());
        var (signalStrengths, letters) = ProcessCycles(cycles);

        Part1Solution = signalStrengths.Where((_, i) => i % 40 == 20).Sum();
        Part2Solution = letters.DecodeAsciiMessage();
    }

    private static List<int> GetCycles(string[] input)
    {
        var cycles = new List<int>();
        foreach (var line in input)
        {
            cycles.Add(0);
            if (line.StartsWith("addx"))
                cycles.Add(int.Parse(line[4..]));
        }

        return cycles;
    }

    private static (List<int> signalStrengths, string letters) ProcessCycles(List<int> cycles)
    {
        var signalStrengths = new List<int>() { 1 };
        var register = 1;
        var crt = new StringBuilder();

        for (int i = 0; i < cycles.Count; i++)
        {
            // Add signal strength for this cycle
            signalStrengths.Add(register * (i + 1));

            // Check if the CRT should draw a pixel for the current column
            var col = i % 40;
            if (col % 40 == 0) crt.AppendLine();
            crt.Append(register.IsBetween(col - 1, col + 1) ? '#' : ' ');

            // Update the register at the end of the cycle
            register += cycles[i];
        }

        return (signalStrengths, crt.ToString());
    }
}
