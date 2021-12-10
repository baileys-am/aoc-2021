using AOC;

static void PartOne(List<SubCommand> commands)
{
    int horizontalPos = 0;
    int depth = 0;
    foreach (var command in commands)
    {
        switch (command.Move)
        {
            case SubMove.Forward:
                horizontalPos += command.Units;
                break;
            case SubMove.Down:
                depth += command.Units;
                break;
            case SubMove.Up:
                depth -= command.Units;
                break;
            default:
                break;
        }
    }
    Console.WriteLine($"Current horizontal position: {horizontalPos}");
    Console.WriteLine($"Current depth: {depth}");
    Console.WriteLine($"Answer: {horizontalPos * depth}");
    // Answer is 1694130
}

static void PartTwo(List<SubCommand> commands)
{
    int horizontalPos = 0;
    int depth = 0;
    int aim = 0;
    foreach (var command in commands)
    {
        switch (command.Move)
        {
            case SubMove.Forward:
                horizontalPos += command.Units;
                depth += aim * command.Units;
                break;
            case SubMove.Down:
                aim += command.Units;
                break;
            case SubMove.Up:
                aim -= command.Units;
                break;
            default:
                break;
        }
    }
    Console.WriteLine($"Current horizontal position: {horizontalPos}");
    Console.WriteLine($"Current depth: {depth}");
    Console.WriteLine($"Answer: {horizontalPos * depth}");
    // Answer is 1698850445
}

var commands = InputParser.ParseSubCommands("../inputs/day02.txt");
PartOne(commands);
PartTwo(commands);