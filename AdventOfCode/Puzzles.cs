using System.Drawing;

namespace AdventOfCode;

// ReSharper disable once UnusedType.Global
/// <summary>
/// https://adventofcode.com/2023
/// </summary>
public static class Puzzles
{
    /// <summary>
    /// The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.
    /// </summary>
    public static void One()
    {
        var lines = File.ReadAllLines("./Inputs/1-puzzle.txt");
        var total = lines
            .Select(l => $"{l.First(char.IsDigit)}{l.Last(char.IsDigit)}")
            .Select(int.Parse)
            .Sum();

        Console.WriteLine(total);
    }

    /// <summary>
    /// Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".
    /// </summary>
    public static void Two()
    {
        var lines = File.ReadAllLines("./Inputs/1-puzzle.txt");
        (string text, int value)[] digits =
        {
            ("one", 1), ("two", 2), ("three", 3),
            ("four", 4), ("five", 5), ("six", 6),
            ("seven", 7), ("eight", 8), ("nine", 9)
        };

        var sum = 0;
        foreach (var line in lines)
        {
            List<int> numbers = [];

            for (var index = 0; index < line.Length; index++)
            {
                if (int.TryParse(line.AsSpan(index, 1), out var n))
                {
                    numbers.Add(n);
                }
                else
                {
                    // check for text
                    foreach (var (text, value) in digits)
                    {
                        // avoid overshooting
                        var match = new string(line.Skip(index).Take(text.Length).ToArray());
                        if (match == text)
                        {
                            numbers.Add(value);
                            break;
                        }
                    }
                }
            }

            var secretNumber = $"{numbers[0]}{numbers[^1]}";
            sum += int.Parse(secretNumber);
        }

        Console.WriteLine(sum);
    }

    public static void Three()
    {
        var lines = File.ReadAllLines("./Inputs/2-puzzle.txt");
        Dictionary<int, List<List<(string color, int count)>>> games = new();

        // let's parse this business
        foreach (var line in lines)
        {
            var onColon = line.Split(":");
            var id = int.Parse(onColon[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            var bags = onColon[1].Split(";");

            List<List<(string color, int count)>> allBags = new();
            foreach (var bag in bags)
            {
                List<(string color, int count)> singleBag = new();
                var contents = bag.Split(",");
                singleBag.AddRange(
                    contents
                        .Select(content => content.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                        .Select(meta => (color: meta[1], count: int.Parse(meta[0])))
                );
                allBags.Add(singleBag);
            }

            games.Add(id, allBags);
        }
        
        // let's answer the question
        // Determine which games would have been possible if the bag had been loaded with only 12 red cubes, 13 green cubes, and 14 blue cubes. What is the sum of the IDs of those games?

        var results = new List<int>();
            
        foreach (var (id, game) in games)
        {
            var possible = true;
            foreach (var nope in from rounds in game 
                     let reds = rounds.Where(x => x.color == "red").Any(x => x.count > 12) 
                     let blues = rounds.Where(x => x.color == "blue").Any(x => x.count > 14) 
                     let greens = rounds.Where(x => x.color == "green").Any(x => x.count > 13) 
                     where greens || reds || blues 
                     select false)
            {
                possible = nope;
            }

            if (possible)
                results.Add(id);
        }

        var totalOfIds = results.Sum();
        Console.WriteLine(totalOfIds);
    }

    public static void Four()
    {
        var lines = File.ReadAllLines("./Inputs/2-puzzle.txt");
        Dictionary<int, List<List<(string color, int count)>>> games = new();

        // let's parse this business
        foreach (var line in lines)
        {
            var onColon = line.Split(":");
            var id = int.Parse(onColon[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            var bags = onColon[1].Split(";");

            List<List<(string color, int count)>> allBags = new();
            foreach (var bag in bags)
            {
                List<(string color, int count)> singleBag = new();
                var contents = bag.Split(",");
                singleBag.AddRange(
                    contents
                        .Select(content => content.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                        .Select(meta => (color: meta[1], count: int.Parse(meta[0])))
                );
                allBags.Add(singleBag);
            }

            games.Add(id, allBags);
        }
        
        // let's sum up the powers
        var results = new List<int>();
        foreach (var (_, game) in games)
        {
            var blues = game.SelectMany(r => r).Where(x => x.color == "blue").ToList();
            var reds = game.SelectMany(r => r).Where(x => x.color == "red").ToList();
            var greens = game.SelectMany(r => r).Where(x => x.color == "green").ToList();

            // account for empty sequences
            var power = (blues.Any() ? blues.Max(x => x.count) : 1) *
                        (greens.Any() ? greens.Max(x => x.count) : 1) *
                        (reds.Any() ? reds.Max(x => x.count) : 1);
            
            results.Add(power);
        }

        var powers = results.Sum();
        Console.WriteLine(powers);
    }
}