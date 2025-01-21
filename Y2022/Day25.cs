using System.Text;

namespace AdventOfCode.Y2022;

internal class Day25() : Solver(2022, 25)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = LongToSnafu(input.Sum(SnafuToLong));
        Part2Solution = "Happy Christmas!";
    }

    private static long SnafuToLong(string snafu) => snafu.Aggregate(0L, (result, c) => (result * 5) + SnafuDigitToDecimal(c));

    private static string LongToSnafu(long number)
    {
        var sb = new StringBuilder();
        while (number > 0)
        {
            var digit = number % 5;
            if (digit.IsAnyOf(3, 4)) digit -= 5;
            number = (number - digit) / 5;
            sb.Insert(0, DecimalToSnafuDigit(digit));
        }
        return sb.ToString();
    }

    private static long SnafuDigitToDecimal(char snafuDigit)
        => snafuDigit switch
        {
            '2' => 2,
            '1' => 1,
            '0' => 0,
            '-' => -1,
            '=' => -2,
            _ => throw new NotSupportedException()
        };

    private static char DecimalToSnafuDigit(long d)
        => d switch
        {
            2 => '2',
            1 => '1',
            0 => '0',
            -1 => '-',
            -2 => '=',
            _ => throw new NotSupportedException()
        };
}
