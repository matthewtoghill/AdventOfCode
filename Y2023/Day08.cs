namespace AdventOfCode.Y2023;

internal class Day08() : Solver(2023, 8)
{
    private record Node(string Left, string Right);

    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(string[] input)
    {
        var (instructions, nodes) = ParseInput(input);

        var current = "AAA";
        var steps = 0;

        while (current != "ZZZ")
        {
            var node = nodes[current];
            switch (instructions[steps % instructions.Length])
            {
                case 'L': current = node.Left; break;
                case 'R': current = node.Right; break;
            }

            steps++;
        }

        return steps;
    }

    private static long Part2(string[] input)
    {
        var (instructions, nodes) = ParseInput(input);
        var startingNodes = nodes.Keys.Where(x => x.EndsWith('A')).ToArray();
        var stepList = new List<int>();

        foreach (var start in startingNodes)
        {
            var current = start;
            var steps = 0;

            while (!current.EndsWith('Z'))
            {
                var node = nodes[current];
                switch (instructions[steps % instructions.Length])
                {
                    case 'L': current = node.Left; break;
                    case 'R': current = node.Right; break;
                }

                steps++;
            }

            stepList.Add(steps);
        }

        return stepList.LowestCommonMultiple();
    }

    private static (string, Dictionary<string, Node>) ParseInput(string[] input)
    {
        Dictionary<string, Node> nodes = [];
        foreach (var item in input.Skip(2))
        {
            var split = item.Split(["=", "(", ",", ")", " "], StringSplitOptions.RemoveEmptyEntries);
            nodes.Add(split[0], new Node(split[1], split[2]));
        }

        return (input[0], nodes);
    }

}
