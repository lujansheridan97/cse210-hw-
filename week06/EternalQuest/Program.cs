using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    private static List<Goal> _goals = new List<Goal>();
    private static int _totalPoints = 0;

    public static void Main(string[] args)
    {
        LoadGoals();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Eternal Quest - Goal Tracker");
            Console.WriteLine("1. Create a new goal");
            Console.WriteLine("2. Display goals");
            Console.WriteLine("3. Record an event");
            Console.WriteLine("4. Display total points");
            Console.WriteLine("5. Save and Exit");
            Console.Write("Choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    DisplayGoals();
                    break;
                case "3":
                    RecordEvent();
                    break;
                case "4":
                    Console.WriteLine($"Total Points: {_totalPoints}");
                    Console.ReadKey();
                    break;
                case "5":
                    SaveGoals();
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static void CreateGoal()
    {
        Console.WriteLine("Enter goal type (simple, eternal, checklist): ");
        string goalType = Console.ReadLine().ToLower();

        Console.WriteLine("Enter goal name: ");
        string name = Console.ReadLine();

        Console.WriteLine("Enter goal description: ");
        string description = Console.ReadLine();

        Goal goal = null;
        switch (goalType)
        {
            case "simple":
                goal = new SimpleGoal(name, description);
                break;
            case "eternal":
                goal = new EternalGoal(name, description);
                break;
            case "checklist":
                Console.WriteLine("Enter number of times to complete: ");
                int target = int.Parse(Console.ReadLine());
                goal = new ChecklistGoal(name, description, target);
                break;
            default:
                Console.WriteLine("Invalid goal type.");
                return;
        }

        _goals.Add(goal);
    }

    private static void DisplayGoals()
    {
        Console.Clear();
        foreach (var goal in _goals)
        {
            goal.DisplayGoal();
        }
        Console.ReadKey();
    }

    private static void RecordEvent()
    {
        Console.WriteLine("Enter the goal number to record an event for: ");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].Name}");
        }

        int goalIndex = int.Parse(Console.ReadLine()) - 1;
        if (goalIndex >= 0 && goalIndex < _goals.Count)
        {
            _goals[goalIndex].RecordEvent();
            _totalPoints += _goals[goalIndex].Points;
        }
    }

    private static void SaveGoals()
    {
        using (StreamWriter writer = new StreamWriter("goals.txt"))
        {
            foreach (var goal in _goals)
            {
                writer.WriteLine(goal.GetStringRepresentation());
            }
        }
        Console.WriteLine("Goals saved.");
    }

    private static void LoadGoals()
    {
        if (File.Exists("goals.txt"))
        {
            var lines = File.ReadAllLines("goals.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(",");
                if (parts[0] == "SimpleGoal")
                {
                    _goals.Add(new SimpleGoal(parts[1], parts[2]) { Points = int.Parse(parts[3]), IsComplete = bool.Parse(parts[4]) });
                }
                else if (parts[0] == "EternalGoal")
                {
                    _goals.Add(new EternalGoal(parts[1], parts[2]) { Points = int.Parse(parts[3]) });
                }
                else if (parts[0] == "ChecklistGoal")
                {
                    _goals.Add(new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3])) { Points = int.Parse(parts[4]), Completed = int.Parse(parts[5]) });
                }
            }
        }
    }
}

public abstract class Goal
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }

    public Goal(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public abstract void RecordEvent();
    public abstract void DisplayGoal();
    public abstract string GetStringRepresentation();
}

public class SimpleGoal : Goal
{
    public bool IsComplete { get; set; }

    public SimpleGoal(string name, string description) : base(name, description)
    {
        Points = 100;
        IsComplete = false;
    }

    public override void RecordEvent()
    {
        if (!IsComplete)
        {
            IsComplete = true;
            Console.WriteLine($"Completed: {Name} (+{Points} pts)");
        }
        else
        {
            Console.WriteLine($"{Name} is already complete.");
        }
    }

    public override void DisplayGoal()
    {
        string status = IsComplete ? "[X]" : "[ ]";
        Console.WriteLine($"{status} {Name} - {Description}");
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal,{Name},{Description},{Points},{IsComplete}";
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, string description) : base(name, description)
    {
        Points = 50;
    }

    public override void RecordEvent()
    {
        Console.WriteLine($"Recorded: {Name} (+{Points} pts)");
    }

    public override void DisplayGoal()
    {
        Console.WriteLine($"[∞] {Name} - {Description}");
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal,{Name},{Description},{Points}";
    }
}

public class ChecklistGoal : Goal
{
    public int TargetCount { get; set; }
    public int Completed { get; set; }

    public ChecklistGoal(string name, string description, int targetCount) : base(name, description)
    {
        TargetCount = targetCount;
        Completed = 0;
        Points = 25;
    }

    public override void RecordEvent()
    {
        if (Completed < TargetCount)
        {
            Completed++;
            Console.WriteLine($"Progress: {Name} ({Completed}/{TargetCount}) (+{Points} pts)");
        }
        else
        {
            Console.WriteLine($"{Name} is already complete.");
        }
    }

    public override void DisplayGoal()
    {
        string status = Completed >= TargetCount ? "[X]" : "[ ]";
        Console.WriteLine($"{status} {Name} - {Description} ({Completed}/{TargetCount})");
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal,{Name},{Description},{TargetCount},{Points},{Completed}";
    }
}
