static void PartOne(string filepath)
{
    var die = new DeterministicDie();
    var game = new DiracDice(filepath, die);
    var result = game.WhatDoYouGetIfYouMultiplyTheScoreOfTheLosingPlayerByTheNumberOfTimesTheDieWasRolledDuringTheGame();
    Console.WriteLine($"Result: {result}");
    // Answer is 
}

static void PartTwo(string filepath)
{
    // Answer is 
}

var filepath = "../inputs/day21.txt";
PartOne(filepath);
PartTwo(filepath);


interface IDie
{
    long NumberOfRolls { get; }
    int Roll();
}

class DeterministicDie : IDie
{
    private int _value = 0;

    public long NumberOfRolls { get; private set; }

    public int Roll()
    {
        this._value = this._value + 1 > 100 ? 1 : this._value + 1;
        this.NumberOfRolls++;
        return this._value;
    }
}

class Player
{
    private readonly int _startPos;

    public int CurrentPos { get; private set; }
    public long Score { get; private set; }
    public bool HasWon => this.Score >= 1000;

    public Player(int startPos)
    {
        this._startPos = startPos;
        this.CurrentPos = this._startPos;
    }

    public void Move(IDie die)
    {
        int moves = 0;
        for (int i = 0; i < 3; i++)
        {
            var roll = die.Roll();
            Console.Write(roll + " ");
            moves += roll;
        }
        this.CurrentPos = (this.CurrentPos + moves) % 10 == 0 ? 10 : (this.CurrentPos + moves) % 10;
        this.Score += this.CurrentPos;
        Console.WriteLine($"and moves to space {this.CurrentPos} for a total score of {this.Score}");
    }
}

class DiracDice
{
    private readonly IDie _die;
    private Player _player1;
    private Player _player2;

    public DiracDice(string filepath, IDie die)
    {
        var lines = File.ReadAllLines(filepath);
        this._player1 = new Player(int.Parse(lines[0].Split(": ").Last()));
        this._player2 = new Player(int.Parse(lines[1].Split(": ").Last()));
        this._die = die;
    }

    public long WhatDoYouGetIfYouMultiplyTheScoreOfTheLosingPlayerByTheNumberOfTimesTheDieWasRolledDuringTheGame()
    {
        while (!this._player1.HasWon && !this._player2.HasWon)
        {
            Console.Write("Player 1 rolls ");
            this._player1.Move(this._die);
            if (this._player1.HasWon)
            {
                break;
            }
            Console.Write("Player 2 rolls ");
            this._player2.Move(this._die);
        }
        if (this._player1.HasWon)
        {
            return this._player2.Score * this._die.NumberOfRolls;
        }
        else
        {
            return this._player1.Score * this._die.NumberOfRolls;
        }
    }
}