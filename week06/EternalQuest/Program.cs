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
