namespace AdventOfCode.Y2022;

internal class Day19() : Solver(2022, 19)
{
    private record RobotState(int Ore, int Clay, int Obsidian, int Geodes, int OreBots, int ClayBots, int ObsidianBots, int GeodeBots, int MinsLeft);

    public override void Run()
    {
        var blueprints = Input.ReadAllLines().Select(ParseBlueprint).ToArray();
        Part1Solution = blueprints.Sum(x => x.Id * GenerateMaxGeodes(x, 24));
        Part2Solution = blueprints.Take(3).Product(x => GenerateMaxGeodes(x, 32));
    }

    private static int GenerateMaxGeodes(Blueprint blueprint, int totalMinutes)
    {
        var initialState = new RobotState(0, 0, 0, 0, 1, 0, 0, 0, totalMinutes);
        Queue<RobotState> queue = new([initialState]);
        HashSet<RobotState> states = [];
        var maxGeodes = 0;

        while (queue.Count > 0)
        {
            var (ore, clay, obsidian, geodes, oreBots, clayBots, obsidianBots, geodeBots, minsLeft) = queue.Dequeue();

            maxGeodes = int.Max(maxGeodes, geodes);

            if (minsLeft < 1)
                continue;

            ore = int.Min(ore, (minsLeft * blueprint.MaxOreRequired) - (oreBots * (minsLeft - 1)));
            clay = int.Min(clay, (minsLeft * blueprint.ObsidianCost.Clay) - (clayBots * (minsLeft - 1)));
            obsidian = int.Min(obsidian, (minsLeft * blueprint.GeodeCost.Obsidian) - (obsidianBots * (minsLeft - 1)));

            var state = new RobotState(ore, clay, obsidian, geodes, oreBots, clayBots, obsidianBots, geodeBots, minsLeft);

            if (states.Contains(state)) continue;

            states.Add(state);

            // don't build anything
            var baseState = new RobotState(ore + oreBots,
                                           clay + clayBots,
                                           obsidian + obsidianBots,
                                           geodes + geodeBots,
                                           oreBots,
                                           clayBots,
                                           obsidianBots,
                                           geodeBots,
                                           minsLeft - 1);

            queue.Enqueue(baseState);

            // try to build an ore robot if additional resources are still needed
            if (ore >= blueprint.OreCost && oreBots < blueprint.MaxOreRequired)
                queue.Enqueue(baseState with
                {
                    Ore = ore + oreBots - blueprint.OreCost,
                    OreBots = oreBots + 1
                });

            // try to build a clay robot if additional resources are still needed
            if (ore >= blueprint.ClayCost && clayBots < blueprint.ObsidianCost.Clay)
                queue.Enqueue(baseState with
                {
                    Ore = ore + oreBots - blueprint.ClayCost,
                    ClayBots = clayBots + 1
                });

            // try to build an obsidian robot if additional resources are still needed
            if (ore >= blueprint.ObsidianCost.Ore && clay >= blueprint.ObsidianCost.Clay && obsidianBots < blueprint.GeodeCost.Obsidian)
                queue.Enqueue(baseState with
                {
                    Ore = ore + oreBots - blueprint.OreCost,
                    Clay = clay + clayBots - blueprint.ObsidianCost.Clay,
                    ObsidianBots = obsidianBots + 1
                });

            // try to build a geode robot
            if (ore >= blueprint.GeodeCost.Ore && obsidian >= blueprint.GeodeCost.Obsidian)
                queue.Enqueue(baseState with
                {
                    Ore = ore + oreBots - blueprint.OreCost,
                    Obsidian = obsidian + obsidianBots - blueprint.GeodeCost.Obsidian,
                    GeodeBots = geodeBots + 1
                });
        }

        return maxGeodes;
    }

    private static Blueprint ParseBlueprint(string line) => new(line);

    private class Blueprint
    {
        public int Id { get; }
        public int OreCost { get; }
        public int ClayCost { get; }
        public (int Ore, int Clay) ObsidianCost { get; }
        public (int Ore, int Obsidian) GeodeCost { get; }
        public int MaxOreRequired { get; }

        public Blueprint(string line)
        {
            var nums = line.ExtractInts().ToArray();
            Id = nums[0];
            OreCost = nums[1];
            ClayCost = nums[2];
            ObsidianCost = (nums[3], nums[4]);
            GeodeCost = (nums[5], nums[6]);
            MaxOreRequired = new int[] { OreCost, ClayCost, ObsidianCost.Ore, GeodeCost.Ore }.Max();
        }
    }
}
