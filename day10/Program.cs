static void PartOne(string filepath)
{
    var lines = File.ReadAllLines(filepath);
    int score = 0;
    foreach (var line in lines)
    {
        var reducedLine = line.ToList();
        var indicesToRemove = new List<int>();
        bool foundError = false;
        for (int i = 0; i < line.Length && !foundError; i++)
        {
            var chunk = line[i];
            if (IsClosingChunk(chunk))
            {
                bool foundOpeningChunk = false;
                for (int j = i - 1; j >= 0 && !foundError && !foundOpeningChunk; j--)
                {
                    if (!indicesToRemove.Contains(i) &&
                        !indicesToRemove.Contains(j) &&
                        (i - j + 1) % 2 == 0)
                    {
                        if (line[j] == GetOpeningChunk(chunk))
                        {
                            reducedLine[i] = ' ';
                            reducedLine[j] = ' ';
                            indicesToRemove.Add(i);
                            indicesToRemove.Add(j);
                            foundOpeningChunk = true;
                        }
                        else if (IsOpeningChunk(line[j]))
                        {
                            score += GetInvalidChunkScore(line[i]);
                            foundError = true;
                        }
                    }
                }
            }
        }
        Console.WriteLine(new string(reducedLine.Where(c => c != ' ').ToArray()));
    }
    Console.WriteLine($"Score: {score}");
    // Answer is 392367
}

static void PartTwo(string filepath)
{
    var lines = File.ReadAllLines(filepath);
    var lineScores = new List<long>();
    var linesToRemove = new List<string>();
    foreach (var line in lines)
    {
        var reducedLine = line.ToList();
        var indicesToRemove = new List<int>();
        bool foundError = false;
        for (int i = 0; i < line.Length && !foundError; i++)
        {
            var chunk = line[i];
            if (IsClosingChunk(chunk))
            {
                bool foundOpeningChunk = false;
                for (int j = i - 1; j >= 0 && !foundError && !foundOpeningChunk; j--)
                {
                    if (!indicesToRemove.Contains(i) &&
                        !indicesToRemove.Contains(j) &&
                        (i - j + 1) % 2 == 0)
                    {
                        if (line[j] == GetOpeningChunk(chunk))
                        {
                            reducedLine[i] = ' ';
                            reducedLine[j] = ' ';
                            indicesToRemove.Add(i);
                            indicesToRemove.Add(j);
                            foundOpeningChunk = true;
                        }
                        else if (IsOpeningChunk(line[j]))
                        {
                            foundError = true;
                        }
                    }
                }
            }
        }
        
        if (!foundError)
        {
            var incompleteLine = new string(reducedLine.Where(c => c != ' ').ToArray());
            long lineScore = 0;

            for (int i = incompleteLine.Length - 1; i >= 0; i--)
            {
                lineScore = 5 * lineScore + GetIncompleteChunkScore(incompleteLine[i]);
            }
            
            lineScores.Add(lineScore);
            Console.WriteLine($"{incompleteLine} -> {lineScore}");
        }
    }

    lineScores = lineScores.OrderBy(s => s).ToList();
    Console.WriteLine($"Score: {lineScores[lineScores.Count/2]}");
    // Answer is 2192104158
}

var filepath = "../inputs/day10.txt";
PartOne(filepath);
PartTwo(filepath);


static int GetInvalidChunkScore(char chunk)
{
    switch (chunk)
    {
        case ')':
            return 3;
        case ']':
            return 57;
        case '}':
            return 1197;
        case '>':
            return 25137;
        default:
            return 0;
    }
}

static long GetIncompleteChunkScore(char chunk)
{
    switch (chunk)
    {
        case '(':
            return 1;
        case '[':
            return 2;
        case '{':
            return 3;
        case '<':
            return 4;
        default:
            return 0;
    }
}

static char GetClosingChunk(char openChunk)
{
    switch (openChunk)
    {
        case '(':
            return ')';
        case '[':
            return ']';
        case '{':
            return '}';
        case '<':
            return '>';
        default:
            throw new Exception($"Invalid open chunk");
    }
}

static char GetOpeningChunk(char closingChunk)
{
    switch (closingChunk)
    {
        case ')':
            return '(';
        case ']':
            return '[';
        case '}':
            return '{';
        case '>':
            return '<';
        default:
            throw new Exception($"Invalid close chunk");
    }
}

static bool IsOpeningChunk(char chunk)
{
    switch (chunk)
    {
        case '(':
            return true;
        case '[':
            return true;
        case '{':
            return true;
        case '<':
            return true;
        default:
            return false;
    }
}

static bool IsClosingChunk(char chunk)
{
    switch (chunk)
    {
        case ')':
            return true;
        case ']':
            return true;
        case '}':
            return true;
        case '>':
            return true;
        default:
            return false;
    }
}

static bool HasInvalidChunk(IEnumerable<char> line, out char invalidChunk)
{
    var openChunk = line.First();
    Console.WriteLine(openChunk);
    invalidChunk = ' ';
    foreach (var chunk in line.Skip(1))
    {
        if (IsOpeningChunk(openChunk))
        {
            if (IsOpeningChunk(chunk))
            {
                return HasInvalidChunk(line.Skip(2), out invalidChunk);
            }
            var expectedCloseChunk = GetClosingChunk(openChunk);
            if (chunk == expectedCloseChunk)
            {
                return false;
            }
            else
            {
                invalidChunk = chunk;
                return true;
            }
        }
    }
    return false;
}