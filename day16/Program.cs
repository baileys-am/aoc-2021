static void PartOne(string filepath)
{
    var bits = new BITS(filepath);
    bits.Print();
    var versionSum = bits.ParsePackets().VersionSum;
    Console.WriteLine($"Version sum: {versionSum}");
    // Answer is 1007
}

static void PartTwo(string filepath)
{
    var bits = new BITS(filepath);
    bits.Print();
    var packetList = bits.ParsePackets();
    var expression = packetList.Expression;
    packetList.PrintExpression();
    Console.WriteLine($"Express yo self: {expression}");
    // Answer is 834151779165
}

var filepath = "../inputs/day16.txt";
PartOne(filepath);
PartTwo(filepath);


class Packet
{
    protected readonly int[] _bits;
    protected readonly int _startIndex;
    protected readonly int _endIndex;

    public int Version { get; }
    public int TypeID { get; }

    public List<Packet> Subpackets { get; set; } = new List<Packet>();

    public Packet(int version, int typeID, int[] bits, int startIndex, int endIndex)
    {
        this.Version = version;
        this.TypeID = typeID;
        this._bits = bits;
        this._startIndex = startIndex;
        this._endIndex = endIndex;
    }

    public static Packet ReadNextPacket(int[] bits, int startIndex, out int endIndex)
    {
        var version = (int)Packet.BitsToDec(bits, startIndex, 3);
        var id = (int)Packet.BitsToDec(bits, startIndex + 3, 3);
        Console.WriteLine($"V={version} ID={id}...");
        switch (id)
        {
            case 4: // Literal value packet
                endIndex = startIndex + 6;
                while (endIndex < bits.Length)
                {
                    if (bits[endIndex] == 0)
                    {
                        endIndex += 5;
                        return new Packet(version, id, bits, startIndex, endIndex);
                    }
                    endIndex += 5;
                }
                endIndex = bits.Length - 1;
                throw new Exception("Someone should sanitize all these packets before asking me to spend my time on them!");
            default: // Operator packet
                var subpackets = new List<Packet>();
                var lengthTypeID = Packet.BitsToDec(bits, startIndex + 6, 1);
                switch (lengthTypeID)
                {
                    case 0: // Next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet
                        var totalLength = Packet.BitsToDec(bits, startIndex + 6 + 1, 15);
                        int ogSubpacketStart1 = startIndex + 6 + 1 + 15;
                        int nextSubpacketStart1 = ogSubpacketStart1;
                        do
                        {
                            subpackets.Add(ReadNextPacket(bits, nextSubpacketStart1, out endIndex));
                            nextSubpacketStart1 = endIndex;
                        } while (endIndex - ogSubpacketStart1 < totalLength);
                        break;
                    default: // Next 11 bits are a number that represents the number of sub-packets immediately contained by this packet
                        var numSubPackets = Packet.BitsToDec(bits, startIndex + 6 + 1, 11);
                        int ogSubpacketStart2 = startIndex + 6 + 1 + 11;
                        int nextSubpacketStart2 = ogSubpacketStart2;
                        endIndex = ogSubpacketStart2;
                        for (int j = 0; j < numSubPackets; j++)
                        {
                            subpackets.Add(ReadNextPacket(bits, nextSubpacketStart2, out endIndex));
                            nextSubpacketStart2 = endIndex;
                        }
                        break;
                }
                var packet = new Packet(version, id, bits, startIndex, endIndex);
                packet.Subpackets.AddRange(subpackets);
                return packet;
        }
    }

    public virtual void Print()
    {
        Console.WriteLine($"Version: {this.Version}");
        Console.WriteLine($"ID: {this.TypeID}");
    }

    protected static long BitsToDec(int[] bits, int start, int length)
    {
        double dec = 0;
        Console.Write($"{start}->{start + length - 1}: ");
        for (int i = 0; i < length; i++)
        {
            int index = start + i;
            Console.Write(bits[index]);
            dec += bits[index] * Math.Pow(2, length - i - 1);
        }
        Console.WriteLine();
        return (long)dec;
    }

    public virtual int GetVersionSum()
    {
        int versionSum = this.Version;
        foreach (var subpacket in this.Subpackets)
        {
            versionSum += subpacket.GetVersionSum();
        }
        return versionSum;
    }

    public virtual long GetExpression()
    {
        switch (this.TypeID)
        {
            case 0:
                return this.Subpackets.Select(s => s.GetExpression()).Sum();
            case 1:
                return this.Subpackets.Select(s => s.GetExpression()).Aggregate(1L, (s1, s2) => s1 * s2);
            case 2:
                return this.Subpackets.Select(s => s.GetExpression()).Min();
            case 3:
                return this.Subpackets.Select(s => s.GetExpression()).Max();
            case 4:
                return this.ParseLiteralValue();
            case 5:
                return this.Subpackets[0].GetExpression() > this.Subpackets[1].GetExpression() ? 1 : 0;
            case 6:
                return this.Subpackets[0].GetExpression() < this.Subpackets[1].GetExpression() ? 1 : 0;
            case 7:
                return this.Subpackets[0].GetExpression() == this.Subpackets[1].GetExpression() ? 1 : 0;
            default:
                throw new Exception("DNR");
        }
    }

