using AOC;

var measurements = InputParser.ParseListOfInts("../inputs/day01.txt");
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
Console.WriteLine($"Measurement increases: {measurementIncreases}");