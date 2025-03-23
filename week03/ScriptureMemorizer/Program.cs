using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class Program
{
    static void Main()
    {
        Scripture scripture = new Scripture(new Reference("Proverbs", 3, 5, 6), "Trust in the Lord with all your heart and lean not on your own understanding; in all your ways submit to him, and he will make your paths straight.");
        
        while (true)
        {
            while (!scripture.IsCompletelyHidden())
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine("\nPress Enter to hide words or type 'quit' to exit.");
                
                string input = Console.ReadLine();
                if (input.ToLower() == "quit")
                    return;
                
                scripture.HideRandomWords(2);
            }

            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nCongratulations! You have hidden the entire scripture.");
            Console.WriteLine("Press Enter to restart or type 'quit' to exit.");
            
            string restartInput = Console.ReadLine();
            if (restartInput.ToLower() == "quit")
                return;
            
            scripture.ResetWords();
        }
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _verse;
    private int? _endVerse;

    public Reference(string book, int chapter, int verse, int? endVerse = null)
    {
        _book = book;
        _chapter = chapter;
        _verse = verse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        return _endVerse.HasValue ? $"{_book} {_chapter}:{_verse}-{_endVerse}" : $"{_book} {_chapter}:{_verse}";
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _random = new Random();

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(w => new Word(w)).ToList();
    }

    public string GetDisplayText()
    {
        return $"{_reference.GetDisplayText()}\n" + string.Join(" ", _words.Select(w => w.GetDisplayText()));
    }

    public void HideRandomWords(int numberToHide)
    {
        var visibleWords = _words.Where(w => !w.IsHidden()).ToList();
        if (visibleWords.Count == 0) return;

        for (int i = 0; i < numberToHide && visibleWords.Count > 0; i++)
        {
            int index = _random.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(w => w.IsHidden());
    }

    public void ResetWords()
    {
        foreach (var word in _words)
        {
            word.Show();
        }
    }
}

class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public void Show()
    {
        _isHidden = false;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        return _isHidden ? new string('_', _text.Length) : _text;
    }
}
