using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;

internal class Day15() : Solver(2022, 15)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static int Part1(string[] input)
    {
        var sensors = ParseSensors(input);
        const int row = 2_000_000;

        // get the left and right edge positions of the sensor radius that intersect with the row
        var sensorRanges = new List<(int start, int end)>();
        foreach (var (sensor, beacon, radius) in sensors)
        {
            var rowDistance = Math.Abs(row - sensor.Y);
            if (rowDistance > radius) continue; // skip where the sensor radius does not intersect with the row

            var left = sensor.X - (radius - rowDistance);
            var right = sensor.X + (radius - rowDistance);
            sensorRanges.Add((left, right));
        }

        var minX = sensorRanges.Min(x => x.start);
        var maxX = sensorRanges.Max(x => x.end);
        var beaconsOnRow = sensors.Select(x => x.Beacon.Y).Where(y => y == row).Distinct().Count();

        // count the number of points along the row that are visible to sensors
        var visibleCount = 0;

        for (int x = minX; x <= maxX; x++)
        {
            foreach (var (start, end) in sensorRanges)
            {
                if (x.IsBetween(start, end))
                {
                    visibleCount++;
                    break;
                }
            }
        }

        return visibleCount - beaconsOnRow;
    }

    private static long Part2(string[] input)
    {
        var sensors = ParseSensors(input).OrderBy(x => x.Radius).ToList(); // sort on radius, smallest first
        const int rangeMax = 4_000_000;

        foreach (var (sensor, beacon, radius) in sensors)
        {
            var top = new Position(sensor.X, sensor.Y - radius - 1);
            var bot = new Position(sensor.X, sensor.Y + radius + 1);
            var edgeLength = radius + 2; // the outside length of the sensor radius

            for (int i = 0; i < edgeLength; i++)
            {
                // originally this was checking 1 spot on all 4 edges (NW/NE/SE/SW)
                // however it ended up solving quicker when only checking a single edge on each sensor
                // in this case the solution was found in the fewest iterations/checks when only checking the SE edge
                // this may not be true for all inputs and varies depending on the order of the sensors collection

                //var NW = new Position(top.X - i, top.Y + i);
                //if (IsDistressBeacon(sensors, rangeMax, NW)) return (NW.X * 4_000_000L) + NW.Y;

                //var NE = new Position(top.X + i, top.Y + i);
                //if (IsDistressBeacon(sensors, rangeMax, NE)) return (NE.X * 4_000_000L) + NE.Y;

                var SE = new Position(bot.X + i, bot.Y - i);
                if (IsDistressBeacon(sensors, rangeMax, SE)) return (SE.X * 4_000_000L) + SE.Y;

                //var SW = new Position(bot.X - i, bot.Y - i);
                //if (IsDistressBeacon(sensors, rangeMax, SW)) return (SW.X * 4_000_000L) + SW.Y;
            }
        }

        return 0;
    }

    private static bool IsDistressBeacon(List<(Position Sensor, Position Beacon, int Radius)> sensors, int rangeMax, Position p)
        => p.X.IsBetween(0, rangeMax) && p.Y.IsBetween(0, rangeMax) && sensors.All(s => s.Sensor.ManhattanDistance(p) > s.Radius);

    private static List<(Position Sensor, Position Beacon, int Radius)> ParseSensors(string[] input)
    {
        var result = new List<(Position Sensor, Position Beacon, int Radius)>();
        foreach (var line in input)
        {
            var split = line.SplitOn(" ", "=", ",", ":");
            var sensor = new Position(int.Parse(split[3]), int.Parse(split[6]));
            var beacon = new Position(int.Parse(split[13]), int.Parse(split[16]));
            var radius = sensor.ManhattanDistance(beacon);

            result.Add((sensor, beacon, radius));
        }

        return result;
    }
}
