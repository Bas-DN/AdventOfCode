namespace day00;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        List<string> objects = new();
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                }
            }
        }
        Console.WriteLine($"");
    }
}
public static class Utils
{
}