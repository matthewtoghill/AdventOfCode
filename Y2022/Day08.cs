namespace AdventOfCode.Y2022;

internal class Day08() : GridSolver(2022, 8)
{
    public override void Run()
    {
        (Part1Solution, Part2Solution) = SurveyTrees();
    }

    private (int visibleCount, int scenicScore) SurveyTrees()
    {
        var visibleCount = 0;
        var maxScore = 0;

        foreach (var tree in GridMap)
        {
            var isVisible = false;
            var score = 1;

            foreach (var dir in "NESW")
            {
                int distance = 1;
                var next = tree.Key;

                while (true)
                {
                    next = next.MoveInDirection(dir);
                    if (!GridMap.TryGetValue(next, out var val))
                    {
                        score *= distance - 1;
                        isVisible = true;
                        break;
                    }

                    if (val >= tree.Value)
                    {
                        score *= distance;
                        break;
                    }
                    distance++;
                }
            }

            maxScore = Math.Max(maxScore, score);
            if (isVisible) visibleCount++;
        }

        return (visibleCount, maxScore);
    }
}
