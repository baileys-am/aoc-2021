static void PartOne(string filepath)
{
    var vent = new Vent(File.ReadAllLines(filepath).ToList());
    int count = vent.CalculateDangerousAreas();
    Console.WriteLine($"Dangerous area count: {count}");
    // No longer works since diagnonals are supported
    // Answer is 8350
}

static void PartTwo(string filepath)
{
    var vent = new Vent(File.ReadAllLines(filepath).ToList());
    int count = vent.CalculateDangerousAreas();
    Console.WriteLine($"Dangerous area count: {count}");
    // Easy and quick to just compare all coords for each line :facepalm:
    // Answer is 19374
}

var filepath = "../inputs/day05.txt";
PartOne(filepath);
PartTwo(filepath);

public enum LineDirection
{
    Horz,
    Vert,
    Diag
}

public class Coord
{
    public int X { get; }
    public int Y { get; }

    public Coord(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public bool Equals(Coord other)
    {
        if (Object.ReferenceEquals(other, null)) return false;
        if (Object.ReferenceEquals(this, other)) return true;
        return this.X.Equals(other.X) && this.Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Coord other)
        {
            return this.Equals(other);
        }
        else
        {
            return base.Equals(obj);
        }
    }

    public override int GetHashCode()
    {
        int hashX = this.X.GetHashCode();
        int hashY = this.Y.GetHashCode();
        return hashX ^ hashY;
    }
}

public class Line
{
    public LineDirection Direction { get; set; }
    public Coord StartCoord { get; set; }
    public Coord EndCoord { get; set; }

    public Line(string line)
    {
        var coords = line.Split(" -> ");
        var startCoord = coords[0].Split(",");
        var endCoord = coords[1].Split(",");
        this.StartCoord = new Coord(int.Parse(startCoord[0]), int.Parse(startCoord[1]));
        this.EndCoord = new Coord(int.Parse(endCoord[0]), int.Parse(endCoord[1]));
        if (this.StartCoord.X == this.EndCoord.X)
        {
            this.Direction = LineDirection.Vert;
        }
        else if (this.StartCoord.Y == this.EndCoord.Y)
        {
            this.Direction = LineDirection.Horz;
        }
        else
        {
            this.Direction = LineDirection.Diag;
        }
    }

    public List<Coord> GetCoords()
    {
        var coords = new List<Coord>();
        switch (this.Direction)
        {
            case LineDirection.Horz:
                int xDelta = this.EndCoord.X - this.StartCoord.X;
                int xSign = xDelta / Math.Abs(xDelta);
                for (int i = 0; i <= Math.Abs(xDelta); i++)
                {
                    int x = this.StartCoord.X + xSign * i;
                    coords.Add(new Coord(x, this.StartCoord.Y));
                }
                break;
            case LineDirection.Vert:
                int yDelta = this.EndCoord.Y - this.StartCoord.Y;
                int ySign = yDelta / Math.Abs(yDelta);
                for (int i = 0; i <= Math.Abs(yDelta); i++)
                {
                    int y = this.StartCoord.Y + ySign * i;
                    coords.Add(new Coord(this.StartCoord.X, y));
                }
                break;
            case LineDirection.Diag:
                int deltaX = this.EndCoord.X - this.StartCoord.X;
                int deltaY = this.EndCoord.Y - this.StartCoord.Y;
                int signX = deltaX / Math.Abs(deltaX);
                int signY = deltaY / Math.Abs(deltaY);
                if (Math.Abs(deltaX) != Math.Abs(deltaY))
                {
                    throw new Exception("Not a 45 degree diagonal line");
                }
                for (int i = 0; i <= Math.Abs(deltaX); i++)
                {
                    int x = this.StartCoord.X + signX * i;
                    int y = this.StartCoord.Y + signY * i;
                    coords.Add(new Coord(x, y));
                }
                break;
        }
        return coords;
    }

