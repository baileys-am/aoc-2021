static void PartOne(string filepath)
{
    var data = File.ReadAllLines(filepath);
    var numOfBits = data.First().Length;
    var gammaRate = 0;
    var epsilonRate = 0;
    for (int i = 0; i < numOfBits; i++)
    {
        var pow = numOfBits - i - 1;
        var total = data.Select(line => int.Parse(line.Substring(i, 1))).Sum();
        bool oneIsCommon = total > data.Length / 2;
        if (oneIsCommon)
        {
            gammaRate += (int)Math.Pow(2, pow);
        }
        else 
        {
            epsilonRate += (int)Math.Pow(2, pow);
        }
    }
    Console.WriteLine($"Gamma rate: {gammaRate}");
    Console.WriteLine($"Epsilon rate: {epsilonRate}");
    Console.WriteLine($"Power consumption: {gammaRate * epsilonRate}");
    // Answer is 3429254
}

static void PartTwo(string filepath)
{
    var data = File.ReadAllLines(filepath);
    var numOfBits = data.First().Length;

    var oxyRate = 0;
    var oxyData = new List<string>(data);
    for (int i = 0; i < numOfBits && oxyData.Count > 1; i++)
    {
        var bitColumn = oxyData.Select(line => int.Parse(line.Substring(i, 1))).ToList();
        var total = bitColumn.Sum();
        bool oneIsCommon = total >= oxyData.Count / 2.0;
        int removeCount = 0;
        for (int j = 0; j < bitColumn.Count && oxyData.Count > 1; j++)
        {
            if ((oneIsCommon && bitColumn[j] == 0) || (!oneIsCommon && bitColumn[j] == 1))
            {
                oxyData.RemoveAt(j - removeCount);
                removeCount++;
            }
        }
    }
    var oxyBits = oxyData.First();
    for (int i = 0; i < numOfBits; i++)
    {
        var pow = numOfBits - i - 1;
        var oxyBit = int.Parse(oxyBits.Substring(i, 1));
        oxyRate += oxyBit * (int)Math.Pow(2, pow);
    }

    var co2Rate = 0;
    var co2Data = new List<string>(data);
    for (int i = 0; i < numOfBits && co2Data.Count > 1; i++)
    {
        var bitColumn = co2Data.Select(line => int.Parse(line.Substring(i, 1))).ToList();
        var total = bitColumn.Sum();
        bool zeroIsCommon = total < co2Data.Count / 2.0;
        int removeCount = 0;
        for (int j = 0; j < bitColumn.Count && co2Data.Count > 1; j++)
        {
            if ((zeroIsCommon && bitColumn[j] == 0) || (!zeroIsCommon && bitColumn[j] == 1))
            {
                co2Data.RemoveAt(j - removeCount);
                removeCount++;
            }
        }
    }
    var co2Bits = co2Data.First();
    for (int i = 0; i < numOfBits; i++)
    {
        var pow = numOfBits - i - 1;
        var co2Bit = int.Parse(co2Bits.Substring(i, 1));
        co2Rate += co2Bit * (int)Math.Pow(2, pow);
    }

    Console.WriteLine($"Oxygen generator rating: {oxyRate}");
    Console.WriteLine($"CO2 scrubber rating: {co2Rate}");
    Console.WriteLine($"Life support rating: {oxyRate * co2Rate}");
    // Answer is 
}

var filepath = "../inputs/day03.txt";
PartOne(filepath);
PartTwo(filepath);