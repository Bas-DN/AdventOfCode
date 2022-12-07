namespace day07;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        Directory baseDir = new("/");
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            var currentCommand = "";
            Stack<Directory> currentDir = new();
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    // Change directory
                    if (line.StartsWith(Utils.cd))
                    {
                        currentCommand = Utils.cd;
                        if (line.StartsWith(Utils.back))
                            currentDir.Pop();
                        else
                        {
                            var newDirName = line.Split(Utils.cd)[1];
                            // Add base dir, otherwise get first in currentDir with the same name
                            if (newDirName == baseDir.Name)
                                currentDir.Push(baseDir);
                            else
                                currentDir.Push(currentDir.Peek().SubDirs.First(d => d.Name == newDirName));
                        }
                    }
                    // List items
                    else if (line.StartsWith(Utils.ls))
                        currentCommand = Utils.ls;
                    // If contents are being listed
                    else if (currentCommand == Utils.ls)
                    {
                        // If it's a directory, add it
                        if (line.StartsWith(Utils.dir))
                            currentDir.First().AddDirectory(
                                new Directory(line.Split(Utils.dir)[1])
                            );
                        else
                        {
                            currentDir.Peek().AddFile(line);
                        }
                    }
                }
            }
            Console.WriteLine($"{baseDir.Name} - {baseDir.DirSize}");
            var atMost100k = baseDir.AtMost100k();
            foreach (var item in atMost100k)
            {
                Console.Write($"[{item.Name} - {item.DirSize}]");
            }
            // Part one
            Console.WriteLine($"Total: {atMost100k.Sum(d => d.DirSize)}");

            var sorted = baseDir.AllSubDirs().OrderBy(c => c.DirSize);
            foreach (var item in sorted)
            {
                Console.WriteLine($"[{item.DirSize} - {item.Name}]");
            }
            const int totalSpace = 70000000;
            const int updateSpace = 30000000;
            var freeSpace = totalSpace - baseDir.DirSize;
            var neededSpace = updateSpace - freeSpace;
            var smallestPossibleDelete = sorted.First(d => d.DirSize >= neededSpace);
            // Part two
            Console.WriteLine($"Smallest delete:[{smallestPossibleDelete.Name} - {smallestPossibleDelete.DirSize}]");
        }
    }
}
public class Directory
{
    public string Name { get; private set; }
    public List<Directory> SubDirs { get; private set; }
    public List<Tuple<string, int>> Files { get; private set; }
    public Directory(string s)
    {
        Name = s;
        SubDirs = new();
        Files = new();
    }
    public void AddDirectory(Directory d)
    {
        SubDirs.Add(d);
    }
    public void AddFile(string s)
    {
        var file = s.Split(' ');
        Files.Add(new Tuple<string, int>(file[1], int.Parse(file[0])));
    }
    public int DirSize
    {
        get
        {
            var size = 0;
            foreach (var item in Files)
                size += item.Item2;
            foreach (var item in SubDirs)
                size += item.DirSize;
            return size;
        }
    }
    public List<Directory> AllSubDirs()
    {
        var allSubDirs = new List<Directory>();
        foreach (var item in SubDirs)
            allSubDirs.AddRange(item.AllSubDirs());
        allSubDirs.Add(this);
        return allSubDirs;
    }
    public List<Directory> AtMost100k()
    {
        var atMost100k = new List<Directory>();
        if (SubDirs.Count > 0)
        {
            foreach (var item in SubDirs)
            {
                // Add items smaller than 100k
                if (item.DirSize < 100000)
                    atMost100k.Add(item);
                atMost100k.AddRange(item.AtMost100k());
            }
        }
        return atMost100k;
    }

}
public static class Utils
{
    public const string back = "$ cd ..";
    public const string cd = "$ cd ";
    public const string ls = "$ ls";
    public const string dir = "dir ";
}