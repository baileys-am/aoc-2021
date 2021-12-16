static void PartOne(string filepath)
{
    var system = new CaveSystem(filepath);
    var paths = system.CalculatePaths(1);
    Console.WriteLine($"Paths: {paths}");
    // Answer is 4411
}

static void PartTwo(string filepath)
{
    var system = new CaveSystem(filepath);
    var paths = system.CalculatePaths(2);
    Console.WriteLine($"Paths: {paths}");
    // Answer is 136767
}

var filepath = "../inputs/day12.txt";
PartOne(filepath);
PartTwo(filepath);


class Cave
{
    private const string StartCaveName = "start";
    private const string EndCaveName = "end";

    public string Name { get; }
    public bool IsBig { get; }
    public List<Cave> Connections { get; } = new List<Cave>();

    public Cave(string name)
    {
        this.Name = name;
        this.IsBig = char.IsUpper(name[0]) || name == StartCaveName || name == EndCaveName;
    }
}

class CaveSystem
{
    private const string StartCaveName = "start";
    private const string EndCaveName = "end";

    private readonly Dictionary<string, Cave> _caves = new Dictionary<string, Cave>();

    public CaveSystem(string filepath)
    {
        var lines = File.ReadAllLines(filepath);
        this._caves = new Dictionary<string, Cave>();
        foreach (var line in lines)
        {
            var split = line.Split("-");
            var startCaveName = split[0];
            var endCaveName = split[1];

            Cave startCave = this._caves.ContainsKey(startCaveName) ? this._caves[startCaveName] : this._caves[startCaveName] = new Cave(startCaveName);
            Cave endCave = this._caves.ContainsKey(endCaveName) ? this._caves[endCaveName] : this._caves[endCaveName] = new Cave(endCaveName);
            startCave.Connections.Add(endCave);
            endCave.Connections.Add(startCave);
        }

        // foreach (var kvp in this._caves)
        // {
        //     Console.Write($"{kvp.Value.Name} ");
        //     foreach (var connection in kvp.Value.Connections)
        //     {
        //         Console.Write($"{connection.Name} ");
        //     }
        //     Console.WriteLine();
        // }
    }

    public int CalculatePaths(int maxSmallCaveVisit)
    {
        return this.CalculateCavePaths(new List<Cave>() { this._caves[StartCaveName] }, maxSmallCaveVisit);
    }

    private int CalculateCavePaths(List<Cave> path, int maxSmallCaveVisit)
    {
        int paths = 0;
        var currentCave = path.Last();
        foreach (var nextCave in currentCave.Connections)
        {
            if (nextCave.Name == EndCaveName)
            {
                // foreach (var cave in path)
                // {
                //     Console.Write($"{cave.Name},");
                // }
                // Console.WriteLine("end");
                paths++;
            }
            else if (nextCave.Name == StartCaveName ||
                    (!nextCave.IsBig && path.Where(c => c.Name == nextCave.Name).Count() >= maxSmallCaveVisit) ||
                    (!nextCave.IsBig && path.Where(c => c.Name == nextCave.Name).Count() == 1 && path.Where(c => !c.IsBig && c.Name != nextCave.Name).GroupBy(c => c.Name).Any(g => g.Count() >= maxSmallCaveVisit)))
            {
                // Next move goes to start
                // Next move goes to small one and we have already visited another small one over max
                continue;
            }
            else
            {
                // Console.WriteLine($"Add {nextCave.Name}");
                path.Add(nextCave);
                paths += CalculateCavePaths(path, maxSmallCaveVisit);
            }
        }
        // Console.WriteLine($"Remove {currentCave.Name}");
        path.RemoveAt(path.Count - 1);
        return paths;
    }
}