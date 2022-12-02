namespace day02;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STARTING");
        FileStream fileStream = new FileStream("input.txt", FileMode.Open);
        List<Round> rounds = new();
        using (StreamReader reader = new StreamReader(fileStream))
        {
            String? line;
            List<int> calories = new();
            while ((line = reader.ReadLine()) != null)
            {
                var roundArray = line.Split(' ');
                rounds.Add(new Round(roundArray[0], roundArray[1]));
            }

        }
        // Part 1
        foreach (var round in rounds)
        {
            Console.WriteLine($"S: {round.SelfChoiceRPS.ToString()} {round.SelfScore}");
        }
        Console.WriteLine($"{rounds.Count()} Total S: {rounds.Sum(round => round.SelfScore)}");
        // Part 2
        foreach (var round in rounds)
        {
            Console.WriteLine($"S: {round.SelfOutcome.ToString()} {round.SelfScoreBasedOnOutcome}");
        }
        Console.WriteLine($"{rounds.Count()} Total S: {rounds.Sum(round => round.SelfScoreBasedOnOutcome)}");
    }
}
public class Round
{
    public string Opponent { get; private set; }
    public string Self { get; private set; }
    public Round(string opponent, string self)
    {
        Opponent = opponent;
        Self = self;
    }
    // XYZ based on RPS
    public RPS SelfChoiceRPS
    {
        get
        {
            switch (Self)
            {
                case "X": return RPS.Rock;
                case "Y": return RPS.Paper;
                case "Z": return RPS.Scissors;
                default: return RPS.NOPE;
            }
        }
    }
    public RPS OpponentChoice
    {
        get
        {
            switch (Opponent)
            {
                case "A": return RPS.Rock;
                case "B": return RPS.Paper;
                case "C": return RPS.Scissors;
                default: return RPS.NOPE;
            }
        }
    }
    private Outcome GetOutcome(RPS p1, RPS p2)
    {
        if (p1 == p2)
            return Outcome.Draw;
        switch (p1, p2)
        {
            case (RPS.Paper, RPS.Rock): return Outcome.Win;
            case (RPS.Rock, RPS.Scissors): return Outcome.Win;
            case (RPS.Scissors, RPS.Paper): return Outcome.Win;
            case (RPS.Paper, RPS.Scissors): return Outcome.Lose;
            case (RPS.Rock, RPS.Paper): return Outcome.Lose;
            case (RPS.Scissors, RPS.Rock): return Outcome.Lose;
            default: return Outcome.NOPE;
        }
    }
    public int SelfScore
    {
        get
        {
            return (int)SelfChoiceRPS + (int)GetOutcome(SelfChoiceRPS, OpponentChoice);
        }
    }
    // XYZ based on Outcome
    public Outcome SelfOutcome
    {
        get
        {
            switch (Self)
            {
                case "X": return Outcome.Lose;
                case "Y": return Outcome.Draw;
                case "Z": return Outcome.Win;
                default: return Outcome.NOPE;
            }
        }
    }
    public RPS ChoiceBasedOnEnd(Outcome o, RPS p2)
    {
        if (o == Outcome.Draw)
            return p2;
        switch (o, p2)
        {
            case (Outcome.Win, RPS.Rock): return RPS.Paper;
            case (Outcome.Win, RPS.Scissors): return RPS.Rock;
            case (Outcome.Win, RPS.Paper): return RPS.Scissors;
            case (Outcome.Lose, RPS.Scissors): return RPS.Paper;
            case (Outcome.Lose, RPS.Paper): return RPS.Rock;
            case (Outcome.Lose, RPS.Rock): return RPS.Scissors;
            default: return RPS.NOPE;
        }
    }
    public int SelfScoreBasedOnOutcome
    {
        get
        {
            var choice = ChoiceBasedOnEnd(SelfOutcome, OpponentChoice);
            return (int)choice + (int)GetOutcome(choice, OpponentChoice);
        }
    }
}