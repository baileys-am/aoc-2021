static void PartOne(string filepath)
{
    var entries = File.ReadAllLines(filepath).Select(l => new DisplayEntry(l)).ToList();
    var uniqueOutputs = 0;
    entries.ForEach(e => 
    {
        uniqueOutputs += e.TallyUniquesInOutput();
    });
    Console.WriteLine($"Outputs: {uniqueOutputs}");
    // Answer is 284
}

static void PartTwo(string filepath)
{
    var entries = File.ReadAllLines(filepath).Select(l => new DisplayEntry(l)).ToList();
    var sum = 0;
    entries.ForEach(e => 
    {
        sum += e.SumOutput();
    });
    Console.WriteLine($"Sum: {sum}");
    // Answer is 973499
}

var filepath = "../inputs/day08.txt";
PartOne(filepath);
PartTwo(filepath);


class DisplayEntry
{
    private string _rawSignalPattern;
    private string _rawOutput;

    public Dictionary<char, char> Decoder { get; } = new Dictionary<char, char>();

    public Dictionary<string, int> Pattern { get; private set; } = new Dictionary<string, int>()
    {
        { "abcefg", 0 },
        { "cf", 1 },
        { "acdeg", 2 },
        { "acdfg", 3 },
        { "bcdf", 4 },
        { "abdfg", 5 },
        { "abdefg", 6 },
        { "acf", 7 },
        { "abcdefg", 8 },
        { "abcdfg", 9 }
    };

    public DisplayEntry(string entry)
    {
        var splits = entry.Split(" | ");
        this._rawSignalPattern = splits[0];
        this._rawOutput = splits[1];

        var one = new List<char>();
        var four = new List<char>();
        var seven = new List<char>();
        var eight = new List<char>();
        var twoThreeFive = new List<char>();
        var zeroSixNine = new List<char>();
        foreach (var value in this._rawSignalPattern.Split(" "))
        {
            switch (value.Length)
            {
                case 2: // 1
                    one.AddRange(value.ToList());
                    break;
                case 4: // 4
                    four.AddRange(value.ToList());
                    break;
                case 3: // 7
                    seven.AddRange(value.ToList());
                    break;
                case 7: // 8
                    eight.AddRange(value.ToList());
                    break;
                case 5: // 2,3,5
                    twoThreeFive.AddRange(value.ToList());
                    break;
                case 6: // 0,6,9
                    zeroSixNine.AddRange(value.ToList());
                    break;
            }
        }

        var newA = seven.Where(c => !one.Contains(c)).First();
        var newE = twoThreeFive.Where(c => twoThreeFive.Where(o => o == c).Count() <= 1 && !four.Contains(c)).First();
        var newB = twoThreeFive.Where(c => twoThreeFive.Where(o => o == c).Count() <= 1 && c != newE).First();
        var newG = eight.Where(c => !seven.Contains(c) && !four.Contains(c) && c != newE).First();
        var newD = eight.Where(c => !seven.Contains(c) && c != newB && c != newE && c != newG).First();
        var newC = zeroSixNine.Where(c => c != newA && c != newB && c != newD && c != newE && c != newG).GroupBy(c => c).First(c => c.Count() == 2).Key;
        var newF = zeroSixNine.Where(c => c != newA && c != newB && c != newD && c != newE && c != newG).GroupBy(c => c).First(c => c.Count() == 3).Key;

        this.Decoder[newA] = 'a';
        this.Decoder[newB] = 'b';
        this.Decoder[newC] = 'c';
        this.Decoder[newD] = 'd';
        this.Decoder[newE] = 'e';
        this.Decoder[newF] = 'f';
        this.Decoder[newG] = 'g';
    }

    public int TallyUniquesInOutput()
    {
        var values = this._rawOutput.Split(" ");
        var tally = 0;
        foreach (var value in values)
        {
            if (value.Length == 2 ||    // 1
                value.Length == 4 ||    // 4
                value.Length == 3 ||    // 7
                value.Length == 7)      // 8
            {
                tally++;
            }
        }
        return tally;
    }

    public int SumOutput()
    {
        var values = this._rawOutput.Split(" ");
        var sum = "";
        foreach (var value in values)
        {
            var code = new string(value.Select(c => this.Decoder[c]).OrderBy(c => c).ToArray());
            sum += $"{this.Pattern[code]}";
        }
        return int.Parse(sum);
    }
}