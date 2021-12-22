using System.Text.RegularExpressions;

static void PartOne(string filepath)
{
    Console.WriteLine(new SnailfishCalc().GetMagnitude("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"));
    // Answer is NO WAY IM DOING THIS ONE
}

static void PartTwo(string filepath)
{
    // Answer is 
}

var filepath = "../inputs/day18_example.txt";
PartOne(filepath);
PartTwo(filepath);


public class SnailfishCalc
{
    public string Add(string expr1, string expr2)
    {
        return null;
    }

    public int GetMagnitude(string expr)
    {
        Regex rx = new Regex(@"[0-9],[0-9]", RegexOptions.Compiled);
        foreach (Match match in rx.Matches(expr))
        {
            Console.WriteLine(match.Value);
        }
        return 0;
    }
}