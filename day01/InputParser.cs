using System.IO;

namespace AOC
{
    public class InputParser
    {
        public static List<int> ParseListOfInts(string filepath)
        {
            return File.ReadAllLines(filepath).Select(line => int.Parse(line)).ToList();
        }
    }
}