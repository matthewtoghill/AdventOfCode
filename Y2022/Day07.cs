namespace AdventOfCode.Y2022;

internal class Day07() : Solver(2022, 7)
{
    public override void Run()
    {
        var folderSizes = GetFolderSizes(Input.ReadAllLines());
        Part1Solution = folderSizes.Where(x => x <= 100_000).Sum();
        Part2Solution = folderSizes.Where(x => x >= (folderSizes[0] - 40_000_000)).Min();
    }

    private static List<int> GetFolderSizes(string[] input)
    {
        var currentDirectoryPath = new Stack<string>(["root"]);
        var directories = new Dictionary<string, int>();

        foreach (var line in input.Skip(1))
        {
            switch (line.SplitOn(" "))
            {
                // go up one folder
                case ["$", "cd", ".."]:
                    currentDirectoryPath.Pop();
                    break;

                // enter the sub directory and specify it's full path
                case ["$", "cd", var name]:
                    currentDirectoryPath.Push($"{currentDirectoryPath.Peek()}/{name}");
                    break;

                // ignore ls and dir statements
                case ["$", "ls"]:
                case ["dir", _]:
                    break;

                // add the file size to the directory and all parent directories
                case [var size, _]:
                    var sizeVal = int.Parse(size);
                    currentDirectoryPath.ForEach(x => directories.IncrementAt(x, sizeVal));
                    break;
            }
        }

        return directories.Values.ToList();
    }
}
