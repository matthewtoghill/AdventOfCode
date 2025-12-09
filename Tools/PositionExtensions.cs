using System.Text;
using AdventOfCode.Tools.Models;

namespace AdventOfCode.Tools;

public static class PositionExtensions
{
    public static Position[] MoveAllInDirection(this Position[] positions, char direction)
    {
        for (int i = 0; i < positions.Length; i++)
            positions[i] = positions[i].MoveInDirection(direction);

        return positions;
    }

    public static List<Position> MoveAllInDirection(this List<Position> positions, char direction)
    {
        for (int i = 0; i < positions.Count; i++)
            positions[i] = positions[i].MoveInDirection(direction);

        return positions;
    }

    public static int CalculateArea(this Position position, Position other, bool includeBorder = true)
        => (Math.Abs(position.X - other.X) + (includeBorder ? 1 : 0))
         * (Math.Abs(position.Y - other.Y) + (includeBorder ? 1 : 0));

    public static long CalculateAreaLong(this Position position, Position other, bool includeBorder = true)
        => (Math.Abs((long)position.X - (long)other.X) + (includeBorder ? 1 : 0))
         * (Math.Abs((long)position.Y - (long)other.Y) + (includeBorder ? 1 : 0));

    public static int CalculateArea(this List<Position> positions)
    {
        var result = 0;
        var zip = positions.Zip(positions.Skip(1));
        foreach (var (a, b) in zip)
        {
            result += (a.X * b.Y) - (a.Y * b.X);
        }

        return Math.Abs(result);
    }

    public static HashSet<Position> CreateBox(this Position cornerA, Position cornerB)
    {
        var box = new HashSet<Position>();
        var topLeft = new Position(Math.Min(cornerA.X, cornerB.X), Math.Min(cornerA.Y, cornerB.Y));
        var bottomRight = new Position(Math.Max(cornerA.X, cornerB.X), Math.Max(cornerA.Y, cornerB.Y));

        for (int x = topLeft.X; x <= bottomRight.X; x++)
            for (int y = topLeft.Y; y <= bottomRight.Y; y++)
                box.Add(new Position(x, y));

        return box;
    }

    public static HashSet<Position> CreateBoxOutline(this Position cornerA, Position cornerB)
    {
        var box = new HashSet<Position>();
        var topLeft = new Position(Math.Min(cornerA.X, cornerB.X), Math.Min(cornerA.Y, cornerB.Y));
        var bottomRight = new Position(Math.Max(cornerA.X, cornerB.X), Math.Max(cornerA.Y, cornerB.Y));

        for (int x = topLeft.X; x <= bottomRight.X; x++)
        {
            box.Add(new Position(x, topLeft.Y));
            box.Add(new Position(x, bottomRight.Y));
        }

        for (int y = topLeft.Y; y <= bottomRight.Y; y++)
        {
            box.Add(new Position(topLeft.X, y));
            box.Add(new Position(bottomRight.X, y));
        }

        return box;
    }

    public static int CountPositionsInsideArea(this List<Position> positions)
    {
        var count = positions.ToHashSet().Count;
        var area = CalculateArea(positions);
        return 1 + (area - count) / 2;
    }

    public static int CountPerimeter(this List<Position> positions) => positions.ToHashSet().CountPerimeter();
    public static int CountPerimeter(this HashSet<Position> positions) => positions.Sum(p => 4 - p.GetNeighbours().Count(positions.Contains));

    public static int CountCorners(this List<Position> positions) => positions.ToHashSet().CountCorners();
    public static int CountCorners(this HashSet<Position> positions)
    {
        var corners = 0;
        var directionPairs = new List<(char, char)> { ('n', 'e'), ('n', 'w'), ('s', 'e'), ('s', 'w') };
        foreach (var pos in positions)
        {
            foreach (var (dirA, dirB) in directionPairs)
            {
                var a = pos.MoveInDirection(dirA);
                var b = pos.MoveInDirection(dirB);

                if (!positions.Contains(a) && !positions.Contains(b))
                    corners++;

                if (positions.Contains(a) && positions.Contains(b) && !positions.Contains(a.MoveInDirection(dirB)))
                    corners++;
            }
        }

        return corners;
    }

    public static void PrintMap<T>(this Dictionary<Position, T> map)
    {
        var maxHeight = map.Keys.Max(x => x.Y);
        var maxWidth = map.Keys.Max(x => x.X);
        for (int y = 0; y <= maxHeight; y++)
        {
            var sb = new StringBuilder();
            for (int x = 0; x <= maxWidth; x++)
            {
                sb.Append(map[new Position(x, y)]);
            }

            Console.WriteLine(sb.ToString());
        }
    }

    public static void PrintGrid(this HashSet<Position> positions, int gridWidth, int gridHeight)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            var sb = new StringBuilder();
            for (int x = 0; x < gridWidth; x++)
            {
                if (positions.Contains(new(x, y)))
                    sb.Append('#');
                else
                    sb.Append('.');
            }

            Console.WriteLine(sb.ToString());
        }
    }
}