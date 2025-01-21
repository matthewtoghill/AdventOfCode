using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;

internal class Day17() : Solver(2022, 17)
{
    public override void Run()
    {
        var jetPattern = Input.ReadAll().ToCharArray();
        Part1Solution = GetTowerHeight(2022, jetPattern);
        Part2Solution = GetTowerHeight(1_000_000_000_000, jetPattern);
    }

    private static long GetTowerHeight(long totalRocksToAdd, char[] jetPattern)
    {
        var tower = new HashSet<Position>();
        var towerPattern = new Dictionary<(int jetIndex, int rockIndex), (long rocks, int height)>();
        var jetIndex = 0;
        var height = 0;
        var rockCount = 0L;

        while (rockCount < totalRocksToAdd)
        {
            var rock = GetRock(rockCount++, height);
            while (true)
            {
                if (jetPattern[jetIndex] == '<')
                {
                    if (rock.Min(b => b.X) > 1)
                    {
                        rock = rock.MoveAllInDirection('l');

                        if (rock.Any(tower.Contains))
                            rock.MoveAllInDirection('r');
                    }
                }
                else
                {
                    if (rock.Max(b => b.X) < 7)
                    {
                        rock = rock.MoveAllInDirection('r');

                        if (rock.Any(tower.Contains))
                            rock.MoveAllInDirection('l');
                    }
                }

                jetIndex = (jetIndex + 1) % jetPattern.Length;
                rock = rock.MoveAllInDirection('u'); // move down (here, up is 'down')

                // check for collision
                if (rock.Any(tower.Contains) || rock.Any(p => p.Y <= 0))
                {
                    rock.MoveAllInDirection('d'); // move up (here, down is 'up')
                    break;
                }
            }

            // Add the settled rock to the tower and get the new tower height
            rock.ForEach(b => tower.Add(b));
            height = tower.Max(b => b.Y);

            if (rockCount < 5)
                continue;

            // Check for repeating tower pattern where the combination of the jetIndex and rockIndex repeat
            // - Store the rockCount and tower height for the first instance of the jet and rock index combinations
            // - Once the combination is found again:
            //   - check if the total rocks left to add is evenly divisible by the pattern length (rockCount - r1)
            //   - if so, then calculate and return the total tower height
            var key = (jetIndex, (int)(rockCount % 5));
            if (!towerPattern.TryAdd(key, (rockCount, height)))
            {
                var (r1, h1) = towerPattern[key];
                if ((totalRocksToAdd - r1) % (rockCount - r1) == 0)
                    return ((totalRocksToAdd - r1) / (rockCount - r1) * (height - h1)) + h1;
            }
        }

        return height;
    }

    private static Position[] GetRock(long index, int height)
        => (index % 5) switch
        {
            0 => [new(3, height + 4), new(4, height + 4), new(5, height + 4), new(6, height + 4)],
            1 => [new(4, height + 4), new(3, height + 5), new(4, height + 5), new(5, height + 5), new(4, height + 6)],
            2 => [new(3, height + 4), new(4, height + 4), new(5, height + 4), new(5, height + 5), new(5, height + 6)],
            3 => [new(3, height + 4), new(3, height + 5), new(3, height + 6), new(3, height + 7)],
            4 => [new(3, height + 4), new(4, height + 4), new(3, height + 5), new(4, height + 5)],
            _ => throw new NotSupportedException()
        };
}
