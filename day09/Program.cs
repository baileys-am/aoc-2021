static void PartOne(string filepath)
{
    var floor = new Floor(filepath);
    int riskSum = floor.GetLowPoints().Select(p => p.Risk).Sum();
    Console.WriteLine($"Risk sum: {riskSum}");
    // Answer is 498
}

static void PartTwo(string filepath)
{
    var floor = new Floor(filepath);
    int riskSum = floor.GetBasins().Select(p => p.Size).OrderByDescending(s => s).Take(3).Aggregate(1, (acc, val) => acc * val);
    Console.WriteLine($"Answer: {riskSum}");
    // Answer is 1071000
}

var filepath = "../inputs/day09.txt";
PartOne(filepath);
PartTwo(filepath);


class Coord
{
    public int X { get; }
    public int Y { get; }
    public int Height { get; set; }
    public int Risk { get; set; }

    public Coord(int x, int y, int height)
    {
        this.X = x;
        this.Y = y;
        this.Height = height;
        this.Risk = this.Height + 1;
    }
}

class Basin
{
    public int Size { get; }

    public Basin(int size)
    {
        this.Size = size;
    }
}

class Floor
{
    public int XDim { get; }
    public int YDim { get; }
    public int[,] HeightMap { get; }

    public Floor(string filepath)
    {
        var lines = File.ReadAllLines(filepath);
        this.XDim = lines.First().Length;
        this.YDim = lines.Count();
        this.HeightMap = new int[this.YDim, this.XDim];
        for (int i = 0; i < this.YDim; i++)
        {
            var rowHeights = lines.ElementAt(i).Select(c => int.Parse(c.ToString()));
            for (int j = 0; j < this.XDim; j++)
            {
                this.HeightMap[i, j] = rowHeights.ElementAt(j);
            }
        }
    }

    public List<Coord> GetLowPoints()
    {
        var lowPoints = new List<Coord>();
        for (int i = 0; i < this.YDim; i++)
        {
            for (int j = 0; j < this.XDim; j++)
            {
                int height = this.HeightMap[i, j];
                bool lowPoint = true;
                if (i == 0)
                {
                    lowPoint &= this.HeightMap[i+1, j] > height;
                }
                else if (i == this.YDim - 1)
                {
                    lowPoint &= this.HeightMap[i-1, j] > height;
                }
                else
                {
                    lowPoint &= this.HeightMap[i+1, j] > height;
                    lowPoint &= this.HeightMap[i-1, j] > height;
                }

                if (j == 0)
                {
                    lowPoint &= this.HeightMap[i, j+1] > height;
                }
                else if (j == this.XDim - 1)
                {
                    lowPoint &= this.HeightMap[i, j-1] > height;
                }
                else
                {
                    lowPoint &= this.HeightMap[i, j+1] > height;
                    lowPoint &= this.HeightMap[i, j-1] > height;
                }

                if (lowPoint)
                {
                    lowPoints.Add(new Coord(j, i, height));
                }
            }
        }
        return lowPoints;
    }

    public List<Basin> GetBasins()
    {
        var basins = new List<Basin>();

        var lowPoints = this.GetLowPoints();
        foreach (var point in lowPoints)
        {
            var visitedPoints = new List<Coord>();
            var size = DiscoverBasinSize(visitedPoints, point, point);
            basins.Add(new Basin(size));
        }

        return basins;
    }

    private int DiscoverBasinSize(List<Coord> visitedPoints, Coord prev, Coord start)
    {
        if (start.Height == 9 || visitedPoints.Any(p => p.X == start.X && p.Y == start.Y))
        {
            return 0;
        }
        else
        {
            visitedPoints.Add(start);
            int size = 1;
            if (start.Y - 1 >= 0 && start.Y - 1 != prev.Y) // Look up
            {
                size += DiscoverBasinSize(visitedPoints, start, new Coord(start.X, start.Y - 1, this.HeightMap[start.Y - 1, start.X]));
            }
            if (start.Y + 1 < this.YDim && start.Y + 1 != prev.Y) // Look down
            {
                size += DiscoverBasinSize(visitedPoints, start, new Coord(start.X, start.Y + 1, this.HeightMap[start.Y + 1, start.X]));
            }
            if (start.X - 1 >= 0 && start.X -1 != prev.X) // Look left
            {
                size += DiscoverBasinSize(visitedPoints, start, new Coord(start.X - 1, start.Y, this.HeightMap[start.Y, start.X - 1]));
            }
            if (start.X + 1 < this.XDim && start.X + 1 != prev.X) // Look right
            {
                size += DiscoverBasinSize(visitedPoints, start, new Coord(start.X + 1, start.Y, this.HeightMap[start.Y, start.X + 1]));
            }
            // Console.WriteLine(size);
            return size;
        }
    }
}