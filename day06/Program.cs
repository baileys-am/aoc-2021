const long BeginnersDaysToMakeFishie = 8;
const long IntermediatesDaysToMakeFishie = 7;

static void PartOne(string filepath)
{
    long babyMakingDays = 80;
    var babyMakingTimes = File.ReadAllText(filepath).Split(",").Select(f => int.Parse(f)).ToList();
    long fishiesInTheSea = 0;
    babyMakingTimes.ForEach(fishie =>
    {
        fishiesInTheSea += BabyFishieMaker(0, fishie, babyMakingDays - 1);
    });
    Console.WriteLine($"Fishies in the sea: {fishiesInTheSea}");
    // Answer is 396210
}

static void PartTwo(string filepath)
{
    long babyMakingDays = 256;
    var babyMakingTimes = File.ReadAllText(filepath).Split(",").Select(f => int.Parse(f)).ToList();
    Dictionary<long, long> fishieCount = new Dictionary<long, long>();
    Dictionary<long, long> fishieBabies = new Dictionary<long, long>();
    babyMakingTimes.ForEach(fishie =>
    {
        if (fishieCount.ContainsKey(fishie))
        {
            fishieCount[fishie] += 1;
        }
        else
        {
            Console.WriteLine(fishie);
            fishieCount[fishie] = 1;
            fishieBabies[fishie] = BabyFishieMaker(0, fishie, babyMakingDays - 1);
        }
    });
    long fishiesInTheSea = 0;
    foreach (var kvp in fishieBabies)
    {
        fishiesInTheSea += kvp.Value * fishieCount[kvp.Key];
    }
    Console.WriteLine($"Fishies in the sea: {fishiesInTheSea}");
    // Answer is 1770823541496
}

static long BabyFishieMaker(long myGen, long babyMakingCountdown, long babyMakingDays)
{
    long fishiesInTheSea = 1;
    long myBabyFishieCount = (babyMakingDays - babyMakingCountdown) / (IntermediatesDaysToMakeFishie);
    myBabyFishieCount += (babyMakingDays - IntermediatesDaysToMakeFishie * myBabyFishieCount) >= babyMakingCountdown ? 1 : 0;
    if (myBabyFishieCount > 0)
    {
        for (long i = 0; i < myBabyFishieCount; i++)
        {
            long babiesBabyMakingCountDown = BeginnersDaysToMakeFishie;
            long babiesBabyMakingDays = babyMakingDays - babyMakingCountdown - i * IntermediatesDaysToMakeFishie - 1;
            fishiesInTheSea += BabyFishieMaker(myGen + 1, babiesBabyMakingCountDown, babiesBabyMakingDays);
        }
    }
    return fishiesInTheSea;
}

var filepath = "../inputs/day06.txt";
PartOne(filepath);
PartTwo(filepath);