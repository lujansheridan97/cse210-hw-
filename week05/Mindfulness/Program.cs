using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

abstract class Activity
{
    protected int duration;
    protected string activityName;
    protected string description;

    public Activity(string name, string desc)
    {
        activityName = name;
        description = desc;
    }

    // Start the activity with a prompt for the duration
    public void Start()
    {
        Console.WriteLine($"{activityName} - {description}");
        Console.Write("How many seconds would you like to spend on this activity? ");
        duration = int.Parse(Console.ReadLine());
        Console.WriteLine("Get ready...");
        Pause(3); // Pause for 3 seconds before starting
    }

    // Common method to simulate a pause
    protected void Pause(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000); // Display a "." every second for a pause
        }
        Console.WriteLine();
    }

    // Abstract method to be implemented by each specific activity
    public abstract void Run();

    // End the activity with a common message
    public void End()
    {
        Console.WriteLine($"Great job! You completed {activityName} for {duration} seconds.");
        Pause(3);
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity() : base("Breathing Activity", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.") { }

    public override void Run()
    {
        int elapsedTime = 0;
        while (elapsedTime < duration)
        {
            Console.WriteLine("Breathe in...");
            Pause(3); // Pause for 3 seconds (simulate breathing in)
            Console.WriteLine("Breathe out...");
            Pause(3); // Pause for 3 seconds (simulate breathing out)
            elapsedTime += 6; // 6 seconds for each cycle
        }
    }
}

class ReflectionActivity : Activity
{
    private List<string> prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need."
    };

    private List<string> questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "How did you feel when it was complete?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?"
    };

    public ReflectionActivity() : base("Reflection Activity", "This activity will help you reflect on times in your life when you have shown strength and resilience.") { }

    public override void Run()
    {
        Random rand = new Random();
        string prompt = prompts[rand.Next(prompts.Count)];
        Console.WriteLine(prompt);
        Pause(3); // Pause for a few seconds after displaying the prompt

        int elapsedTime = 0;
        while (elapsedTime < duration)
        {
            string question = questions[rand.Next(questions.Count)];
            Console.WriteLine(question);
            Pause(4); // Pause for 4 seconds after each question
            elapsedTime += 4; // 4 seconds for each question
        }
    }
}

class ListingActivity : Activity
{
    private List<string> prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?"
    };

    public ListingActivity() : base("Listing Activity", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.") { }

    public override void Run()
    {
        Random rand = new Random();
        string prompt = prompts[rand.Next(prompts.Count)];
        Console.WriteLine(prompt);
        Pause(3); // Pause for a few seconds after displaying the prompt

        Console.WriteLine("Start listing items (type 'done' to finish): ");
        List<string> items = new List<string>();
        DateTime startTime = DateTime.Now;

        while ((DateTime.Now - startTime).TotalSeconds < duration)
        {
            string item = Console.ReadLine();
            if (item.ToLower() == "done") break;
            items.Add(item);
        }

        Console.WriteLine($"You listed {items.Count} items.");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Mindfulness Activity App!");
        while (true)
        {
            // Display menu
            Console.WriteLine("\nPlease select an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Exit");
            Console.Write("Your choice: ");
            int choice = int.Parse(Console.ReadLine());

            Activity activity = null;

            switch (choice)
            {
                case 1:
                    activity = new BreathingActivity();
                    break;
                case 2:
                    activity = new ReflectionActivity();
                    break;
                case 3:
                    activity = new ListingActivity();
                    break;
                case 4:
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    continue;
            }

            activity.Start();
            activity.Run();
            activity.End();
        }
    }
}
