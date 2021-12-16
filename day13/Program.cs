static void PartOne(string filepath)
{
    var manual = new Manual(filepath);
    int dots = manual.StepTransaction();
    Console.WriteLine($"Dots: {dots}");
    // Answer is 655
}

static void PartTwo(string filepath)
{
    var manual = new Manual(filepath);
    int dots = manual.StepAllTransactions();
    manual.Print();
    // Answer is JPZCUAUR
}

var filepath = "../inputs/day13.txt";
PartOne(filepath);
PartTwo(filepath);


class Coord
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coord(string line)
    {
        var split = line.Split(",");
        this.X = int.Parse(split[0]);
        this.Y = int.Parse(split[1]);
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

class Transaction
{
    public bool IsXFold { get; }
    public bool IsYFold { get; }
    public int FoldAt { get; }

    public Transaction(string line)
    {
        var split = line.Split("=");
        if (split[0].Last() == 'x')
        {
            this.IsXFold = true;
            this.FoldAt = int.Parse(split[1]);
        }
        else
        {
            this.IsYFold = true;
            this.FoldAt = int.Parse(split[1]);
        }
    }
}

class Manual
{
    private readonly List<Coord> _coords = new List<Coord>();
    private readonly List<Transaction> _transactions = new List<Transaction>();
    private int _xDim;
    private int _yDim;
    private int _currentStep;

    public Manual(string filepath)
    {
        var lines = File.ReadAllLines(filepath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            else
            {
                var coord = new Coord(line);
                this._coords.Add(coord);
                this._xDim = Math.Max(this._xDim, coord.X + 1);
                this._yDim = Math.Max(this._yDim, coord.Y + 1);
            }
        }

        foreach (var line in lines.Skip(this._coords.Count + 1))
        {
            var transaction = new Transaction(line);
            this._transactions.Add(transaction);
            if (transaction.IsXFold)
            {
                this._xDim = Math.Max(this._xDim, transaction.FoldAt + 1);
            }
            else if (transaction.IsYFold)
            {
                this._yDim = Math.Max(this._yDim, transaction.FoldAt + 1);
            }
        }
    }

    public void Print()
    {
        for (int y = 0; y < this._yDim; y++)
        {
            for (int x = 0; x < this._xDim; x++)
            {
                if (this._coords.Any(c => c.IsSame(x, y)))
                {
                    Console.Write($"#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }

    public int StepTransaction()
    {
        var transaction = this._transactions[this._currentStep];
        foreach (var coord in this._coords.ToList())
        {
            if (transaction.IsYFold && coord.Y >= transaction.FoldAt && transaction.FoldAt - (coord.Y - transaction.FoldAt) >= 0)
            {
                var newY = transaction.FoldAt - (coord.Y - transaction.FoldAt);
                if (this._coords.Any(c => c.IsSame(coord.X, newY)))
                {
                    this._coords.Remove(coord);
                }
                else
                {
                    coord.Y = newY;
                }
            }
            else if (transaction.IsYFold && coord.Y >= transaction.FoldAt && transaction.FoldAt - (coord.Y - transaction.FoldAt) < 0)
            {
                this._coords.Remove(coord);
            }

            if (transaction.IsXFold && coord.X >= transaction.FoldAt && transaction.FoldAt - (coord.X - transaction.FoldAt) >= 0)
            {
                var newX = transaction.FoldAt - (coord.X - transaction.FoldAt);
                if (this._coords.Any(c => c.IsSame(newX, coord.Y)))
                {
                    this._coords.Remove(coord);
                }
                else
                {
                    coord.X = newX;
                }
            }
            else if (transaction.IsXFold && coord.X >= transaction.FoldAt && transaction.FoldAt - (coord.X - transaction.FoldAt) < 0)
            {
                this._coords.Remove(coord);
            }
        }

        this._xDim = 0;
        this._yDim = 0;
        foreach (var coord in this._coords)
        {
            this._xDim = Math.Max(this._xDim, coord.X + 1);
            this._yDim = Math.Max(this._yDim, coord.Y + 1);
        }

        this._currentStep++;
        return this._coords.Count;
    }

    public int StepAllTransactions()
    {
        int dots = 0;
        for (int i = 0; i < this._transactions.Count; i++)
        {
            dots = this.StepTransaction();
        }
        return dots;
    }
}