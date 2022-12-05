namespace day05;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        List<Movement> movements = new();
        Stack<Stack<string>> allCrates = new();
        var width = 0;
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            bool init = true;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0 && init)
                {
                    if (width == 0)
                        width = Utils.CalcWidth(line.Length);
                    var crateRow = new Stack<string>();
                    for (int i = 0; i < width; i++)
                    {
                        // Get crates
                        var crate = line.Substring(i * 3 + i, 3);
                        // Stop when number appears
                        if (int.TryParse(crate, out int result))
                            break;
                        crateRow.Push(crate);
                        Console.WriteLine($"{crate}");
                    }
                    allCrates.Push(crateRow);
                }
                else
                {
                    init = false;
                    if (!string.IsNullOrWhiteSpace(line))
                        movements.Add(new Movement(line));
                }
            }
        }
        var stacksOfCrates = Utils.BuildCrateStacks(allCrates, width);
        Console.WriteLine($"CRATES CREATED");
        foreach (var item in stacksOfCrates)
        {
            Console.WriteLine(item.ToString());
        }
        // First part crate moving
        // foreach (var item in movements)
        // {
        //     for (int i = 0; i < item.Amount; i++)
        //     {
        //         // Add the popped crate to the other stack
        //         stacksOfCrates[item.ToStack - 1].AddCrate(
        //             stacksOfCrates[item.FromStack - 1].PopCrate()
        //         );
        //     }
        // }
        // Console.WriteLine($"CRATES MOVED");

        foreach (var item in stacksOfCrates)
        {
            Console.WriteLine(item.ToString());
        }

        // First part printing top
        // foreach (var item in stacksOfCrates)
        // {
        //     Console.Write(item.PopCrate());
        // }
        // Console.WriteLine($"");
        foreach (var item in movements)
        {
            // Add the popped crate to the other stack
            stacksOfCrates[item.ToStack - 1].AddCrates(
                stacksOfCrates[item.FromStack - 1].PopCrates(item.Amount)
            );
        }
        // Second part
        foreach (var item in stacksOfCrates)
        {
            Console.Write(item.PopCrate());
        }
        Console.WriteLine($"");
    }
}
public class StackOfCrates
{
    private Stack<char> crates = new();
    public void AddCrate(string s)
    {
        if (!string.IsNullOrWhiteSpace(s))
        {
            var c = s.Split('[', ']')[1].ToCharArray().First();
            crates.Push(c);
        }
    }
    public void AddCrate(char c)
    {
        crates.Push(c);
    }
    public void AddCrates(char[] c)
    {
        for (int i = c.Length - 1; i >= 0; i--)
        {
            crates.Push(c[i]);
        }
    }
    public char PopCrate()
    {
        return crates.Pop();
    }
    public char[] PopCrates(int amount)
    {
        var poppedCrates = new char[amount];
        for (int i = 0; i < amount; i++)
        {
            poppedCrates[i] = crates.Pop();
        }
        return poppedCrates;
    }
    public int Count()
    {
        return crates.Count;
    }
    public override string ToString()
    {
        return String.Join(",", crates); ;
    }
}
public static class Utils
{
    public static StackOfCrates[] BuildCrateStacks(Stack<Stack<string>> allCrates, int stackCount)
    {
        var stacks = new StackOfCrates[stackCount];
        while (allCrates.Count > 0)
        {
            // Reverse for loop to start at the end
            for (int i = stackCount; i >= 0; i--)
            {
                if (allCrates.Count == 0)
                    break;
                var test = allCrates.Pop();
                // Add all crates from that row
                for (int j = stackCount - 1; j >= 0; j--)
                {
                    if (test.Any())
                    {
                        if (stacks[j] == null)
                            stacks[j] = new();
                        stacks[j].AddCrate(test.Pop());
                    }
                }
            }
        }
        return stacks;
    }

    internal static int CalcWidth(int length)
    {
        var width = 0;
        while (length != 0)
        {
            width++;
            length -= 3;
            //whitespace
            if (length > 0)
                length--;
        }
        return width;
    }
}
public class Movement
{
    public int Amount { get; private set; }
    public int FromStack { get; private set; }
    public int ToStack { get; private set; }
    public Movement(string s)
    {
        foreach (var item in s.Split(" "))
        {
            int result;
            if (int.TryParse(item, out result) && Amount == 0)
                Amount = result;
            else if (int.TryParse(item, out result) && FromStack == 0)
                FromStack = result;
            else if (int.TryParse(item, out result) && ToStack == 0)
                ToStack = result;
        }
    }

}