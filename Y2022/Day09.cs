using AdventOfCode.Tools.Models;

namespace AdventOfCode.Y2022;

internal class Day09() : Solver(2022, 9)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();

        Part1Solution = SimulateRope(input, 2);
        Part2Solution = SimulateRope(input, 10);
    }

    private static int SimulateRope(string[] input, int knotCount)
    {
        var knots = new Position[knotCount];
        HashSet<Position> visited = [knots[^1]];

        foreach (var line in input)
        {
            var steps = int.Parse(line[1..]);

            for (int i = 0; i < steps; i++)
            {
                knots[0] = knots[0].MoveInDirection(line[0]);

                for (int k = 1; k < knots.Length; k++)
                    knots[k] = MoveFollower(knots[k - 1], knots[k]);

                visited.Add(knots[^1]);
            }
        }

        return visited.Count;
    }

    private static Position MoveFollower(Position leader, Position follower)
    {
        var distance = leader.ChessDistance(follower);

        if (distance <= 1)
            return follower;

        var (distX, distY) = leader - follower;
        return new(follower.X + Math.Sign(distX), follower.Y + Math.Sign(distY));
    }
}
