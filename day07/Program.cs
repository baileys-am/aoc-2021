static void PartOne(string filepath)
{
    var crabPos = File.ReadAllText(filepath).Split(",").Select(l => int.Parse(l)).ToList();
    var minPos = crabPos.Min();
    var maxPos = crabPos.Max();
    var posCount = maxPos - minPos + 1;
    var weightedPos = new int[posCount];
    foreach (var pos in crabPos)
    {
        for (int i = 0; i < posCount; i++)
        {
            int weight = Math.Abs(pos - (minPos + i));
            weightedPos[i] += weight;
        }
    }
    var bestPosIndex = 0;
    for (int i = 0; i < posCount; i++)
    {
        if (weightedPos[i] < weightedPos[bestPosIndex])
        {
            bestPosIndex = i;
        }
    }
    int bestPos = bestPosIndex + minPos;
    int cost = weightedPos[bestPosIndex];
    Console.WriteLine($"Best pos: {bestPos} | Cost: {cost}");
    // Answer is 344138
}

static void PartTwo(string filepath)
{
    var crabPos = File.ReadAllText(filepath).Split(",").Select(l => int.Parse(l)).ToList();
    var minPos = crabPos.Min();
    var maxPos = crabPos.Max();
    var posCount = maxPos - minPos + 1;
    var weightedPos = new int[posCount];
    foreach (var pos in crabPos)
    {
        for (int i = 0; i < posCount; i++)
        {
            int delta = Math.Abs(pos - (minPos + i));
            int weight = Enumerable.Range(1, delta).Sum();
            weightedPos[i] += weight;
        }
    }
    var bestPosIndex = 0;
    for (int i = 0; i < posCount; i++)
    {
        if (weightedPos[i] < weightedPos[bestPosIndex])
        {
            bestPosIndex = i;
        }
    }
    int bestPos = bestPosIndex + minPos;
    int cost = weightedPos[bestPosIndex];
    Console.WriteLine($"Best pos: {bestPos} | Cost: {cost}");
    // Answer is 94862124
}

var filepath = "../inputs/day07.txt";
PartOne(filepath);
PartTwo(filepath);