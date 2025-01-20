namespace AdventOfCode.Y2022;

internal class Day21() : Solver(2022, 21)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static double Part1(string[] input)
    {
        var dict = input.Select(x => x.Split(": ")).ToDictionary(x => x[0], x => x[1]);
        return EvaluateCommand("root", ref dict);
    }

    private static double Part2(string[] input)
    {
        var dict = input.Select(x => x.Split(": ")).ToDictionary(x => x[0], x => x[1]);
        var rootLeft = dict["root"].Split(" ")[0];
        var rootRight = dict["root"].Split(" ")[2];
        var humanVal = double.Parse(dict["humn"]);
        double increment = 1;
        Func<double, double> getNextIncrementValue = x => x * 2; // Function for increasing the increment value
        var history = new List<(double HumanVal, double Diff)>();

        while (true)
        {
            // Evaluate the left and  right sides of the root equation with the new human value
            var leftVal = EvaluateCommand(rootLeft, ref dict, humanVal);
            var rightVal = EvaluateCommand(rootRight, ref dict, humanVal);

            // Once left and right sides of the root equation are equal, break out and return the human value
            if (leftVal == rightVal)
                break;

            // Update the history with the human value and the difference between the left and right values
            history.Add((humanVal, Math.Abs(rightVal - leftVal)));

            // If the differences stop getting smaller then the increment has gone beyond the required human value
            // Go back 2 steps in the history and reset the increment to 1
            if (history.Count > 2 && history[^1].Diff > history[^2].Diff)
            {
                humanVal = history[^3].HumanVal;
                increment = 1;

                // When the history is small enough on each reset, change the increment function to only increase by 1
                if (history.Count < 5)
                    getNextIncrementValue = x => x + 1;

                history.Clear();
            }

            // Get the new increment and update the human value
            increment = getNextIncrementValue(increment);
            humanVal += increment;
        }

        return humanVal;
    }

    private static double EvaluateCommand(string key, ref Dictionary<string, string> dict, double? humnVal = null)
    {
        if (humnVal.HasValue && key == "humn") return humnVal.Value;

        return dict[key].Split(" ") switch
        {
            [var left, "+", var right] => EvaluateCommand(left, ref dict, humnVal) + EvaluateCommand(right, ref dict, humnVal),
            [var left, "-", var right] => EvaluateCommand(left, ref dict, humnVal) - EvaluateCommand(right, ref dict, humnVal),
            [var left, "*", var right] => EvaluateCommand(left, ref dict, humnVal) * EvaluateCommand(right, ref dict, humnVal),
            [var left, "/", var right] => EvaluateCommand(left, ref dict, humnVal) / EvaluateCommand(right, ref dict, humnVal),
            [var num] => double.Parse(num),
            _ => throw new Exception()
        };
    }
}
