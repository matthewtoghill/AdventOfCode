namespace AdventOfCode.Y2021;

internal class Day12() : Solver(2021, 12)
{
    public override void Run()
    {
        var rooms = LoadRooms(Input.ReadAllLines());
        Part1Solution = CountPaths(rooms, []);
        Part2Solution = CountPaths(rooms, [], allowTwice: true);
    }

    private static Dictionary<string, List<string>> LoadRooms(string[] input)
    {
        DefaultDictionary<string, List<string>> rooms = new(defaultValueFactory: () => []);
        foreach (var line in input)
        {
            var split = line.Split('-');
            rooms[split[0]].Add(split[1]);
            rooms[split[1]].Add(split[0]);
        }
        return rooms;
    }

    private static int CountPaths(Dictionary<string, List<string>> rooms, HashSet<string> visited, string current = "start", bool allowTwice = false)
    {
        if (current == "end")
            return 1;

        int numPaths = 0;

        // If small room, add to visited
        if (current.All(char.IsLower)) visited.Add(current);

        foreach (var room in rooms[current])
        {
            if (room == "start") continue;

            if (!visited.Contains(room)) // If the room is not visited (big or unseen) then explore further
                numPaths += CountPaths(rooms, [.. visited], room, allowTwice);
            else if (allowTwice)         // else small and seen before but allow twice flag is true, explore further
                numPaths += CountPaths(rooms, [.. visited], room, false);
        }

        return numPaths;
    }
}
