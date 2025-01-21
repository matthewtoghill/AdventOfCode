namespace AdventOfCode.Y2021;

internal class Day04() : Solver(2021, 4)
{
    public override void Run()
    {
        var input = Input.ReadAsParagraphs();
        var bingoNumbers = input[0].ExtractInts().ToArray();
        (Part1Solution, Part2Solution) = Solve(bingoNumbers, input.Skip(1).ToArray());
    }

    private static (int, int) Solve(int[] bingoNumbers, string[] input)
    {
        var boards = input.Select(x => new BingoBoard(x.SplitLines())).ToList();
        List<int> completedBoards = [];

        foreach (var ball in bingoNumbers)
        {
            for (int i = 0; i < boards.Count; i++)
            {
                if (boards[i].MarkNumber(ball))
                {
                    completedBoards.Add(boards[i].Score);
                    boards.RemoveAt(i);
                    i--;
                }
            }
        }

        return (completedBoards[0], completedBoards[^1]);
    }

    private class BingoNumber(int number)
    {
        public int Number { get; init; } = number;
        public bool IsMarked { get; set; }
    }

    private class BingoBoard
    {
        private Dictionary<int, BingoNumber> _numberSet = [];
        public BingoBoard(string[] board)
        {
            for (int row = 0; row < board.Length; row++)
            {
                var nums = board[row].ExtractInts().ToArray();
                for (int col = 0; col < nums.Length; col++)
                {
                    Numbers[row, col] = new BingoNumber(nums[col]);
                }
            }

            _numberSet = Numbers.Flatten().ToDictionary(x => x.Number, x => x);
        }

        public BingoNumber[,] Numbers { get; set; } = new BingoNumber[5, 5];
        public int Score { get; private set; }

        public bool MarkNumber(int number)
        {
            if (_numberSet.TryGetValue(number, out var bingoNumber))
                bingoNumber.IsMarked = true;

            if (HasBingo())
            {
                Score = SumUnmarked() * number;
                return true;
            }

            return false;
        }

        private bool HasBingo()
        {
            var rowHasBingo = Enumerable.Range(0, 5).Any(i => Numbers.SliceRow(i).All(b => b.IsMarked));
            var colHasBingo = Enumerable.Range(0, 5).Any(i => Numbers.SliceColumn(i).All(b => b.IsMarked));
            return rowHasBingo || colHasBingo;
        }

        private int SumUnmarked() => _numberSet.Where(x => !x.Value.IsMarked).Sum(x => x.Key);
    }
}
