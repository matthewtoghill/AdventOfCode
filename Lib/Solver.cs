using AdventOfCode.Tools.Models;

namespace AdventOfCode.Lib;

internal abstract class Solver
{
    public int Year { get; init; }
    public int Day { get; init; }

    public object Part1Solution { get; set; } = string.Empty;
    public object Part2Solution { get; set; } = string.Empty;

    protected Solver(int year, int day)
    {
        Year = year;
        Day = day;
        Input = new(year, day);
    }

    public static Input Input { get; private set; } = null!;

    public virtual void Run() { }
}

internal abstract class GridSolver : Solver
{
    internal Position MinPos { get; private set; }
    internal Position MaxPos { get; private set; }
    internal string[] Grid { get; private set; }
    internal Dictionary<Position, char> GridMap { get; private set; } = [];

    protected GridSolver(int year, int day) : base (year, day)
    {
        Grid = Input.ReadAllLines();
        GridMap = Grid.AsCharMap();
        SetMinMaxPos(Grid);
    }

    protected void SetMinMaxPos(string[] input)
    {
        MinPos = new(0, 0);
        MaxPos = new(input[0].Length - 1, input.Length - 1);
    }
}
