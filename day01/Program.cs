using AOC;

static void PartOne(List<int> measurements)
{
    int measurementIncreases = 0;
    int lastMeasurement = measurements.First();
    foreach (var measurement in measurements.Skip(1))
    {
        if (measurement > lastMeasurement)
        {
            measurementIncreases++;
        }
        lastMeasurement = measurement;
    }
    Console.WriteLine($"Part one measurement increases: {measurementIncreases}");
    // Answer is 1602
}

static void PartTwo(List<int> measurements)
{
    int measurementIncreases = 0;
    int windowLength = 3;
    int lastMeasurementAccum = measurements[0] + measurements[1] + measurements[2];
    for (int windowStartIndex = 1; windowStartIndex < measurements.Count - windowLength + 1; windowStartIndex++)
    {
        int measurementAccum = 0;
        for (int i = windowStartIndex; i < windowStartIndex + windowLength; i++)
        {
            measurementAccum += measurements[i];
        }
        if (measurementAccum > lastMeasurementAccum)
        {
            measurementIncreases++;
        }
        lastMeasurementAccum = measurementAccum;
    }
    Console.WriteLine($"Part two measurement increases: {measurementIncreases}");
    // Answer is 1633
}

var measurements = InputParser.ParseListOfInts("../inputs/day01.txt");
PartOne(measurements);
PartTwo(measurements);