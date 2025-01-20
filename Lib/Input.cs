namespace AdventOfCode.Lib;

internal class Input(int year, int day)
{
    public string ReadAll() => File.ReadAllText(@$"..\..\..\data\advent-of-code-inputs\{year}\day{day:00}.txt");
    public string[] ReadAllLines() => File.ReadAllLines(@$"..\..\..\data\advent-of-code-inputs\{year}\day{day:00}.txt");

    public IEnumerable<T> ReadAllLinesTo<T>(StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries) where T : IParsable<T>
        => ReadAll().SplitLines(splitOptions)
                    .Select(x => T.Parse(x, null));

    public string[] ReadAsParagraphs(StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        => ReadAll().Split(["\n\n", "\r\n\r\n"], splitOptions);

    public T[,] ReadAsGrid<T>() where T : IParsable<T>
    {
        var lines = ReadAllLines();
        var grid = new T[lines.Length, lines[0].Length];
        for (int x = 0; x < lines.Length; x++)
        {
            for (int y = 0; y < lines[x].Length; y++)
            {
                grid[x, y] = T.Parse(lines[x][y].ToString(), null);
            }
        }
        return grid;
    }
}
