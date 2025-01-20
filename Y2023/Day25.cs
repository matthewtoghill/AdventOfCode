namespace AdventOfCode.Y2023;

internal class Day25() : Solver(2023, 25)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 3);
        Part2Solution = "Happy Christmas!";
    }

    private static long Solve(string[] input, int requiredCutSize)
    {
        var components = GetComponents(input);
        var iterations = 0;

        while (true)
        {
            iterations++;
            var graph = BuildGraph(components);

            while (graph.Count > 2)
                graph.Values.RandomElement().MergeRandomConnection(graph);

            if (graph.Values.All(x => x.Connections.Count == requiredCutSize))
                return graph.Values.Select(x => x.Value).Product();
        }
    }

    private static Dictionary<string, Component> BuildGraph(List<Component> components)
        => components.ToDictionary(k => k.Name, v => new Component(v.Name, v.Connections));

    private static List<Component> GetComponents(string[] input)
    {
        List<Component> graph = [];

        void addRelationship(string name, string component)
        {
            var current = graph.FirstOrDefault(x => x.Name == name);
            if (current is null)
            {
                current = new Component(name);
                graph.Add(current);
            }
            current.Connections.Add(component);
        }

        foreach (var line in input)
        {
            var split = line.Split([":", " "], StringSplitOptions.RemoveEmptyEntries);
            var name = split[0];
            var connections = split[1..];

            foreach (var wire in connections)
            {
                addRelationship(name, wire);
                addRelationship(wire, name);
            }
        }

        return graph;
    }


    private record Component(string Name)
    {
        public List<string> Connections { get; set; } = [];
        public int Value { get; private set; } = 1;

        public Component(string name, IEnumerable<string> connections) : this(name)
        {
            Connections = connections.ToList();
        }

        public void MergeRandomConnection(Dictionary<string, Component> components)
        {
            var component = components[Connections.RandomElement()];
            Value += component.Value;
            Connections.AddRange(component.Connections);

            foreach (var item in component.Connections)
            {
                while (components[item].Connections.Remove(component.Name))
                    components[item].Connections.Add(Name);
            }

            Connections.RemoveAll(x => x == Name);
            components.Remove(component.Name);
        }
    }
}
