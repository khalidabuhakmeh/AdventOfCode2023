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
        (string text, int value)[] digits = {
            ("one", 1), ("two", 2), ("three", 3),
            ("four", 4), ("five", 5), ("six", 6),
            ("seven", 7), ("eight", 8), ("nine", 9)
        };
        
        var sum = 0;
        foreach (var line in lines)
        {
            List<int> numbers = [];

            for(var index = 0; index < line.Length; index++)
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
}