namespace AdventOfCode.Y2024;

internal class Day09() : Solver(2024, 9)
{
    public override void Run()
    {
        var input = Input.ReadAll();
        Part1Solution = Part1(input);
        Part2Solution = Part2(input);
    }

    private static long Part1(string input)
    {
        var id = 0;
        var totalFileSize = 0;
        List<int> files = [];
        Stack<int> fileStack = [];

        for (int i = 0; i < input.Length; i++)
        {
            var size = input[i].ToInt();
            if (i % 2 == 0)
            {
                totalFileSize += size;
                Enumerable.Range(0, size).ForEach(_ =>
                {
                    files.Add(id);
                    fileStack.Push(id);
                });
                id++;
            }
            else
            {
                files.AddRange(Enumerable.Repeat(-1, size));
            }
        }

        int[] disk = new int[totalFileSize];

        for (int i = 0; i < disk.Length; i++)
        {
            if (files[i] == -1)
            {
                disk[i] = fileStack.Pop();
                files.RemoveAt(files.Count - 1);
            }
            else
            {
                disk[i] = files[i];
            }
        }

        return CalculateChecksum(disk);
    }

    private static long Part2(string input)
    {
        var id = 0;
        var totalFileSize = 0;
        Queue<File> sizesQueue = [];
        List<File> unallocatedFiles = [];

        for (int i = 0; i < input.Length; i++)
        {
            var size = input[i] - '0';
            if (i % 2 == 0)
            {
                totalFileSize += size;
                sizesQueue.Enqueue(new(id, size));
                unallocatedFiles.Add(new(id, size));
                id++;
            }
            else
            {
                totalFileSize += size;
                sizesQueue.Enqueue(new(-1, size));
            }
        }

        int[] disk = new int[totalFileSize];

        for (int i = 0; i < disk.Length; i++)
        {
            if (sizesQueue.Count == 0) break;
            var current = sizesQueue.Dequeue();
            if (current.Id == -1)
            {
                var freeSize = current.Size;
                var unfilledFreeSize = freeSize;
                List<File> itemsToMove = [];

                do
                {
                    var file = unallocatedFiles.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Size <= freeSize, new(-2, 0));
                    if (file.Id == -2) break;
                    freeSize -= file.Size;
                    unallocatedFiles.Remove(file);
                    itemsToMove.Add(new(file.Id, file.Size));
                }
                while (freeSize > 0);

                itemsToMove.ForEach(item =>
                {
                    Enumerable.Range(0, item.Size).ForEach(_ => {
                        disk[i++] = item.Id;
                        unfilledFreeSize--;
                    });
                });
                i += unfilledFreeSize - 1;
            }
            else
            {
                if (unallocatedFiles.Remove(current))
                {
                    Enumerable.Range(0, current.Size).ForEach(x => disk[i + x] = current.Id);
                }
                i += current.Size - 1;
            }
        }

        return CalculateChecksum(disk);
    }

    private static long CalculateChecksum(int[] disk) => disk.Sum((value, i) => (long)i * value);
}

internal record File(int Id, int Size);