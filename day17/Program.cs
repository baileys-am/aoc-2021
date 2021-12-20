static void PartOne(string filepath)
{
    // var trickShot = new TrickShot(20, 30, -10, -5);
    var trickShot = new TrickShot(253, 280, -73, -46);
    // trickShot.FindOvershootVelocities(out int xVmax, out int yVmax);
    int xVmax = 99; int yVmax = 99;
    Console.WriteLine($"Max velocities: ({xVmax}, {yVmax})");
    List<TrickShotResult> results = new List<TrickShotResult>();
    for (int yV = yVmax; yV >= 0; yV--)
    {
        for (int xV = xVmax - 1; xV >= 0; xV--)
        {
            var hit = trickShot.Fire(xV, yV, out int x, out int y, out int maxY);
            if (hit)
            {
                results.Add(new TrickShotResult(x, y, xV, yV, maxY));
            }
        }
    }
    var maxYpossible = results.Select(r => r.MaxY).Max();
    var maxYresult = results.Where(r => r.MaxY == maxYpossible).OrderBy(r => r.xV).First();
    Console.WriteLine($"({maxYresult.x},{maxYresult.y}) -> Velocity: ({maxYresult.xV},{maxYresult.yV}) | Max Y: {maxYpossible}");
    // maxYresult.Print(trickShot);
    // Answer is 2628
}

static void PartTwo(string filepath)
{
    // var trickShot = new TrickShot(20, 30, -10, -5);
    var trickShot = new TrickShot(253, 280, -73, -46);
    // trickShot.FindOvershootVelocities(out int xVmax, out int yVmax);
    int xVmax = 1000; int yVmax = 1000;
    Console.WriteLine($"Max velocities: ({xVmax}, {yVmax})");
    List<TrickShotResult> results = new List<TrickShotResult>();
    for (int yV = yVmax; yV >= -1000; yV--)
    {
        for (int xV = xVmax - 1; xV >= 0; xV--)
        {
            var hit = trickShot.Fire(xV, yV, out int x, out int y, out int maxY);
            if (hit)
            {
                results.Add(new TrickShotResult(x, y, xV, yV, maxY));
            }
        }
    }
    var maxYpossible = results.Select(r => r.MaxY).Max();
    var maxYresult = results.Where(r => r.MaxY == maxYpossible).OrderBy(r => r.xV).First();
    Console.WriteLine($"Possible velocities: {results.Count} | Max Y: {maxYpossible}");
    // maxYresult.Print(trickShot);
    // Answer is 1334
}

var filepath = "../inputs/day17_example.txt";
PartOne(filepath);
PartTwo(filepath);


class TrickShotResult
{
    public int x { get; }
    public int y { get; }
    public int xV { get; }
    public int yV { get; }
    public int MaxY { get; }
    
    public TrickShotResult(int x, int y, int xV, int yV, int maxY)
    {
        this.x = x;
        this.y = y;
        this.xV = xV;
        this.yV = yV;
        this.MaxY = maxY;
    }

    public void Print(TrickShot trickShot)
    {
        trickShot.Fire(this.xV, this.yV, out _, out _, out _, true);
    }
}

class TrickShot
{
    int[] _xT = new int[2];
    int[] _yT = new int[2];

    public TrickShot(int minX, int maxX, int minY, int maxY)
    {
        this._xT[0] = minX;
        this._xT[1] = maxX;
        this._yT[0] = minY;
        this._yT[1] = maxY;
    }

    public bool Fire(int xV, int yV, out int x, out int y, out int maxY, bool print = false)
    {
        x = 0;
        y = 0;
        maxY = y;
        bool hit = false;
        var pos = new List<Tuple<int, int>>();
        do
        {
            if (print)
            {
                pos.Add(new Tuple<int, int>(x, y));
            }

            if (x >= this._xT[0] && x <= this._xT[1] && y >= this._yT[0] && y <= this._yT[1])
            {
                hit = true;
                break;
            }

            // Firing alg
            x += xV;
            y += yV;
            if (xV > 0) xV -= 1;
            if (xV < 0) xV += 1;
            yV -= 1;

            // Update state
            maxY = Math.Max(maxY, y);
        } while (x <= this._xT[1] && y >= this._yT[0]);

        if (print)
        {
            var xmax = Math.Max(pos.Select(t => t.Item1).Max(), this._xT.Max());
            var ymax = Math.Max(pos.Select(t => t.Item2).Max(), this._yT.Max());
            var ymin = Math.Min(pos.Select(t => t.Item2).Min(), this._yT.Min());

            for (int yyy = ymax; yyy >= ymin; yyy--)
            {
                for (int xxx = 0; xxx <= xmax; xxx++)
                {
                    if (xxx == 0 && yyy == 0)
                    {
                        Console.Write("S");
                    }
                    else if (pos.Any(t => t.Item1 == xxx && t.Item2 == yyy))
                    {
                        Console.Write("#");
                    }
                    else if (xxx >= this._xT[0] && xxx <= this._xT[1] && yyy >= this._yT[0] && yyy <= this._yT[1])
                    {
                        Console.Write("T");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        return hit;
    }

    public void FindOvershootVelocities(out int xV, out int yV)
    {
        int x = 0;
        int y = 0;
        int prevX = 0;
        xV = 0;
        yV = 0;
        bool overshoot = false;
        bool dropshot = false;
        while (!(overshoot && dropshot))
        {
            bool hit = this.Fire(xV, yV, out x, out y, out _);
            if (!overshoot)
            {
                if (x > this._xT[1] && y > this._yT[1])
                {
                    overshoot = true;
                    yV++;
                }
                else if (x <= prevX)
                {
                    xV++;
                }
                else
                {
                    yV++;
                }
            }
            else
            {
                if (x <= prevX)
                {
                    dropshot = true;
                    yV++;
                }
                else
                {
                    yV++;
                }
            }

            prevX = x;
        }
    }
}