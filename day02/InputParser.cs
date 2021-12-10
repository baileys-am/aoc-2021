using System.IO;

namespace AOC
{
    public enum SubMove
    {
        Forward,
        Down,
        Up
    }

    public class SubCommand
    {
        public SubMove Move { get; }
        public int Units { get; }

        public SubCommand(SubMove move, int units)
        {
            this.Move = move;
            this.Units = units;
        }
    }

    public class InputParser
    {
        public static List<SubCommand> ParseSubCommands(string filepath)
        {
            return File.ReadAllLines(filepath).Select(line => {
                var parts = line.Split(" ");
                switch (parts[0])
                {
                    case "forward":
                        return new SubCommand(SubMove.Forward, int.Parse(parts[1]));
                    case "down":
                        return new SubCommand(SubMove.Down, int.Parse(parts[1]));
                    case "up":
                        return new SubCommand(SubMove.Up, int.Parse(parts[1]));
                    default:
                        throw new System.Exception("Unknown sub move command");
                }
            }).ToList();
        }
    }
}