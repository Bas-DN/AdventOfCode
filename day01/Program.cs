namespace day01;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        List<Elf> elves = new();
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            List<int> calories = new();
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    calories.Add(Int32.Parse(line));
                }
                else
                {
                    elves.Add(new(calories));
                    calories = new();
                }
            }
        }
        // var count = 0;
        // foreach (var item in elves)
        // {
        //     _testOutputHelper.WriteLine($"{count} - {item.GetTotalCalories()}");
        //     count++;
        // }
        List<int> totalCalories = new();
        foreach (var item in elves)
        {
            totalCalories.Add(item.GetTotalCalories());
        }
        var value = totalCalories.Max();
        // First half answer
        Console.WriteLine($"MAX - {value}");
        var values = totalCalories
            .OrderByDescending(x => x)
            .Take(3);
        foreach (var valuee in values)
        {
            Console.WriteLine($"Top 3 total: - {valuee}");
        }
        var top3Total = values.Sum();
        // Second half answer
        Console.WriteLine($"Top 3 total: - {top3Total}");
    }
}


public class Elf
{
    IEnumerable<int> Calories { get; set; }
    public Elf(IEnumerable<int> calories)
    {
        Calories = calories;
    }
    public int GetTotalCalories()
    {
        return Calories.Sum();
    }
}