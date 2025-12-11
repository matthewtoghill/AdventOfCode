using System.Text.RegularExpressions;
using Microsoft.Z3;

namespace AdventOfCode.Y2025;

internal class Day10() : AdventOfCode.Lib.Solver(2025, 10)
{
    public override void Run()
    {
        var machines = Input.ReadAllLines().Select(ParseLine).ToList();
        Part1Solution = machines.Sum(MinimumButtonsForLight);
        Part2Solution = machines.Sum(MinimumButtonsForJoltage);
    }

    private static int MinimumButtonsForLight(Machine machine)
    {
        var visited = new bool[1 << machine.DiagramLength];
        var queue = new Queue<(int Mask, int Presses)>();
        queue.Enqueue((0, 0));
        visited[0] = true;

        while (queue.Count > 0)
        {
            var (mask, presses) = queue.Dequeue();
            if (mask == machine.LightMask)
                return presses;

            foreach (var buttonMask in machine.ButtonMasks)
            {
                var next = mask ^ buttonMask;
                if (visited[next])
                    continue;

                visited[next] = true;
                queue.Enqueue((next, presses + 1));
            }
        }

        return -1;
    }

    private static int MinimumButtonsForJoltage(Machine machine)
    {
        using var ctx = new Context();
        var opt = ctx.MkOptimize();
        var buttonVars = CreateButtonVariables(ctx, opt, machine.ButtonCount);

        AddPositionConstraints(ctx, opt, machine, buttonVars);

        opt.MkMinimize(ctx.MkAdd(buttonVars.Cast<ArithExpr>().ToArray()));

        if (opt.Check() != Status.SATISFIABLE)
            return 0;

        var model = opt.Model;
        var presses = 0;

        foreach (var varExpr in buttonVars)
            presses += ((IntNum)model.Evaluate(varExpr)).Int;

        return presses;
    }

    private static IntExpr[] CreateButtonVariables(Context ctx, Optimize opt, int count)
    {
        var vars = new IntExpr[count];
        var zero = ctx.MkInt(0);
        for (int i = 0; i < count; i++)
        {
            vars[i] = ctx.MkIntConst(i.ToString());
            opt.Add(ctx.MkGe(vars[i], zero));
        }

        return vars;
    }

    private static void AddPositionConstraints(Context ctx, Optimize opt, Machine machine, IntExpr[] buttonVars)
    {
        for (int position = 0; position < machine.DiagramLength; position++)
        {
            var affectingButtons = machine.ButtonsByPosition[position];
            var target = ctx.MkInt(machine.Joltages[position]);

            if (affectingButtons.Length == 0)
            {
                opt.Add(ctx.MkEq(ctx.MkInt(0), target));
                continue;
            }

            var terms = affectingButtons.Select(index => (ArithExpr)buttonVars[index]).ToArray();
            opt.Add(ctx.MkEq(ctx.MkAdd(terms), target));
        }
    }

    private static Machine ParseLine(string line)
    {
        var match = LineRegex.Match(line);
        if (!match.Success)
            throw new ArgumentException("Line has invalid format.", nameof(line));

        var diagram = match.Groups["diagram"].Value;
        var buttons = ButtonsGroupRegex.Matches(match.Groups["buttons"].Value)
                                       .Select(x => x.Groups[1].Value.ExtractInts().ToArray())
                                       .ToList();
        var joltages = match.Groups["joltages"].Value.ExtractInts().ToArray();

        return new Machine(diagram, buttons, joltages);
    }

    private static readonly Regex LineRegex = new(@"^\s*\[(?<diagram>[.#]+)\]\s*(?<buttons>(?:\([^)]*\)\s*)+)\{(?<joltages>[^}]*)\}\s*$", RegexOptions.Compiled);
    private static readonly Regex ButtonsGroupRegex = new(@"\(([^)]*)\)", RegexOptions.Compiled);

    private sealed class Machine
    {
        public Machine(string diagram, List<int[]> buttons, int[] joltages)
        {
            Diagram = diagram;
            Buttons = buttons;
            Joltages = joltages;
            DiagramLength = diagram.Length;
            LightMask = ComputeLightMask(diagram);
            ButtonMasks = BuildButtonMasks(buttons);
            ButtonsByPosition = BuildButtonsByPosition(DiagramLength, buttons);
        }

        public string Diagram { get; }
        public List<int[]> Buttons { get; }
        public int[] Joltages { get; }
        public int DiagramLength { get; }
        public int LightMask { get; }
        public int[] ButtonMasks { get; }
        public int[][] ButtonsByPosition { get; }
        public int ButtonCount => Buttons.Count;

        private static int ComputeLightMask(string diagram)
        {
            var mask = 0;
            for (int i = 0; i < diagram.Length; i++)
            {
                if (diagram[i] == '#')
                    mask |= 1 << i;
            }

            return mask;
        }

        private static int[] BuildButtonMasks(List<int[]> buttons)
        {
            var result = new int[buttons.Count];
            for (int i = 0; i < buttons.Count; i++)
            {
                var mask = 0;
                foreach (var position in buttons[i])
                    mask |= 1 << position;
                result[i] = mask;
            }

            return result;
        }

        private static int[][] BuildButtonsByPosition(int diagramLength, List<int[]> buttons)
        {
            var positions = new List<int>[diagramLength];

            for (int i = 0; i < diagramLength; i++)
                positions[i] = [];

            for (int button = 0; button < buttons.Count; button++)
            {
                foreach (var position in buttons[button])
                    positions[position].Add(button);
            }

            var result = new int[diagramLength][];

            for (int i = 0; i < diagramLength; i++)
                result[i] = positions[i].ToArray();

            return result;
        }
    }
}