    public string GetPrettyPrintExpression()
    {
        switch (this.TypeID)
        {
            case 0:
                return "(" + this.Subpackets.Select(s => s.GetPrettyPrintExpression()).Aggregate((s1, s2) => $"{s1} + {s2}") + ")";
            case 1:
                return "(" + this.Subpackets.Select(s => s.GetPrettyPrintExpression()).Aggregate((s1, s2) => $"{s1} * {s2}") + ")";
            case 2:
                return " min([" + this.Subpackets.Select(s => s.GetPrettyPrintExpression()).Aggregate((s1, s2) => $"{s1}, {s2}") + "]) ";
            case 3:
                return " max([" + this.Subpackets.Select(s => s.GetPrettyPrintExpression()).Aggregate((s1, s2) => $"{s1}, {s2}") + "]) ";
            case 4:
                return "" + this.ParseLiteralValue().ToString() + "";
            case 5:
                return " (" + this.Subpackets[0].GetPrettyPrintExpression() + ") > (" + this.Subpackets[1].GetPrettyPrintExpression() + ") ";
            case 6:
                return " (" + this.Subpackets[0].GetPrettyPrintExpression() + ") < (" + this.Subpackets[1].GetPrettyPrintExpression() + ") ";
            case 7:
                return " (" + this.Subpackets[0].GetPrettyPrintExpression() + ") == (" + this.Subpackets[1].GetPrettyPrintExpression() + ") ";
            default:
                throw new Exception("DNR");
        }
    }

    private long ParseLiteralValue()
    {
        int length = this._endIndex - (this._startIndex + 6);
        length -= length / 5;
        var bits = new int[length];
        for (int i = 0; i < length; i++)
        {
            int mappedIndex = this._startIndex + 6 + i + (i / 4 + 1);
            bits[i] = this._bits[mappedIndex];
        }
        var value = Packet.BitsToDec(bits, 0, length);
        return value;
    }
}

class PacketList
{
    private readonly List<Packet> _packets;

    public int VersionSum { get; private set; } = 0;

    public long Expression { get; private set; } = 0;

    public PacketList(List<Packet> packets)
    {
        this._packets = packets;

        foreach (var packet in this._packets)
        {
            this.VersionSum += packet.GetVersionSum();
        }

        foreach (var packet in this._packets)
        {
            this.Expression += packet.GetExpression();
        }
    }

    public void PrintExpression()
    {
        foreach (var packet in this._packets)
        {
            var expr = packet.GetPrettyPrintExpression();
            Console.WriteLine(expr);
        }
    }
}

class BITS
{
    private readonly int[] _bits;

    public BITS(string filepath)
    {
        var packetz = File.ReadAllText(filepath);
        this._bits = new int[packetz.Length*4];
        int i = 0;
        foreach (var hex in packetz)
        {
            switch (hex)
            {
                case '0':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 0;
                    break;
                case '1':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 1;
                    break;
                case '2':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 0;
                    break;
                case '3':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 1;
                    break;
                case '4':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 0;
                    break;
                case '5':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 1;
                    break;
                case '6':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 0;
                    break;
                case '7':
                    this._bits[i] = 0;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 1;
                    break;
                case '8':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 0;
                    break;
                case '9':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 1;
                    break;
                case 'A':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 0;
                    break;
                case 'B':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 0;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 1;
                    break;
                case 'C':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 0;
                    break;
                case 'D':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 0;
                    this._bits[i + 3] = 1;
                    break;
                case 'E':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 0;
                    break;
                case 'F':
                    this._bits[i] = 1;
                    this._bits[i + 1] = 1;
                    this._bits[i + 2] = 1;
                    this._bits[i + 3] = 1;
                    break;
            }
            i += 4;
        }
    }

    public void Print()
    {
        Console.Write($"Binary: ");
        foreach (var bit in this._bits)
        {
            Console.Write(bit);
        }
        Console.WriteLine();
    }

    public PacketList ParsePackets()
    {
        var packets = new List<Packet>();
        int startIndex = 0;
        int endIndex;
        do
        {
            var packet = Packet.ReadNextPacket(this._bits, startIndex, out endIndex);
            startIndex = endIndex + 1;
            packets.Add(packet);
        } while (this._bits.Length - startIndex > 6);
        return new PacketList(packets);
    }
}