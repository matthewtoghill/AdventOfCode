using System.Diagnostics;
using System.Reflection;
using Spectre.Console;

AnsiConsole.Write(new FigletText("Advent of Code").LeftJustified().Color(Color.Red));

var solutions = Assembly.Load("AdventOfCode")
                        .GetTypes()
                        .Where(x => x.GetTypeInfo().IsClass
                                 && !x.GetTypeInfo().IsAbstract
                                 && typeof(Solver).IsAssignableFrom(x))
                        .OrderBy(x => x.FullName)
                        .ToArray();

var names = solutions.Select(x => x.FullName).ToArray();
var years = names.Select(x => x!.Split('.')[1]).Distinct().ToArray();

string GetYearChoice()
    => AnsiConsole.Prompt(new SelectionPrompt<string>()
                  .EnableSearch()
                  .Title("\n\nChoose a Year")
                  .PageSize(15)
                  .AddChoices(["All", .. years.OrderDescending().Select(x => x[1..]), "Exit"]));

string GetDayChoice(string[] days)
    => AnsiConsole.Prompt(new SelectionPrompt<string>()
                  .EnableSearch()
                  .Title("Choose a Day")
                  .PageSize(30)
                  .AddChoices(["All", "Latest", .. days.Select(x => x[^2..]), "Exit"]));

string[] GetAvailableDays(string year)
    => names.Where(x => x!.Contains($".Y{year}.")).Select(x => x!.Split('.')[2]).Distinct().ToArray();

while (true)
{
    var yearChoice = GetYearChoice();
    switch (yearChoice)
    {
        case "All":
            RunAllYears();
            continue;
        case "Exit":
            return;
    }

    var days = GetAvailableDays(yearChoice);
    var dayChoice = GetDayChoice(days);

    switch (dayChoice)
    {
        case "All":
            RunYear(yearChoice);
            break;
        case "Latest":
            RunDay(yearChoice, days[^1][^2..]);
            break;
        case "Exit":
            return;
        default:
            RunDay(yearChoice, dayChoice);
            break;
    }
}

void RunAllYears()
{
    Dictionary<string, TimeSpan> yearTimes = [];
    Stopwatch allSW = Stopwatch.StartNew();

    foreach (var year in years)
        yearTimes.Add(year[1..], RunYear(year[1..]));

    allSW.Stop();

    var yearTimesTable = new Table().AddColumns("Year", "Elapsed");

    foreach (var year in years)
        yearTimesTable.AddRow(year[1..], GetElapsedColour(yearTimes[year[1..]]));

    yearTimesTable.Columns[0].Footer("Total");
    yearTimesTable.Columns[1].Footer(GetElapsedColour(allSW.Elapsed));
    AnsiConsole.Write(yearTimesTable);
}

TimeSpan RunYear(string year)
{
    AnsiConsole.Write(new FigletText(year).LeftJustified().Color(Color.Green));
    Stopwatch yearSW = Stopwatch.StartNew();

    var table = new Table().AddColumns("Day", "Part 1", "Part 2", "Elapsed");

    AnsiConsole.Live(table)
        .AutoClear(false)
        .Start(ctx =>
        {
            foreach (var day in GetAvailableDays(year))
            {
                RunDayTable(year, day[^2..], table);
                ctx.Refresh();
            }

            yearSW.Stop();
            table.Columns[^1].Footer(GetElapsedColour(yearSW.Elapsed));
            ctx.Refresh();
        });

    //AnsiConsole.MarkupLine($"Year Elapsed: {yearSW.Elapsed}\n\n");
    return yearSW.Elapsed;
}

void RunDayTable(string year, string day, Table table)
{
    var item = solutions.FirstOrDefault(x => x.FullName!.Contains($".Y{year}.Day{day}"));
    if (item is null)
    {
        table.AddRow(day, "N/A", "N/A", "[red]N/A[/]");
        return;
    }

    var instance = (Solver)Activator.CreateInstance(item)!;

    Stopwatch sw = Stopwatch.StartNew();

    instance.Run();
    sw.Stop();

    table.AddRow(day, $"{instance.Part1Solution}", $"{instance.Part2Solution}", GetElapsedColour(sw.Elapsed));
}

void RunDay(string year, string day)
{
    var item = solutions.FirstOrDefault(x => x.FullName!.Contains($".Y{year}.Day{day}"));
    if (item is null)
    {
        AnsiConsole.MarkupLine($"[bold red]Solution not found for: {year} - {day}[/]");
        return;
    }

    var instance = (Solver)Activator.CreateInstance(item)!;

    AnsiConsole.MarkupLine($"[bold]Year:[/]\t{instance.Year}");
    AnsiConsole.MarkupLine($"[bold]Day:[/]\t{instance.Day:00}\n");

    Stopwatch sw = Stopwatch.StartNew();

    instance.Run();
    sw.Stop();

    AnsiConsole.MarkupLine($"Part 1: {instance.Part1Solution}");
    AnsiConsole.MarkupLine($"Part 2: {instance.Part2Solution}");
    AnsiConsole.MarkupLine($"\nElapsed: {GetElapsedColour(sw.Elapsed)}\n\n");
}

string GetElapsedColour(TimeSpan elapsed)
    => elapsed.TotalSeconds switch
    {
        < 1 => $"[green]{elapsed}[/]",
        < 2 => $"[darkorange]{elapsed}[/]",
        _ => $"[red]{elapsed}[/]"
    };
