static void PartOne(string filepath)
{
    var map = new OctipiMap(filepath);
    int flashes = map.ObserveFlashes(800);
    map.PrintEnergyLevels();
    Console.WriteLine($"Flahes: {flashes}");
    // Answer is 
}

static void PartTwo(string filepath)
{
    // Answer is 517
}

var filepath = "../inputs/day11.txt";
PartOne(filepath);
PartTwo(filepath);


class OctipiMap
{
    private readonly int _rowDim;
    private readonly int _colDim;
    private readonly int[,] _energy;

    public OctipiMap(string filepath)
    {
        var lines = File.ReadAllLines(filepath);
        this._rowDim = lines.Count();
        this._colDim = lines.First().Length;
        this._energy = new int[this._rowDim, this._colDim];
        for (int r = 0; r < this._rowDim; r++)
        {
            for (int c = 0; c < this._colDim; c++)
            {
                this._energy[r,c] = int.Parse(lines[r][c].ToString());
            }
        }
    }

    public void PrintEnergyLevels()
    {
        for (int r = 0; r < this._rowDim; r++)
        {
            for (int c = 0; c < this._colDim; c++)
            {
                Console.Write(this._energy[r,c]);
            }
            Console.WriteLine();
        }
    }

    public int ObserveFlashes(int steps)
    {
        int flashes = 0;
        for (int i = 0; i < steps; i++)
        {
            for (int r = 0; r < this._rowDim; r++)
            {
                for (int c = 0; c < this._colDim; c++)
                {
                    this._energy[r,c]++;
                }
            }

            var flashers = new List<Tuple<int, int>>();
            for (int r = 0; r < this._rowDim; r++)
            {
                for (int c = 0; c < this._colDim; c++)
                {
                    if (this._energy[r,c] > 9)
                    {
                        this.Bloom(flashers, r, c);
                    }
                }
            }

            int allCount = 0;
            for (int r = 0; r < this._rowDim; r++)
            {
                for (int c = 0; c < this._colDim; c++)
                {
                    if (this._energy[r,c] > 9)
                    {
                        this._energy[r,c] = 0;
                        flashes++;
                        allCount++;
                    }
                }
            }
            if (allCount == this._rowDim * this._colDim)
            {
                Console.WriteLine($"Full frontal at: {i+1}");
            }
        }
        return flashes;
    }

    private void Bloom(List<Tuple<int,int>> flashers, int r, int c)
    {
        this._energy[r,c]++;

        if (this._energy[r,c] <= 9 || flashers.Any(f => f.Item1 == r && f.Item2 == c))
        {
            return;
        }

        flashers.Add(new Tuple<int, int>(r, c));
        if (r - 1 >= 0) // top
        {
            this.Bloom(flashers, r-1, c);
        }
        if (r + 1 < this._rowDim) // bottom
        {
            this.Bloom(flashers, r+1, c);
        }
        if (c - 1 >= 0) // left
        {
            this.Bloom(flashers, r, c-1);
        }
        if (c + 1 < this._colDim) // right
        {
            this.Bloom(flashers, r, c+1);
        }

        if (r - 1 >= 0 && c - 1 >= 0) // top left
        {
            this.Bloom(flashers, r-1, c-1);
        }
        if (r - 1 >= 0 && c + 1 < this._colDim) // top right
        {
            this.Bloom(flashers, r-1, c+1);
        }
        if (r + 1 < this._rowDim && c - 1 >= 0) // bottom left
        {
            this.Bloom(flashers, r+1, c-1);
        }
        if (r + 1 < this._rowDim && c + 1 < this._colDim) // bottom right
        {
            this.Bloom(flashers, r+1, c+1);
        }
    }
}