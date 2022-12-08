namespace day08;

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
                    objects.Add(line);
                }
            }
        }
        var grid = new TreeGrid(objects);
        // Part one
        Console.WriteLine($"Visible trees count: {grid.GetVisibleTrees()}");
        // Part two
        Console.WriteLine($"Max scenic score: {grid.GetHighestScenicScore()}");
    }
}
public class TreeGrid
{
    public Tree[,] Grid { get; private set; }
    public TreeGrid(IEnumerable<string> grid)
    {
        var xCount = grid.First().Length;
        var yCount = grid.Count();
        Grid = new Tree[xCount, yCount];

        for (int x = 0; x < Grid.GetLength(0); x++)
        {
            var currentRow = grid.Skip(x).First();
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                var currentItem = currentRow.ToCharArray().Skip(y).First();
                Grid[x, y] = new Tree(int.Parse(currentItem.ToString()));
            }
        }
    }
    public void PrintGrid()
    {
        for (int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                Console.Write($"{Grid[x, y].Length}");
            }
            Console.WriteLine($"");
        }
    }
    public int GetVisibleTrees()
    {
        CheckVisibility();
        var count = 0;
        for (int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
                if (Grid[x, y].Visible)
                    count++;
        }
        return count;
    }
    public int GetHighestScenicScore()
    {
        var highestScore = 0;
        for (int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                Grid[x, y].SetScenicScore(ScenicTop(x, y), ScenicBottom(x, y), ScenicLeft(x, y), ScenicRight(x, y));
                if (Grid[x, y].ScenicScore > highestScore)
                    highestScore = Grid[x, y].ScenicScore;
            }
        }
        return highestScore;
    }

    private int ScenicRight(int treeX, int treeY)
    {
        var count = 0;
        if (treeY == Grid.GetLength(1) - 1)
            return count;
        for (int y = treeY + 1; y < Grid.GetLength(1); y++)
            if (Grid[treeX, y].Length >= Grid[treeX, treeY].Length)
            {
                count++; break; // Stop when a tree is bigger
            }
            else // Otherwise add one
                count++;
        return count;
    }

    private int ScenicLeft(int treeX, int treeY)
    {
        var count = 0;
        if (treeY == 0)
            return count;
        for (int y = treeY - 1; y >= 0; y--)
            if (Grid[treeX, y].Length >= Grid[treeX, treeY].Length)
            {
                count++; break; // Stop when a tree is bigger
            }
            else // Otherwise add one
                count++;
        return count;
    }

    private int ScenicBottom(int treeX, int treeY)
    {
        var count = 0;
        if (treeX == Grid.GetLength(0) - 1)
            return count;
        for (int x = treeX + 1; x < Grid.GetLength(0); x++)
            if (Grid[x, treeY].Length >= Grid[treeX, treeY].Length)
            {
                count++; break; // Stop when a tree is bigger
            }
            else // Otherwise add one
                count++;
        return count;
    }

    private int ScenicTop(int treeX, int treeY)
    {
        var count = 0;
        if (treeX == 0)
            return count;
        for (int x = treeX - 1; x >= 0; x--)
            if (Grid[x, treeY].Length >= Grid[treeX, treeY].Length)
            {
                count++; break; // Stop when a tree is bigger
            }
            else // Otherwise add one
                count++;
        return count;
    }

    private void CheckVisibility()
    {
        for (int x = 0; x < Grid.GetLength(0); x++)
            for (int y = 0; y < Grid.GetLength(1); y++)
                if (IsHighestFromRight(x, y) || IsHighestFromLeft(x, y) || IsHighestFromBottom(x, y) || IsHighestFromTop(x, y))
                    Grid[x, y].IsVisible();
    }
    private bool IsHighestFromTop(int treeX, int treeY)
    {
        var currentTreeLength = Grid[treeX, treeY].Length;
        if (treeX == 0)
            return true;
        for (int x = treeX - 1; x >= 0; x--)
            if (Grid[x, treeY].Length >= currentTreeLength) // Tree next to it
                return false;
        return true;
    }
    private bool IsHighestFromBottom(int treeX, int treeY)
    {
        var currentTreeLength = Grid[treeX, treeY].Length;
        if (treeX == Grid.GetLength(0) - 1)
            return true;
        for (int x = treeX + 1; x < Grid.GetLength(0); x++)
            if (Grid[x, treeY].Length >= currentTreeLength) // Tree next to it
                return false;
        return true;
    }
    private bool IsHighestFromLeft(int treeX, int treeY)
    {
        var currentTreeLength = Grid[treeX, treeY].Length;
        if (treeY == 0)
            return true;
        for (int y = treeY - 1; y >= 0; y--)
            if (Grid[treeX, y].Length >= currentTreeLength) // Tree next to it
                return false;
        return true;
    }
    private bool IsHighestFromRight(int treeX, int treeY)
    {
        var currentTreeLength = Grid[treeX, treeY].Length;
        if (treeY == Grid.GetLength(1) - 1)
            return true;
        for (int y = treeY + 1; y < Grid.GetLength(1); y++)
            if (Grid[treeX, y].Length >= currentTreeLength) // Tree next to it
                return false;
        return true;
    }
}
public class Tree
{
    public bool Visible { get; private set; }
    public int Length { get; private set; }
    public int ScenicScore { get; private set; }
    public Tree(int length)
    {
        Length = length;
        Visible = false;
    }
    public void IsVisible() => Visible = true;
    public void SetScenicScore(int top, int bottom, int left, int right) => ScenicScore = top * bottom * left * right;
}