    public List<Coord> GetIntersects(Line other)
    {
        var thisXindices = new List<int>() { this.StartCoord.X, this.EndCoord.X };
        thisXindices.Sort();
        var thisYindices = new List<int>() { this.StartCoord.Y, this.EndCoord.Y };
        thisYindices.Sort();
        var otherXindices = new List<int>() { other.StartCoord.X, other.EndCoord.X };
        otherXindices.Sort();
        var otherYindices = new List<int>() { other.StartCoord.Y, other.EndCoord.Y };
        otherYindices.Sort();
        var thisCoords = this.GetCoords();
        var otherCoords = other.GetCoords();
        var intCoords = new List<Coord>();

        if (this.Direction == other.Direction)
        {
            switch (this.Direction)
            {
                case LineDirection.Horz: // Only X changes
                    if (this.StartCoord.Y == other.StartCoord.Y)
                    {
                        if ( (thisXindices[0] >= otherXindices[0] && thisXindices[0] <= otherXindices[1]) ||
                             (otherXindices[0] >= thisXindices[0] && otherXindices[0] <= thisXindices[1]) )
                        {
                            var indices = new List<int>() {
                                this.StartCoord.X, this.EndCoord.X, other.StartCoord.X, other.EndCoord.X
                            };
                            indices.Sort();
                            for (int i = indices[1]; i <= indices[2]; i++)
                            {
                                intCoords.Add(new Coord(i, this.StartCoord.Y));
                            }
                        }
                    }
                    break;
                case LineDirection.Vert: // Only Y changes
                    if (this.StartCoord.X == other.StartCoord.X)
                    {
                        if ( (thisYindices[0] >= otherYindices[0] && thisYindices[0] <= otherYindices[1]) ||
                             (otherYindices[0] >= thisYindices[0] && otherYindices[0] <= thisYindices[1]) )
                        {
                            var indices = new List<int>() {
                                this.StartCoord.Y, this.EndCoord.Y, other.StartCoord.Y, other.EndCoord.Y
                            };
                            indices.Sort();
                            for (int i = indices[1]; i <= indices[2]; i++)
                            {
                                intCoords.Add(new Coord(this.StartCoord.X, i));
                            }
                        }
                    }
                    break;
                case LineDirection.Diag:
                    foreach (var coord in thisCoords)
                    {
                        if (otherCoords.Any(c => c.Equals(coord)))
                        {
                            intCoords.Add(coord);
                        }
                    }
                    break;
            }
        }
        else
        {
            switch (this.Direction)
            {
                case LineDirection.Horz: // Only intersects if Y crosses other
                    switch (other.Direction)
                    {
                        case LineDirection.Vert:
                            if ( (otherXindices[0] >= thisXindices[0] && otherXindices[0] <= thisXindices[1]) &&
                                 (thisYindices[0] >= otherYindices[0] && thisYindices[0] <= otherYindices[1]) )
                            {
                                int x = otherXindices[0];
                                int y = thisYindices[0];
                                intCoords.Add(new Coord(x, y));
                            }
                            break;
                        case LineDirection.Diag:
                            foreach (var coord in thisCoords)
                            {
                                if (otherCoords.Any(c => c.Equals(coord)))
                                {
                                    intCoords.Add(coord);
                                }
                            }
                            break;
                    }
                    break;
                case LineDirection.Vert: // Only intersects if X crosses other
                    switch (other.Direction)
                    {
                        case LineDirection.Horz:
                            if ( (thisXindices[0] >= otherXindices[0] && thisXindices[0] <= otherXindices[1]) &&
                                 (otherYindices[0] >= thisYindices[0] && otherYindices[0] <= thisYindices[1]) )
                            {
                                int x = thisXindices[0];
                                int y = otherYindices[0];
                                intCoords.Add(new Coord(x, y));
                            }
                            break;
                        case LineDirection.Diag:
                            foreach (var coord in thisCoords)
                            {
                                if (otherCoords.Any(c => c.Equals(coord)))
                                {
                                    intCoords.Add(coord);
                                }
                            }
                            break;
                    }
                    break;
                case LineDirection.Diag:
                    foreach (var coord in thisCoords)
                    {
                        if (otherCoords.Any(c => c.Equals(coord)))
                        {
                            intCoords.Add(coord);
                        }
                    }
                    break;
            }
        }
        return intCoords;
    }

    public override string ToString()
    {
        return $"{this.StartCoord.X},{this.StartCoord.Y} -> {this.EndCoord.X},{this.EndCoord.Y}";
    }
}

public class Vent
{
    public List<Line> Lines { get; } = new List<Line>();

    public Vent(List<string> lines)
    {
        foreach (var line in lines)
        {
            this.Lines.Add(new Line(line));
        }
    }

    public int CalculateDangerousAreas()
    {
        // var intCoords = new List<Coord>();
        // for (int i = 0; i < this.Lines.Count; i++)
        // {
        //     var line = this.Lines[i];
        //     for (int j = i + 1; j < this.Lines.Count; j++)
        //     {
        //         var otherLine = this.Lines[j];
        //         var intersects = line.GetIntersects(otherLine);
        //         intCoords.AddRange(intersects);
        //         Console.WriteLine($"{line} | {otherLine} | {intersects.Count}");
        //     }
        // }
        int maxX = 0;
        int maxY = 0;
        foreach (var line in this.Lines)
        {
            maxX = Math.Max(maxX, Math.Max(line.StartCoord.X + 1, line.EndCoord.X + 1));
            maxY = Math.Max(maxY, Math.Max(line.StartCoord.Y + 1, line.EndCoord.Y + 1));
        }
        int[,] grid = new int[maxX,maxY];
        foreach (var coord in this.Lines.SelectMany(l => l.GetCoords()))
        {
            grid[coord.X, coord.Y]++;
        }

        int count = 0;
        for (int i = 0; i < maxY; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                count += grid[j, i] > 1 ? 1 : 0;
            }
        }

        return count;
    }
}