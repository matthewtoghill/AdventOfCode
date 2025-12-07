namespace AdventOfCode.Y2025;

internal class Day06() : Solver(2025, 6)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private long Part1(string[] input)
    {
        var operators = input[^1].RemoveExtraSpaces().Split(" ");
        var results = input[0].ExtractNumeric<long>().ToArray();

        for (int row = 1; row < input.Length- 1; row++)
        {
            var numbers = input[row].ExtractNumeric<long>().ToArray();

            for (int i = 0; i < operators.Length; i++)
            {
                if (operators[i] == "+")
                    results[i] += numbers[i];
                else if (operators[i] == "*")
                    results[i] *= numbers[i];
            }
        }

        return results.Sum();
    }

    private static readonly char[] _operators = ['*', '+'];

    private static long Part2(string[] input)
    {
        var total = 0L;
        var sign = 'x';
        var result = 0L;

        for (int col = 0; col < input[0].Length; col++)
        {
            var buffer = "";
            for (int row = 0; row < input.Length; row++)
            {
                var c = input[row][col];

                if (_operators.Contains(c))
                    sign = c;
                else
                    buffer += c;

            }

            if (string.IsNullOrWhiteSpace(buffer))
            {
                total += result;

                sign = 'x';
                result = 0L;
                continue;
            }

            if (sign == '+')
            {
                result += long.Parse(buffer);
            }
            else if (sign == '*')
            {
                if (result == 0) result = 1;
                result *= long.Parse(buffer);
            }
        }

        return total + result;
    }
}
