namespace AdventOfCode.Y2020;

internal class Day14() : Solver(2020, 14)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, true);
        Part2Solution = Solve(input, false);
    }

    private static long Solve(string[] input, bool maskValues)
    {
        var memory = new Dictionary<long, long>();
        var mask = string.Empty;

        foreach (var line in input)
        {
            if (line.StartsWith("mask"))
            {
                mask = line.Split(" = ")[1];
                continue;
            }

            var parts = line.Split(['[', ']', ' ', '='], StringSplitOptions.RemoveEmptyEntries);
            var address = long.Parse(parts[1]);
            var value = long.Parse(parts[2]);

            if (maskValues)
            {
                memory[address] = ApplyMaskToValue(mask, value);
            }
            else
            {
                foreach (var addr in GetAllAddresses(mask, address))
                {
                    memory[addr] = value;
                }
            }
        }

        return memory.Values.Sum();
    }

    private static long ApplyMaskToValue(string mask, long value)
    {
        var binary = Convert.ToString(value, 2).PadLeft(36, '0');
        var result = string.Empty;

        for (int i = 0; i < 36; i++)
        {
            result += mask[i] == 'X' ? binary[i] : mask[i];
        }

        return Convert.ToInt64(result, 2);
    }

    private static List<long> GetAllAddresses(string mask, long address)
    {
        var binary = Convert.ToString(address, 2).PadLeft(36, '0');
        var masked = new char[36];

        for (int i = 0; i < 36; i++)
        {
            masked[i] = mask[i] switch
            {
                '0' => binary[i],
                '1' => '1',
                _ => 'X'
            };
        }

        return ExpandFloatingBits(new string(masked));
    }

    private static List<long> ExpandFloatingBits(string address)
    {
        if (!address.Contains('X'))
            return [Convert.ToInt64(address, 2)];

        var result = new List<long>();
        var index = address.IndexOf('X');

        result.AddRange(ExpandFloatingBits(address[..index] + '0' + address[(index + 1)..]));
        result.AddRange(ExpandFloatingBits(address[..index] + '1' + address[(index + 1)..]));

        return result;
    }
}
