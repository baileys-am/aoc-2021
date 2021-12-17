static void PartOne(string filepath)
{
    var polymer = new Polymer(filepath);
    var score = polymer.Step(10);
    Console.WriteLine($"Score: {score}");
    // Answer is 2703
}

static void PartTwo(string filepath)
{
    var polymer = new Polymer(filepath);
    var score = polymer.Step(40);
    Console.WriteLine($"Score: {score}");
    // Answer is 3497279528313
}

var filepath = "../inputs/day14.txt";
PartOne(filepath);
PartTwo(filepath);


class InsertionRule
{
    public string Pair { get; }
    public char NewElement { get; }

    public InsertionRule(string line)
    {
        var split = line.Split(" -> ");
        this.Pair = split[0];
        this.NewElement = split[1][0];
    }
}

class Polymer
{
    private readonly Dictionary<string, long> _pairCount = new Dictionary<string, long>();
    public string Template { get; private set; }
    public List<InsertionRule> Rules { get; } = new List<InsertionRule>();

    public Polymer(string filepath)
    {
        var lines = File.ReadAllLines(filepath);
        this.Template = lines[0];
        for (int i = 0; i < this.Template.Length - 1; i++)
        {
            var pair = this.Template.Substring(i, 2);
            if (this._pairCount.ContainsKey(pair))
            {
                this._pairCount[pair] += 1;
            }
            else
            {
                this._pairCount[pair] = 1;
            }
        }
        foreach (var line in lines.Skip(2))
        {
            this.Rules.Add(new InsertionRule(line));
        }
    }

    public long Step(int times)
    {
        long score = 0;
        for (int i = 0; i < times; i++)
        {
            score = this.StepSmartly();
        }
        return score;
    }

    public long Step()
    {
        var insertions = new Dictionary<int, InsertionRule>();
        foreach (var rule in this.Rules)
        {
            for (int i = 0; i < this.Template.Length - 1; i++)
            {
                if (this.Template.Substring(i, rule.Pair.Length) == rule.Pair)
                {
                    insertions[i+1] = rule;
                }
            }
        }

        if (insertions.Any())
        {
            var newTemplate = this.Template.ToList();
            foreach (var key in insertions.Keys.OrderBy(k => k))
            {
                int offset = newTemplate.Count - this.Template.Length;
                newTemplate.Insert(key + offset, insertions[key].NewElement);
            }
            this.Template = new string(newTemplate.ToArray());
        }
        // Console.WriteLine(this.Template);

        long leastCommonCount = int.MaxValue;
        long mostCommonCount = 0;
        foreach (var groupedElements in this.Template.GroupBy(c => c))
        {
            leastCommonCount = Math.Min(leastCommonCount, groupedElements.Count());
            mostCommonCount = Math.Max(mostCommonCount, groupedElements.Count());
        }
        long score = mostCommonCount - leastCommonCount;
        return score;
    }

    public long StepSmartly()
    {
        var pairsToRemove = new List<string>();
        var pairsToAdd = new Dictionary<string, long>();
        foreach (var rule in this.Rules)
        {
            if (this._pairCount.ContainsKey(rule.Pair))
            {
                var count = this._pairCount[rule.Pair];
                var newPair1 = $"{rule.Pair[0]}{rule.NewElement}";
                var newPair2 = $"{rule.NewElement}{rule.Pair[1]}";
                pairsToAdd[newPair1] = pairsToAdd.ContainsKey(newPair1) ? pairsToAdd[newPair1] + count : count;
                pairsToAdd[newPair2] = pairsToAdd.ContainsKey(newPair2) ? pairsToAdd[newPair2] + count : count;
                pairsToRemove.Add(rule.Pair);
            }
        }

        // Console.Write("Before: ");
        // foreach (var kvp in this._pairCount)
        // {
        //     Console.Write($"{kvp.Key}({kvp.Value}) ");
        // }
        // Console.WriteLine();
        foreach (var pair in pairsToRemove)
        {
            this._pairCount.Remove(pair);
        }
        foreach (var kvp in pairsToAdd)
        {
            this._pairCount[kvp.Key] = this._pairCount.ContainsKey(kvp.Key) ? this._pairCount[kvp.Key] + kvp.Value : kvp.Value;
        }
        // Console.Write("After: ");
        // foreach (var kvp in this._pairCount)
        // {
        //     Console.Write($"{kvp.Key}({kvp.Value}) ");
        // }
        // Console.WriteLine();

        var elements = this._pairCount.Keys.Aggregate((k1, k2) => $"{k1}{k2}").ToArray().Distinct();
        var elementCount = new Dictionary<char, long>();
        foreach (var element in elements)
        {
            elementCount[element] = 0;
            foreach (var kvp in this._pairCount)
            {
                if (kvp.Key[0] == element)
                {
                    elementCount[element] += kvp.Value;
                }
            }
        }
        elementCount[this.Template.Last()]++;

        long leastCommonCount = long.MaxValue;
        long mostCommonCount = 0;
        foreach (var kvp in elementCount)
        {
            // Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            leastCommonCount = Math.Min(leastCommonCount, kvp.Value);
            mostCommonCount = Math.Max(mostCommonCount, kvp.Value);
        }
        // Console.WriteLine($"{mostCommonCount} - {leastCommonCount}");
        long score = mostCommonCount - leastCommonCount;
        return score;
    }
}