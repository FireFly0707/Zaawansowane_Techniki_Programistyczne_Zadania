using System;
using System.Collections.Generic;
using System.Linq;

public interface INewsService
{
    Response AddMessage(string title, string content);
    Response ReadMessage(int id);
    Response EditMessage(int id, string newContent);
    Response DeleteMessage(int id);
}

public class Response
{
    public string Status { get; set; }
    public string Message { get; set; }

    public Response(string status, string message)
    {
        Status = status;
        Message = message;
    }
}

public class User
{
    public string Name { get; set; }
    public UserRole Role { get; set; }

    public User(string name, UserRole role)
    {
        Name = name;
        Role = role;
    }
}

public enum UserRole
{
    Guest,
    User,
    Moderator,
    Admin
}
public class Message
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Message(int id, string title, string content)
    {
        Id = id;
        Title = title;
        Content = content;
    }
}

public class NewsService : INewsService
{
    private List<Message> _messages;
    private int _nextId;

    public NewsService()
    {
        _messages = new List<Message>();
        _nextId = 1;
    }

    public Response AddMessage(string title, string content)
    {
        var message = new Message(_nextId++, title, content);
        _messages.Add(message);
        return new Response("Success", "Message added successfully.");
    }

    public Response ReadMessage(int id)
    {
        var message = _messages.FirstOrDefault(m => m.Id == id);
        if (message != null)
        {
            return new Response("Success", $"{message.Title}: {message.Content}");
        }
        return new Response("Error", "Message not found.");
    }

    public Response EditMessage(int id, string newContent)
    {
        var message = _messages.FirstOrDefault(m => m.Id == id);
        if (message == null)
        {
            return new Response("Error", "Message not found.");
        }

        message.Content = newContent;
        return new Response("Success", "Message edited successfully.");
    }

    public Response DeleteMessage(int id)
    {
        var message = _messages.FirstOrDefault(m => m.Id == id);
        if (message == null)
        {
            return new Response("Error", "Message not found.");
        }

        _messages.Remove(message);
        return new Response("Success", "Message deleted successfully.");
    }
}
public class NewsServiceProxy : INewsService
{
    private User _user;
    private static INewsService _newsService = new NewsService();
    private static Dictionary<int, Response> _cache = new Dictionary<int, Response>();
    public NewsServiceProxy(User user)
    {
        _user = user;
        
    }
    private void CleanCache(int messageId)
    {
        _cache.Remove(messageId);
    }
    public Response AddMessage(string title, string content)
    {
        if (_user.Role == UserRole.Admin || _user.Role == UserRole.Moderator || _user.Role == UserRole.User)
        {
            return _newsService.AddMessage(title, content);
        }
        else return new Response("Error", "You Don't have permision to do that");
    }

    public Response ReadMessage(int id)
    {
        if(_cache.ContainsKey(id))
        {
            return _cache[id];
        }
        else
        {
            _cache.Add(id,_newsService.ReadMessage(id));
            return _cache[id];
        }
            
    }

    public Response EditMessage(int id, string newContent)
    {
       if(_user.Role == UserRole.Admin || _user.Role == UserRole.Moderator)
        {
            CleanCache(id);
            return _newsService.EditMessage(id, newContent);
        }
        else return new Response("Error", "You Don't have permision to do that");
    }

    public Response DeleteMessage(int id)
    {
        if (_user.Role == UserRole.Admin)
        {
            CleanCache(id);
            return _newsService.DeleteMessage(id);
        }
        else return new Response("Error", "You Don't have permision to do that");
    }
}

public class Program
{
    public static void Main()
    {
        // Tworzenie instancji użytkowników z różnymi rolami
        var adminUser = new User("AdminUser", UserRole.Admin);
        var moderatorUser = new User("ModeratorUser", UserRole.Moderator);
        var regularUser = new User("RegularUser", UserRole.User);
        var guestUser = new User("GuestUser", UserRole.Guest);

        // Tworzenie instancji NewsServiceProxy dla każdego użytkownika
        var adminProxy = new NewsServiceProxy(adminUser);
        var moderatorProxy = new NewsServiceProxy(moderatorUser);
        var userProxy = new NewsServiceProxy(regularUser);
        var guestProxy = new NewsServiceProxy(guestUser);

        // Testy dla użytkownika Admin
        Console.WriteLine("Admin Tests:");
        Console.WriteLine(adminProxy.AddMessage("Admin Title", "Admin Content").Message);
        Console.WriteLine(adminProxy.ReadMessage(1).Message);
        Console.WriteLine(adminProxy.EditMessage(1, "Edited by Admin").Message);
        Console.WriteLine(adminProxy.DeleteMessage(1).Message);
        Console.WriteLine();

        // Testy dla użytkownika Moderator
        Console.WriteLine("Moderator Tests:");
        Console.WriteLine(moderatorProxy.AddMessage("Moderator Title", "Moderator Content").Message);
        Console.WriteLine(moderatorProxy.ReadMessage(2).Message);
        Console.WriteLine(moderatorProxy.EditMessage(2, "Edited by Moderator").Message);
        Console.WriteLine(moderatorProxy.DeleteMessage(2).Message);  // Oczekiwany błąd
        Console.WriteLine();

        // Testy dla zwykłego użytkownika
        Console.WriteLine("User Tests:");
        Console.WriteLine(userProxy.AddMessage("User Title", "User Content").Message);
        Console.WriteLine(userProxy.ReadMessage(3).Message);
        Console.WriteLine(userProxy.EditMessage(3, "Edited by User").Message);  // Oczekiwany błąd
        Console.WriteLine(userProxy.DeleteMessage(3).Message);  // Oczekiwany błąd
        Console.WriteLine();

        // Testy dla gościa
        Console.WriteLine("Guest Tests:");
        Console.WriteLine(guestProxy.AddMessage("Guest Title", "Guest Content").Message);  // Oczekiwany błąd
        Console.WriteLine(guestProxy.ReadMessage(3).Message);
        Console.WriteLine(guestProxy.EditMessage(3, "Edited by Guest").Message);  // Oczekiwany błąd
        Console.WriteLine(guestProxy.DeleteMessage(3).Message);  // Oczekiwany błąd
        Console.WriteLine();

        // Testy dla cache
        Console.WriteLine("Cache Tests:");
        Console.WriteLine(adminProxy.AddMessage("Guest Title", "Guest Content").Message);
        Console.WriteLine(adminProxy.ReadMessage(3).Message); // Pierwsze pobranie, powinno dodać do cache
        Console.WriteLine(adminProxy.ReadMessage(3).Message); // Drugi raz, powinno być z cache
        adminProxy.EditMessage(3, "New Content after Edit"); // Edycja powinna wyczyścić cache
        Console.WriteLine(adminProxy.ReadMessage(3).Message); // Ponowne pobranie po edycji, powinno być odświeżone

        Console.WriteLine("Testy zakończone.");
    }


}
