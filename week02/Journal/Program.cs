using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static Journal journal = new Journal();
    static string passwordHashPath = "password.txt";

    static void Main()
    {
        if (!AuthenticateUser()) return;

        while (true)
        {
            Console.WriteLine("\nJournal Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal");
            Console.WriteLine("4. Load the journal");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": journal.AddEntry(); break;
                case "2": journal.DisplayEntries(); break;
                case "3":
                    Console.Write("Enter filename to save: ");
                    string saveFile = Console.ReadLine();
                    journal.SaveToFile(saveFile);
                    break;
                case "4":
                    Console.Write("Enter filename to load: ");
                    string loadFile = Console.ReadLine();
                    journal.LoadFromFile(loadFile);
                    break;
                case "5": return;
                default: Console.WriteLine("Invalid option. Try again."); break;
            }
        }
    }

    static bool AuthenticateUser()
    {
        if (File.Exists(passwordHashPath))
        {
            Console.Write("Enter your password: ");
            string enteredPassword = Console.ReadLine();
            string storedHash = File.ReadAllText(passwordHashPath);

            if (ComputeHash(enteredPassword) == storedHash)
            {
                Console.WriteLine("Access granted.");
                return true;
            }
            else
            {
                Console.WriteLine("Incorrect password. Access denied.");
                return false;
            }
        }
        else
        {
            Console.Write("No password found. Create a new password: ");
            string newPassword = Console.ReadLine();
            File.WriteAllText(passwordHashPath, ComputeHash(newPassword));
            Console.WriteLine("Password set. Restart the program to access your journal.");
            return false;
        }
    }

    static string ComputeHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}

class Entry
{
    public DateTime Date { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }

    public override string ToString()
    {
        return $"{Date} - {Prompt} - {Response}";
    }
}

class Journal
{
    private List<Entry> entries = new List<Entry>();
    private List<string> prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    public void AddEntry()
    {
        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Count)];
        Console.WriteLine($"\n{prompt}");
        string response = Console.ReadLine();
        entries.Add(new Entry { Date = DateTime.Now, Prompt = prompt, Response = response });
    }

    public void DisplayEntries()
    {
        Console.WriteLine("\nJournal Entries:");
        foreach (var entry in entries)
            Console.WriteLine(entry.ToString());
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename, true)) // Append instead of overwrite
        {
            foreach (var entry in entries)
            {
                writer.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}");
            }
        }
        Console.WriteLine("Journal saved.");
    }

    public void LoadFromFile(string filename)
    {
        if (File.Exists(filename))
        {
            entries.Clear();
            foreach (var line in File.ReadAllLines(filename))
            {
                var parts = line.Split('|');
                if (parts.Length == 3)
                {
                    entries.Add(new Entry { Date = DateTime.Parse(parts[0]), Prompt = parts[1], Response = parts[2] });
                }
            }
            Console.WriteLine("Journal loaded.");
        }
        else
        {
            Console.WriteLine("No saved journal found.");
        }
    }
}
