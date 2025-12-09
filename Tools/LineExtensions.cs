using AdventOfCode.Tools.Models;

namespace AdventOfCode.Tools;

internal static class LineExtensions
{
    public static Position MidPoint(this Line line)
        => new((line.Start.X + line.End.X) / 2, (line.Start.Y + line.End.Y) / 2);

    public static bool ContainsPosition(this Line line, Position position, bool includeEndpoints = true)
    {
        // Vector from start to end
        long vx = (long)line.End.X - line.Start.X;
        long vy = (long)line.End.Y - line.Start.Y;

        // Vector from start to position
        long wx = (long)position.X - line.Start.X;
        long wy = (long)position.Y - line.Start.Y;

        // Check colinearity via cross product (v x w == 0)
        long cross = (vx * wy) - (vy * wx);
        if (cross != 0)
        {
            return false;
        }

        // Check projection lies within the segment using dot product bounds
        long dot = (vx * wx) + (vy * wy);
        long lenSq = (vx * vx) + (vy * vy);

        if (includeEndpoints)
        {
            return 0 <= dot && dot <= lenSq;
        }
        else
        {
            return 0 < dot && dot < lenSq;
        }
    }
}
