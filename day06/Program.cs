namespace day06;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        Signal signal;
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    signal = new Signal(line);
                    var marker = signal.FindMarker();
                    var startMarkerIndex = signal.GetCharactersProcessed(marker);
                    // Part one
                    Console.WriteLine($"{marker} - {startMarkerIndex}");


                    var messageMarker = signal.FindMarker(14, startMarkerIndex);
                    var messageMarkerIndex = signal.GetCharactersProcessed(messageMarker, 14);
                    // Part two
                    Console.WriteLine($"{messageMarker} - {messageMarkerIndex}");
                }
            }
        }
        Console.WriteLine($"");
    }
}
public class Signal
{
    private string datastreamBuffer;
    public Signal(string s)
    {
        datastreamBuffer = s;
    }
    public string FindMarker(int distinctChars = 4, int offset = 0)
    {
        var chars = datastreamBuffer.ToArray();
        char[] marker = new char[0];
        for (int i = offset; i < chars.Length; i++)
        {
            var pickedChars = chars
                .Skip(i)
                .Take(distinctChars)
                .GroupBy(c => c);
            if (pickedChars.Count() == distinctChars)
                marker = chars.Skip(i).Take(distinctChars).ToArray();
            // If marker is found, stop searching
            if (marker.Length == distinctChars)
                break;
        }
        return new string(marker);
    }
    public int GetCharactersProcessed(string marker, int distinctChars = 4)
    {
        return datastreamBuffer.IndexOf(marker) + distinctChars; // + 4 because marker is 4 long
    }
}