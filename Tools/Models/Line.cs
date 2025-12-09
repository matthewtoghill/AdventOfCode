namespace AdventOfCode.Tools.Models;

public record struct Line(Position Start, Position End)
{
    public readonly double Length => Start.DirectDistance(End);

    public readonly int MinX => Math.Min(Start.X, End.X);
    public readonly int MaxX => Math.Max(Start.X, End.X);
    public readonly int MinY => Math.Min(Start.Y, End.Y);
    public readonly int MaxY => Math.Max(Start.Y, End.Y);

    public readonly bool Intersects(Line other) => Intersects(other.Start, other.End);

    public readonly bool Intersects(Position pointA, Position pointB)
    {
        var minX = Math.Min(pointA.X, pointB.X);
        var maxX = Math.Max(pointA.X, pointB.X);
        var minY = Math.Min(pointA.Y, pointB.Y);
        var maxY = Math.Max(pointA.Y, pointB.Y);

        var lineMinX = Math.Min(Start.X, End.X);
        var lineMaxX = Math.Max(Start.X, End.X);
        var lineMinY = Math.Min(Start.Y, End.Y);
        var lineMaxY = Math.Max(Start.Y, End.Y);

        return lineMaxX > minX && lineMinX < maxX && lineMaxY > minY && lineMinY < maxY;
    }

    public readonly Line Translate(int dx, int dy)
        => new(new Position(Start.X + dx, Start.Y + dy), new Position(End.X + dx, End.Y + dy));

    public readonly Line Reverse() => new(End, Start);
}
