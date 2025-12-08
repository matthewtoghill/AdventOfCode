using System.Numerics;

namespace AdventOfCode.Tools;

public static class PointExtensions
{
    public static T ManhattanDistance<T>(this (T X, T Y) startPos, (T X, T Y) endPos) where T : INumber<T>
        => T.Abs(startPos.X - endPos.X) + T.Abs(startPos.Y - endPos.Y);
    public static T ManhattanDistance<T>(this (T X, T Y, T Z) startPos, (T X, T Y, T Z) endPos) where T : INumber<T>
        => T.Abs(startPos.X - endPos.X) + T.Abs(startPos.Y - endPos.Y) + T.Abs(startPos.Z - endPos.Z);

    public static double DirectDistance(this (int X, int Y) startPos, (int X, int Y) endPos) => Math.Sqrt(Math.Pow(endPos.X - startPos.X, 2) + Math.Pow(endPos.Y - startPos.Y, 2));
    public static double DirectDistance(this (long X, long Y) startPos, (long X, long Y) endPos) => Math.Sqrt(Math.Pow(endPos.X - startPos.X, 2) + Math.Pow(endPos.Y - startPos.Y, 2));

    public static double DirectDistance(this (int X, int Y, int Z) startPos, (int X, int Y, int Z) endPos)
        => Math.Sqrt(Math.Pow(endPos.X - startPos.X, 2) + Math.Pow(endPos.Y - startPos.Y, 2) + Math.Pow(endPos.Z - startPos.Z, 2));

    public static double DirectDistance(this (long X, long Y, long Z) startPos, (long X, long Y, long Z) endPos)
        => Math.Sqrt(Math.Pow(endPos.X - startPos.X, 2) + Math.Pow(endPos.Y - startPos.Y, 2) + Math.Pow(endPos.Z - startPos.Z, 2));

    public static T ChessDistance<T>(this (T X, T Y) startPos, (T X, T Y) endPos) where T : INumber<T>
        => T.Max(T.Abs(startPos.X - endPos.X), T.Abs(startPos.Y - endPos.Y));

    public static bool IsWithinBounds<T>(this (T X, T Y) pos, (T X, T Y) minPos, (T X, T Y) maxPos) where T : INumber<T>
    {
        return minPos.X <= pos.X && pos.X <= maxPos.X
            && minPos.Y <= pos.Y && pos.Y <= maxPos.Y;
    }

    public static bool IsWithinBounds(this (int X, int Y) pos, (int X, int Y) minPos, (int X, int Y) maxPos)
    {
        return minPos.X <= pos.X && pos.X <= maxPos.X
            && minPos.Y <= pos.Y && pos.Y <= maxPos.Y;
    }

    public static bool IsWithinBounds(this (long X, long Y) pos, (long X, long Y) minPos, (long X, long Y) maxPos)
    {
        return minPos.X <= pos.X && pos.X <= maxPos.X
            && minPos.Y <= pos.Y && pos.Y <= maxPos.Y;
    }

    public static bool IsOutsideBounds(this (int X, int Y) pos, (int X, int Y) minPos, (int X, int Y) maxPos)
    {
        return minPos.X > pos.X || pos.X > maxPos.X
            || minPos.Y > pos.Y || pos.Y > maxPos.Y;
    }

    public static bool IsOutsideBounds(this (long X, long Y) pos, (long X, long Y) minPos, (long X, long Y) maxPos)
    {
        return minPos.X > pos.X || pos.X > maxPos.X
            || minPos.Y > pos.Y || pos.Y > maxPos.Y;
    }

    public static IEnumerable<(int X, int Y)> GetNeighbours(this (int X, int Y) pos, bool includeDiagonal = false)
        => includeDiagonal ? GetAllNeighbours(pos) : GetAdjacent(pos);

    private static readonly (int X, int Y)[] CartesianDirections = [(-1, 0), (0, -1), (1, 0), (0, 1)];
    private static readonly (int X, int Y)[] AllDirections = [(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)];

    private static IEnumerable<(int X, int Y)> GetAdjacent(this (int X, int Y) pos)
    {
        foreach (var (X, Y) in CartesianDirections)
            yield return (pos.X + X, pos.Y + Y);
    }

    private static IEnumerable<(int X, int Y)> GetAllNeighbours(this (int X, int Y) pos)
    {
        foreach (var (X, Y) in AllDirections)
            yield return (pos.X + X, pos.Y + Y);
    }

    public static (int, int) MoveInDirection(this (int X, int Y) pos, char direction, int distance = 1)
    {
        return char.ToLower(direction) switch
        {
            'u' or '^' or 'n' => (pos.X, pos.Y - distance),
            'd' or 'v' or 's' => (pos.X, pos.Y + distance),
            'l' or '<' or 'w' => (pos.X - distance, pos.Y),
            'r' or '>' or 'e' => (pos.X + distance, pos.Y),
            _ => (pos.X, pos.Y),
        };
    }

    public static (long, long) MoveInDirection(this (long X, long Y) pos, char direction, long distance = 1)
    {
        return char.ToLower(direction) switch
        {
            'u' or '^' or 'n' => (pos.X, pos.Y - distance),
            'd' or 'v' or 's' => (pos.X, pos.Y + distance),
            'l' or '<' or 'w' => (pos.X - distance, pos.Y),
            'r' or '>' or 'e' => (pos.X + distance, pos.Y),
            _ => (pos.X, pos.Y),
        };
    }

    public static int CalculateArea(this List<(int X, int Y)> positions)
    {
        var result = 0;
        var zip = positions.Zip(positions.Skip(1));
        foreach (var (a, b) in zip)
        {
            result += (a.X * b.Y) - (a.Y * b.X);
        }

        return Math.Abs(result);
    }

    public static long CalculateArea(this List<(long X, long Y)> positions)
    {
        long result = 0;
        var zip = positions.Zip(positions.Skip(1));
        foreach (var (a, b) in zip)
        {
            result += (a.X * b.Y) - (a.Y * b.X);
        }

        return Math.Abs(result);
    }
}