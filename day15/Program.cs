static void PartOne(string filepath)
{
    var cavern = new Cavern(filepath, 1);
    cavern.Print();
    var path = cavern.SearchLowestRiskPath(new Coord(0, 0), new Coord(cavern.XDim - 1, cavern.YDim - 1));
    Console.WriteLine();
    cavern.PrintPath(path);
    // Answer is 656
}

static void PartTwo(string filepath)
{
    var cavern = new Cavern(filepath, 5);
    cavern.Print();
    var startTime = DateTime.Now;
    var path = cavern.SearchLowestRiskPath(new Coord(0, 0), new Coord(cavern.XDim - 1, cavern.YDim - 1));
    var endTime = DateTime.Now;
    Console.WriteLine();
    cavern.PrintPath(path);
    Console.WriteLine($"Total time: {endTime  - startTime}");
    // Answer is 2979
}

var filepath = "../inputs/day15.txt";
// PartOne(filepath);
PartTwo(filepath);


class Coord
{
    public int X { get; }
    public int Y { get; }
    public Coord? Parent { get; set; } = null;
    public int G { get; set; }
    public int H { get; set; }
    public int F => this.G + this.H;

    public Coord(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public bool IsSame(Coord other)
    {
        return this.X == other.X && this.Y == other.Y;
    }

    public bool IsSame(int x, int y)
    {
        return this.X == x && this.Y == y;
    }
}

class Cavern
{
    private readonly int[,] _map;
    public int Repeat { get; }
    public int XDim { get; }
    public int YDim { get; }

    public Cavern(string filepath, int repeat)
    {
        var lines = File.ReadAllLines(filepath);
        this.XDim = lines[0].Length * repeat;
        this.YDim = lines.Length * repeat;
        this._map = new int[this.YDim / repeat, this.XDim / repeat];
        this.Repeat = repeat;

        for (int y = 0; y < this.YDim / repeat; y++)
        {
            for (int x = 0; x < this.XDim / repeat; x++)
            {
                this._map[y, x] = int.Parse(lines[y][x].ToString());
            }
        }
    }

    public List<Coord> SearchLowestRiskPath(Coord start, Coord end)
    {
        var openList = new List<Coord>() { start };
        var closedList = new List<Coord>();

        Coord? endCoord = null;
        while (openList.Any())
        {
            var minCost = openList.Select(c => c.F).Min();
            var parentIndex = openList.FindMinIndex(c => c.F == minCost);
            var parent = openList[parentIndex];
            openList.RemoveAt(parentIndex);

            if (parent.IsSame(end))
            {
                endCoord = parent;
                break;
            }

            foreach (var child in this.GetCoordChildren(parent))
            {
                child.Parent = parent;
                child.G = 0;//parent.G + Math.Abs(child.X - parent.X) + Math.Abs(child.Y - parent.Y);
                child.H = parent.H + this.GetCoordRisk(child);// + Math.Abs(child.X - end.X) + Math.Abs(child.Y - end.Y);

                int otherOpenIndex = openList.FindMinIndex(c => c.IsSame(child));
                int otherClosedIndex = closedList.FindMinIndex(c => c.IsSame(child));
                if (otherOpenIndex >= 0 && openList[otherOpenIndex].F <= child.F)
                {
                    continue;
                }
                else if (otherClosedIndex >= 0 && closedList[otherClosedIndex].F <= child.F)
                {
                    continue;
                }
                else
                {
                    openList.Add(child);
                }
            }

            closedList.Add(parent);
        }

        return this.TraverseParents(endCoord);
    }

    public void Print()
    {
        for (int y = 0; y < this.YDim; y++)
        {
            for (int x = 0; x < this.XDim; x++)
            {
                Console.Write(this.GetCoordRisk(new Coord(x, y)));
            }
            Console.WriteLine();
        }
    }

    public void PrintPath(List<Coord> path)
    {
        for (int y = 0; y < this.YDim; y++)
        {
            for (int x = 0; x < this.XDim; x++)
            {
                if (path.Any(c => c.IsSame(x, y)))
                {
                    Console.Write(this.GetCoordRisk(new Coord(x, y)));
                    if (path.Where(c => c.IsSame(x, y)).Count() > 1)
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    Console.Write("-");
                }
            }
            Console.WriteLine();
        }

        var pathStr = path.Select(c => $"{c.X},{c.Y}").ToList().Aggregate((c1, c2) => $"{c1} -> {c2}");
        var risk = path.Skip(1).Select(c => this.GetCoordRisk(c)).Sum();
        Console.WriteLine($"{pathStr}");
        Console.WriteLine($"Risk: {risk}");
    }

    private List<Coord> GetCoordChildren(Coord parent)
    {
        var children = new List<Coord>();
        if (parent.X - 1 >= 0) // left
        {
            children.Add(new Coord(parent.X - 1, parent.Y));
        }
        if (parent.X + 1 < this.XDim) // right
        {
            children.Add(new Coord(parent.X + 1, parent.Y));
        }
        if (parent.Y - 1 >= 0) // top
        {
            children.Add(new Coord(parent.X, parent.Y - 1));
        }
        if (parent.Y + 1 < this.YDim) // bottom
        {
            children.Add(new Coord(parent.X, parent.Y + 1));
        }
        return children;
    }

    private int GetCoordRisk(Coord coord)
    {
        int mappedX = coord.X % (this.XDim / this.Repeat);
        int xAdditive = coord.X * this.Repeat / this.XDim;
        int mappedY = coord.Y % (this.YDim / this.Repeat);
        int yAdditive = coord.Y * this.Repeat / this.YDim;
        var risk = this._map[mappedY, mappedX] + xAdditive + yAdditive;
        return risk <= 9 ? risk : risk % 9;
    }

    private List<Coord> TraverseParents(Coord? end)
    {
        var parentChain = new List<Coord>();
        while (end != null)
        {
            parentChain.Add(end);
            end = end.Parent;
        }
        parentChain.Reverse();
        return parentChain;
    }
}

static class ListExtensions
{
    public static int FindMinIndex<T>(this List<T> list, Func<T, bool> predicate)
    {
        int index = list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
            {
                index = Math.Min(i, index);
            }
        }
        return index == list.Count ? -1 : index;
    }
}