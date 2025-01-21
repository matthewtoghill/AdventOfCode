namespace AdventOfCode.Y2023;

internal class Day20() : Solver(2023, 20)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Solve(input, 1000);
        Part2Solution = Solve(input, int.MaxValue, isPart2: true);
    }

    private static long Solve(string[] input, int iterations, bool isPart2 = false)
    {
        var modules = ParseModules(input);
        Dictionary<string, int> endLowPulseFactors = [];
        const string startKey = "broadcaster";
        int count = 0;
        long lowPulses = 0;
        long highPulses = 0;

        while (count < iterations || isPart2)
        {
            count++;

            var queue = new Queue<(string from, string moduleId, bool isHighPulse)>([("button", startKey, false)]);
            while (queue.Count > 0)
            {
                var (fromId, moduleId, isHighPulse) = queue.Dequeue();

                if (isHighPulse) highPulses++;
                if (!isHighPulse) lowPulses++;

                // end of the chain, no destination modules to enqueue
                if (!modules.TryGetValue(moduleId, out var module)) continue;

                // start of the chain, could also check if module.Type == ModuleType.None
                if (moduleId == startKey)
                    module.DestinationIds.ForEach(dest => queue.Enqueue((module.Id, dest, isHighPulse)));

                // handle receiving the pulse depending on the module type and if the pulse is high or low
                if (module.Type == ModuleType.FlipFlop && isHighPulse) continue;
                if (module.Type == ModuleType.FlipFlop && !isHighPulse)
                {
                    module.IsOn = !module.IsOn;
                    module.DestinationIds.ForEach(dest => queue.Enqueue((module.Id, dest, module.IsOn)));
                }

                if (module.Type == ModuleType.Conjunction)
                {
                    // If the current module is a Conjunction and has only 1 destination Id and that Id is not in the list of modules
                    // then it will be pointing to the module at end of the chain.
                    // If any of the pulse memory for this conjunction module last sent a High Pulse
                    // then the current iteration count is a factor in determining the Lowest Common Multiple (LCM) of the iteration count
                    // when all modules in this pulse memory will send a High Pulse causing this module to send a Low Pulse to the end module
                    if (isPart2 &&
                        module.DestinationIds.Count == 1 &&
                        !modules.ContainsKey(module.DestinationIds.First()) &&
                        module.ConjunctionPulseMemory.Values.Any(x => x))
                    {
                        // update the list of factors for any module keys that are true (High Pulse)
                        module.ConjunctionPulseMemory.Where(x => x.Value).Select(x => x.Key).ForEach(key => endLowPulseFactors[key] = count);

                        // Once a factor has been found for all of the conjunction keys then calculate the LCM
                        if (module.ConjunctionPulseMemory.Keys.ToHashSet().SetEquals(endLowPulseFactors.Keys))
                            return endLowPulseFactors.Values.LowestCommonMultiple();
                    }

                    module.ConjunctionPulseMemory[fromId] = isHighPulse;

                    // If any are Low then send a High pulse, else all must be High so send a Low pulse
                    var sendHighPulse = module.ConjunctionPulseMemory.Values.Any(x => !x);
                    module.DestinationIds.ForEach(dest => queue.Enqueue((module.Id, dest, sendHighPulse)));
                }
            }
        }

        return lowPulses * highPulses;
    }

    private static Dictionary<string, Module> ParseModules(string[] input)
    {
        Dictionary<string, Module> modules = [];

        foreach (var line in input)
        {
            var split = line.Split(["%", "&", ",", " ", "->"], StringSplitOptions.RemoveEmptyEntries);
            var type = ModuleType.None;
            if (line.StartsWith('%')) type = ModuleType.FlipFlop;
            if (line.StartsWith('&')) type = ModuleType.Conjunction;
            modules[split[0]] = new Module(split[0], type, split.Skip(1).ToHashSet());
        }

        // Set the Conjunction Modules initial memory values
        foreach (var (id, module) in modules)
        {
            foreach (var key in module.DestinationIds)
            {
                if (!modules.ContainsKey(key)) continue;

                if (modules[key].Type == ModuleType.Conjunction)
                    modules[key].ConjunctionPulseMemory[id] = false;

            }
        }

        return modules;
    }

    private enum ModuleType
    {
        None,
        Conjunction,
        FlipFlop
    }

    private record Module(string Id, ModuleType Type)
    {
        public bool IsHighPulse { get; set; }
        public bool IsOn { get; set; }
        public HashSet<string> DestinationIds { get; set; } = [];
        public Dictionary<string, bool> ConjunctionPulseMemory = [];

        public Module(string id, ModuleType type, HashSet<string> destinationIds) : this(id, type)
        {
            DestinationIds = destinationIds;
        }
    }
}
