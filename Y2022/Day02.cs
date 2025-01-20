namespace AdventOfCode.Y2022;

internal class Day02() : Solver(2022, 2)
{
    public override void Run()
    {
        var input = Input.ReadAllLines();
        Part1Solution = input.Sum(ScoreRoundPart1);
        Part2Solution = input.Sum(ScoreRoundPart2);
    }

    // Opponent: A = Rock       B = Paper       C = Scissors
    // Player:   X = Rock = 1   Y = Paper = 2   Z = Scissors = 3
    // Outcome:  Win = 6        Draw = 3        Lose = 0
    private int ScoreRoundPart1(string round) => round switch
    {
        "A X" => 1 + 3, // D: Rock vs Rock
        "A Y" => 2 + 6, // W: Rock vs Paper
        "A Z" => 3 + 0, // L: Rock vs Scissors
        "B X" => 1 + 0, // L: Paper vs Rock
        "B Y" => 2 + 3, // D: Paper vs Paper
        "B Z" => 3 + 6, // W: Paper vs Scissors
        "C X" => 1 + 6, // W: Scissors vs Rock
        "C Y" => 2 + 0, // L: Scissors vs Paper
        "C Z" => 3 + 3, // D: Scissors vs Scissors
        _ => 0
    };

    // Opponent: A = Rock       B = Paper       C = Scissors
    // Outcome:  X = Lose = 0   Y = Draw = 3    Z = Win = 6
    // Play:     Rock = 1       Paper = 2       Scissors = 3
    private int ScoreRoundPart2(string round) => round switch
    {
        "A X" => 0 + 3, // L: Rock with Scissors
        "A Y" => 3 + 1, // D: Rock with Rock
        "A Z" => 6 + 2, // W: Rock with Paper
        "B X" => 0 + 1, // L: Paper with Rock
        "B Y" => 3 + 2, // D: Paper with Paper
        "B Z" => 6 + 3, // W: Paper with Scissors
        "C X" => 0 + 2, // L: Scissors with Paper
        "C Y" => 3 + 3, // D: Scissors with Scissors
        "C Z" => 6 + 1, // W: Scissors with Rock
        _ => 0
    };
}
