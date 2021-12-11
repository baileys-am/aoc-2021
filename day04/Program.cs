static void PartOne(string filepath)
{
    var bingo = new BingoSubsystem(filepath);
    while (bingo.DrawNumber(out int drawNumber))
    {
    }
    if (bingo.Boards.Any(b => b.IsWinner))
    {
        foreach (var board in bingo.Boards.Where(board => board.IsWinner))
        {
            Console.WriteLine($"Board score: {board.Score}");
            // Answer is 51034
        }
    }
    else
    {
        Console.WriteLine("No winners");
    }
}

static void PartTwo(string filepath)
{
    var bingo = new BingoSubsystem(filepath);
    while (true)
    {
        while (bingo.DrawNumber(out int drawNumber))
        {
        }
        if (bingo.Boards.Count > 1)
        {
            foreach (var board in bingo.Boards.Where(board => board.IsWinner).ToList())
            {
                bingo.RemoveBoardAndReset(board);
            }
        }
        else if (bingo.Boards.Any(b => b.IsWinner))
        {
            break;
        }
    }
    var lastWinningBoard = bingo.Boards.First();
    Console.WriteLine($"Board score: {lastWinningBoard.Score}");
    // Answer is 5434
}

var filepath = "../inputs/day04.txt";
PartOne(filepath);
PartTwo(filepath);

class BingoBoardCell
{
    public int Number { get; set; }
    public bool IsSet { get; set; }

    public BingoBoardCell(int number, bool isSet)
    {
        this.Number = number;
        this.IsSet = isSet;
    }

    public override string ToString()
    {
        return $"{this.Number}|{this.IsSet}";
    }
}

class BingoBoard
{
    private readonly int _width;
    private readonly int _height;

    private List<BingoBoardCell> _board = new List<BingoBoardCell>();

    public int BoardNumber { get; }
    public bool IsWinner { get; private set; }
    public int Score { get; private set; }

    public BingoBoard(List<string> boardLines, int number)
    {
        this._width = boardLines.First().Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries).Count();
        this._height = boardLines.Count;
        this._board = boardLines.SelectMany(line => line.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(n => new BingoBoardCell(int.Parse(n), false)))
                                       .ToList();
        this.BoardNumber = number;
    }

    public bool PlayNumber(int number)
    {
        foreach (var cell in this._board)
        {
            if (cell.Number == number)
            {
                cell.IsSet = true;
            }
        }

        if (!this.IsWinner)
        {
            for (int i = 0; i < this._height; i++)
            {
                bool isRowComplete = true;
                foreach (var index in Enumerable.Range(i * this._width, this._width))
                {
                    isRowComplete &= this._board[index].IsSet;
                    if (!isRowComplete)
                    {
                        break;
                    }
                }
                if (isRowComplete)
                {
                    this.IsWinner = true;
                    this.Score = this._board.Where(c => !c.IsSet).Select(c => c.Number).Sum() * number;
                    break;
                }
            }
        }

        if (!this.IsWinner)
        {
            for (int i = 0; i < this._width; i++)
            {
                bool isColComplete = true;
                for (int j = 0; j < this._height; j++)
                {
                    var index = j * this._width + i;
                    isColComplete &= this._board[index].IsSet;
                    if (!isColComplete)
                    {
                        break;
                    }
                }
                if (isColComplete)
                {
                    this.IsWinner = true;
                    this.Score = this._board.Where(c => !c.IsSet).Select(c => c.Number).Sum() * number;
                    break;
                }
            }
        }

        return this.IsWinner;
    }

    public void PrintBoard()
    {
        Console.WriteLine();
        for (int i = 0; i < this._height; i++)
        {
            for (int j = 0; j < this._width; j++)
            {
                Console.Write($"{this._board[i * this._width + j]}  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

class BingoSubsystem
{
    private readonly List<int> _drawNumbers;

    private bool _isFinished;
    private int _drawCount = 0;

    public List<BingoBoard> Boards { get; } = new List<BingoBoard>();

    public BingoSubsystem(string filepath)
    {
        var lines = File.ReadAllLines(filepath);

        this._drawNumbers = lines.First().Split(",").Select(n => int.Parse(n)).ToList();

        var boardLines = new List<string>();
        var boardNumber = 1;
        foreach (var line in lines.Skip(2))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                boardLines.Add(line);
            }
            else
            {
                this.Boards.Add(new BingoBoard(boardLines, boardNumber));
                boardLines = new List<string>();
                boardNumber++;
            }
        }
        this.Boards.Add(new BingoBoard(boardLines, boardNumber));
    }

    public bool DrawNumber(out int drawNumber)
    {
        drawNumber = this._drawNumbers[this._drawCount];
        if (!this._isFinished)
        {
            var hasWinner = false;

            foreach (var board in this.Boards)
            {
                hasWinner |= board.PlayNumber(drawNumber);
            }

            this._drawCount++;
            this._isFinished = this._drawCount >= this._drawNumbers.Count || hasWinner;
            return !this._isFinished;
        }
        else
        {
            return false;
        }
    }

    public void RemoveBoardAndReset(BingoBoard board)
    {
        this.Boards.Remove(board);
        this._isFinished = false;
        this._drawCount = 0;
    }
}