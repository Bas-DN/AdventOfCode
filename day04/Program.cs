namespace day03;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        List<CampSection> campSections = new();
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    campSections.Add(new CampSection(line));
                }
            }
        }
        // First part
        Console.WriteLine($"Full overlap: {campSections.Count(c => c.CheckFullOverlap())}");
        // Second part
        Console.WriteLine($"Any overlap: {campSections.Count(c => c.CheckOverlap())}");
    }
}
public class CampSection
{
    private int[] firstElfSections;
    private int[] secondElfSections;
    public CampSection(string s)
    {
        var elfSections = s.Split(',');
        var first = Utils.SectionArray(elfSections[0].Split('-'));
        var second = Utils.SectionArray(elfSections[1].Split('-'));
        // Create array of ints
        firstElfSections = Enumerable.Range(first[0], first[1] - first[0] + 1).ToArray();
        secondElfSections = Enumerable.Range(second[0], second[1] - second[0] + 1).ToArray();
    }
    public bool CheckFullOverlap()
    {
        if (Utils.CheckFullyContains(firstElfSections, secondElfSections) || Utils.CheckFullyContains(secondElfSections, firstElfSections))
            return true;
        return false;
    }
    public bool CheckOverlap()
    {
        return Utils.CheckContains(firstElfSections, secondElfSections);
    }
}
public static class Utils
{
    public static int[] SectionArray(string[] s)
    {
        var elf = new int[2];
        elf[0] = int.Parse(s[0]);
        elf[1] = int.Parse(s[1]);
        return elf;
    }
    public static bool CheckFullyContains(int[] first, int[] second)
    {
        foreach (var item in second)
        {
            if (!first.Contains(item))
                return false;
        }
        return true;
    }
    public static bool CheckContains(int[] first, int[] second)
    {
        foreach (var item in second)
        {
            if (first.Contains(item))
                return true;
        }
        return false;
    }
}