namespace day03;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        List<Rucksack> rucksacks = new();
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    rucksacks.Add(new Rucksack(line));
                }
            }
        }
        List<char> dupes = new();
        foreach (var sack in rucksacks)
        {
            var dupe = sack.FindDuplicate();
            dupes.Add(dupe);
            // Console.WriteLine($"Dupe: {dupe} - {sack}");
        }
        var prio = new Priorities();
        var prios = new List<int>();
        foreach (var dupe in dupes)
        {
            prios.Add(prio.GetPrio(dupe));
        }
        // Answer of the first half
        Console.WriteLine($"Total Prio: {prios.Sum()}");

        List<IEnumerable<Rucksack>> groups = Utils.GetGroups(rucksacks);
        List<char> badges = new();
        foreach (var group in groups)
        {
            badges.Add(Utils.GetBadge(group));
        }
        List<int> prioBadges = new();
        foreach (var item in badges)
        {
            Console.WriteLine($"{item} - {prio.GetPrio(item)}");
            prioBadges.Add(prio.GetPrio(item));
        }

        Console.WriteLine($"Sum of prio badges: {prioBadges.Sum()}");
    }
}

public static class Utils
{
    public static char GetBadge(IEnumerable<Rucksack> group)
    {
        var sack1 = group.ElementAt(0);
        var sack2 = group.ElementAt(1);
        var sack3 = group.ElementAt(2);
        foreach (var item in sack1.Both)
        {
            if (sack2.Both.Contains(item) && sack3.Both.Contains(item))
                return item;
        }
        throw new Exception("No badge found");
    }
    public static List<IEnumerable<Rucksack>> GetGroups(List<Rucksack> sacks)
    {
        List<IEnumerable<Rucksack>> groups = new();
        for (int i = 0; i < sacks.Count(); i += 3)
        {
            groups.Add(sacks.Skip(i).Take(3));

        }
        // sacks.RemoveRange(0, 3);
        return groups;
    }

}
public class Rucksack
{
    public string Compartment1 { get; private set; }
    public string Compartment2 { get; private set; }
    public string Both { get; private set; }
    public Rucksack(string input)
    {
        Both = input;
        // Insert dash on half of the string
        input = input.Insert(input.Count() / 2, "-");
        string[] compartments = input.Split("-");
        Compartment1 = compartments[0];
        Compartment2 = compartments[1];
    }
    public override string ToString()
    {
        return $"Comp1: [{Compartment1}] - Comp2: [{Compartment2}]";
    }
    public char FindDuplicate()
    {
        foreach (var item in Compartment1.ToCharArray())
        {
            if (Compartment2.Contains(item))
                return item;
        }
        throw new Exception("No duplicate");
    }
}
public class Priorities
{
    private Dictionary<char, int> priorityDict = new();
    public Priorities()
    {
        const string letterString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var count = 1;
        foreach (var letter in letterString)
        {
            priorityDict.Add(letter, count);
            count++;
        }
    }
    public override string ToString()
    {
        return string.Join("|", priorityDict.Select(kvp => kvp.Key + ": " + kvp.Value.ToString()));
    }
    public int GetPrio(char c) => priorityDict[c];
}