namespace AdventOfCode.Y2023;

internal class Day19() : Solver(2023, 19)
{
    private record Part(long X, long M, long A, long S);
    private record PartRange(Range X, Range M, Range A, Range S);
    private record Rule(string Left, string Op, long Right, string Next);
    private record Range(long Min, long Max);

    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        var workflows = GetWorkflows(input[0].SplitLines());
        var parts = input[1].SplitLines().Select(ParsePart).ToArray();

        Part1Solution = Part1(workflows, parts);
        Part2Solution = Part2(workflows, new Range(1, 4000));
    }

    private static long Part1(Dictionary<string, List<Rule>> workflows, Part[] parts)
        => parts.Where(part => CheckAccepted("in", workflows, part))
                .Sum(part => part.X + part.M + part.A + part.S);

    private static long Part2(Dictionary<string, List<Rule>> workflows, Range range)
        => CountAccepted("in", workflows, new PartRange(range, range, range, range));

    private static bool CheckAccepted(string id, Dictionary<string, List<Rule>> workflows, Part part)
    {
        if (id == "A") return true;
        if (id == "R") return false;

        foreach (var workflow in workflows[id])
        {
            switch (workflow.Left.ToUpper(), workflow.Op)
            {
                case ("X", ">") when part.X > workflow.Right:
                case ("M", ">") when part.M > workflow.Right:
                case ("A", ">") when part.A > workflow.Right:
                case ("S", ">") when part.S > workflow.Right:
                case ("X", "<") when part.X < workflow.Right:
                case ("M", "<") when part.M < workflow.Right:
                case ("A", "<") when part.A < workflow.Right:
                case ("S", "<") when part.S < workflow.Right:
                    return CheckAccepted(workflow.Next, workflows, part);
                default:
                    continue;
            }
        }

        return false;
    }

    private static long CountAccepted(string id, Dictionary<string, List<Rule>> workflows, PartRange part)
    {
        if (id == "A")
        {
            return (part.X.Max - part.X.Min + 1)
                 * (part.M.Max - part.M.Min + 1)
                 * (part.A.Max - part.A.Min + 1)
                 * (part.S.Max - part.S.Min + 1);
        }

        if (id == "R") return 0;

        foreach (var (left, op, right, next) in workflows[id])
        {
            var range = left.ToUpper() switch
            {
                "X" => part.X,
                "M" => part.M,
                "A" => part.A,
                "S" => part.S,
                _ => throw new NotSupportedException()
            }; ;

            var passRange = new Range(0, 1);
            var failRange = new Range(0, 1);

            if (op == "<")
            {
                if (range.Min >= right) continue;
                if (range.Max < right) return CountAccepted(next, workflows, part);
                passRange = new Range(range.Min, right - 1);
                failRange = new Range(right, range.Max);
            }
            else if (op == ">")
            {
                if (range.Max <= right) continue;
                if (range.Min > right) return CountAccepted(next, workflows, part);
                failRange = new Range(range.Min, right);
                passRange = new Range(right + 1, range.Max);
            }

            return CountAccepted(next, workflows, BranchPartRanges(part, left, passRange)) + CountAccepted(id, workflows, BranchPartRanges(part, left, failRange));
        }

        return 0;
    }

    private static PartRange BranchPartRanges(PartRange part, string rating, Range range)
        => rating.ToUpper() switch
        {
            "X" => part with { X = range },
            "M" => part with { M = range },
            "A" => part with { A = range },
            "S" => part with { S = range },
            _ => throw new NotSupportedException()
        };

    private static Dictionary<string, List<Rule>> GetWorkflows(string[] input)
    {
        Dictionary<string, List<Rule>> workflows = [];

        foreach (var item in input)
        {
            var (id, list) = ParseWorkflow(item);
            workflows[id] = list;
        }

        return workflows;
    }

    private static (string, List<Rule>) ParseWorkflow(string line)
    {
        var split = line.Replace(">", ",>,").Replace("<", ",<,").Split(["{", "}", ",", ":"], StringSplitOptions.RemoveEmptyEntries);
        var id = split[0];
        var end = split[^1];

        var rules = split[1..^1].Chunk(4).Select(x => new Rule(x[0], x[1], long.Parse(x[2]), x[3])).ToList();
        rules.Add(new Rule("X", ">", 0, end));

        return (id, rules);
    }

    private static Part ParsePart(string line)
    {
        var vals = line.ExtractNumeric<long>().ToArray();
        return new Part(vals[0], vals[1], vals[2], vals[3]);
    }
}
