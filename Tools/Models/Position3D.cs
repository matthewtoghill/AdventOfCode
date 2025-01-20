namespace AdventOfCode.Tools.Models;

public readonly struct Position3D
{
    public readonly int X;
    public readonly int Y;
    public readonly int Z;

    public Position3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(Position3D other) => X == other.X && Y == other.Y && Z == other.Z;
    public override bool Equals(object? obj) => obj is Position3D p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
    public override string ToString() => $"({X}, {Y}, {Z})";

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public static bool operator ==(Position3D left, Position3D right) => left.Equals(right);
    public static bool operator !=(Position3D left, Position3D right) => !left.Equals(right);
    public static Position3D operator +(Position3D a, Position3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Position3D operator +(Position3D a, (int X, int Y, int Z) b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Position3D operator -(Position3D p) => new(-p.X, -p.Y, -p.Z);
    public static Position3D operator -(Position3D a, Position3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Position3D operator -(Position3D a, (int X, int Y, int Z) b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Position3D operator *(Position3D a, Position3D b) => new(a.X * b.X, b.Y * b.Y, a.Z * b.Z);
    public static Position3D operator *(Position3D a, (int X, int Y, int Z) b) => new(a.X * b.X, b.Y * b.Y, a.Z * b.Z);
    public static Position3D operator /(Position3D a, Position3D b) => new(a.X / b.X, b.Y / b.Y, a.Z / b.Z);
    public static Position3D operator /(Position3D a, (int X, int Y, int Z) b) => new(a.X / b.X, b.Y / b.Y, a.Z / b.Z);

    public bool IsBetween(Position3D min, Position3D max)
        => min.X <= X && X <= max.X
        && min.Y <= Y && Y <= max.Y
        && min.Z <= Z && Z <= max.Z;

    public IEnumerable<Position3D> GetNeighbours(bool includeDiagonal = false) => includeDiagonal ? GetAllNeighbours() : GetAdjacent();

    public static readonly (int X, int Y, int Z)[] AdjacentDirections = [(-1, 0, 0), (0, -1, 0), (0, 0, -1), (1, 0, 0), (0, 1, 0), (0, 0, 1)];
    public static readonly (int X, int Y, int Z)[] AllDirections
        = Enumerable.Range(-1, 3)
                    .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                    .Select(z => (x, y, z))))
                    .Where(d => d != (0, 0, 0)).ToArray();

    private IEnumerable<Position3D> GetAdjacent()
    {
        foreach (var dir in AdjacentDirections)
            yield return this + dir;
    }

    private IEnumerable<Position3D> GetAllNeighbours()
    {
        foreach (var dir in AllDirections)
            yield return this + dir;
    }
}
