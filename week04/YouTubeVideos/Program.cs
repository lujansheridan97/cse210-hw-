using System;
using System.Collections.Generic;

class Comment
{
    public string UserName { get; set; }
    public string Text { get; set; }

    public Comment(string userName, string text)
    {
        UserName = userName;
        Text = text;
    }

    public string GetCommentDetails()
    {
        return $"{UserName}: {Text}";
    }
}

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; }
    public List<Comment> Comments { get; set; }

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        Comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return Comments.Count;
    }

    public void GetVideoDetails()
    {
        Console.WriteLine($"Title: {Title}\nAuthor: {Author}\nLength: {Length} seconds\nComments ({GetCommentCount()}):");
        foreach (var comment in Comments)
        {
            Console.WriteLine("- " + comment.GetCommentDetails());
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        List<Video> videos = new List<Video>
        {
            new Video("Learn C# in One Hour", "Code Academy", 3600),
            new Video("Object-Oriented Programming Basics", "Tech Tutor", 2400),
            new Video("Advanced C# Techniques", "Dev Expert", 3000)
        };

        videos[0].AddComment(new Comment("Alice", "Great tutorial!"));
        videos[0].AddComment(new Comment("Bob", "Very helpful, thanks!"));
        
        videos[1].AddComment(new Comment("Charlie", "I finally understand OOP!"));
        videos[1].AddComment(new Comment("Dana", "Clear and concise explanation."));

        videos[2].AddComment(new Comment("Eve", "Awesome content!"));
        videos[2].AddComment(new Comment("Frank", "I learned a lot from this."));

        foreach (var video in videos)
        {
            video.GetVideoDetails();
        }
    }
}