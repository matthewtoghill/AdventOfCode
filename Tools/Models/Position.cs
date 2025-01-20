namespace AdventOfCode.Tools.Models;

public readonly struct Position
{
    public readonly int X;
    public readonly int Y;
    public int Row => Y;
    public int Col => X;

    public Position(int x, int y) => (X, Y) = (x, y);
    public Position((int x, int y) pos) => (X, Y) = (pos.x, pos.y);

    public Position MoveInDirection(char direction, int distance = 1)
    {
        return char.ToLower(direction) switch
        {
            'u' or '^' or 'n' => new(X, Y - distance),
            'd' or 'v' or 's' => new(X, Y + distance),
            'l' or '<' or 'w' => new(X - distance, Y),
            'r' or '>' or 'e' => new(X + distance, Y),
            _ => new(X, Y),
        };
    }

    public Position MoveInOppositeDirection(char direction, int distance = 1)
    {
        return char.ToLower(direction) switch
        {
            'u' or '^' or 'n' => new(X, Y + distance),
            'd' or 'v' or 's' => new(X, Y - distance),
            'l' or '<' or 'w' => new(X + distance, Y),
            'r' or '>' or 'e' => new(X - distance, Y),
            _ => new(X, Y),
        };
    }

    public Position Rotate(RotationDegrees degrees = RotationDegrees.Clockwise_90)
        => degrees switch
        {
            RotationDegrees.Clockwise_90 or RotationDegrees.CounterClockwise_270 => new(Y, -X),
            RotationDegrees.Clockwise_270 or RotationDegrees.CounterClockwise_90 => new(-Y, X),
            RotationDegrees._180 => new(-X, -Y),
            _ => new(X, Y),
        };

    public bool Equals(Position other) => X == other.X && Y == other.Y;
    public bool Equals((int X, int Y) other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Position p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
    public override string ToString() => $"({X}, {Y})";

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public static bool operator ==(Position a, Position b) => a.Equals(b);
    public static bool operator !=(Position a, Position b) => !a.Equals(b);
    public static bool operator ==(Position a, (int X, int Y) b) => a.Equals(b);
    public static bool operator !=(Position a, (int X, int Y) b) => !a.Equals(b);
    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    public static Position operator +(Position a, (int X, int Y) b) => new(a.X + b.X, a.Y + b.Y);
    public static Position operator -(Position p) => new(-p.X, -p.Y);
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    public static Position operator -(Position a, (int X, int Y) b) => new(a.X - b.X, a.Y - b.Y);
    public static Position operator *(Position a, Position b) => new(a.X * b.X, b.Y * b.Y);
    public static Position operator *(Position a, (int X, int Y) b) => new(a.X * b.X, b.Y * b.Y);
    public static Position operator /(Position a, Position b) => new(a.X / b.X, b.Y / b.Y);
    public static Position operator /(Position a, (int X, int Y) b) => new(a.X / b.X, b.Y / b.Y);

    public int ManhattanDistance(Position other) => (X, Y).ManhattanDistance((other.X, other.Y));
    public double DirectDistance(Position other) => (X, Y).DirectDistance((other.X, other.Y));
    public int ChessDistance(Position other) => (X, Y).ChessDistance((other.X, other.Y));

    public bool IsBetween(Position min, Position max)
        => min.X <= X && X <= max.X
        && min.Y <= Y && Y <= max.Y;

    public bool IsOutsideBounds(Position min, Position max)
        => min.X > X || X > max.X
        || min.Y > Y || Y > max.Y;

    public IEnumerable<Position> GetNeighbours(bool includeDiagonal = false) => includeDiagonal ? GetAllNeighbours() : GetAdjacent();

    public static readonly (int X, int Y)[] CartesianDirections = [(-1, 0), (0, -1), (1, 0), (0, 1)];
    public static readonly (int X, int Y)[] AllDirections = [(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)];

    private IEnumerable<Position> GetAdjacent()
    {
        foreach (var dir in CartesianDirections)
            yield return this + dir;
    }

    private IEnumerable<Position> GetAllNeighbours()
    {
        foreach (var dir in AllDirections)
            yield return this + dir;
    }
}