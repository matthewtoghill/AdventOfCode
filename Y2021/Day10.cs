namespace AdventOfCode.Y2021;

internal class Day10() : Solver(2021, 10)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = input.Sum(x => CalculateScore(x, true));
        Part2Solution = input.Select(x => CalculateScore(x, false)).Where(x => x > 0).Median();
    }

    private static long CalculateScore(string line, bool scoreSyntaxError)
    {
        var stack = new Stack<char>();

        foreach (var item in line)
        {
            switch ((stack.FirstOrDefault(), item))
            {
                case ('(', ')') or ('[', ']') or ('{', '}') or ('<', '>'):
                    stack.Pop();
                    break;
                case (_, var c) when ")]}>".Contains(c):
                    return scoreSyntaxError ? GetSyntaxErrorScore(c) : 0;
                default:
                    stack.Push(item);
                    break;
            }
        }

        return scoreSyntaxError ? 0 : stack.Aggregate(0L, (total, c) => (total * 5) + 1 + "([{<".IndexOf(c));
    }

    private static int GetSyntaxErrorScore(char c)
        => c switch
        {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => 0,
        };
}
