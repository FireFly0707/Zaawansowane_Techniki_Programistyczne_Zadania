using System;
using System.Collections.Generic;

public interface IMessage
{
    int Id { get; set; }
    string Title { get; set; }
    string Content { get; set; }
}
public class Message : IMessage
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Message(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
public interface IMessageBox
{
    void AddMessage(IMessage message);
    IMessage GetMessageById(int id);
    void DisplayAllMessageTitles();
}

public class MessageBox : IMessageBox
{
    private List<IMessage> messages = new List<IMessage>();
    private int nextId = 1;

    public void AddMessage(IMessage message)
    {
        message.Id = nextId++;
        messages.Add(message);
    }

    public IMessage GetMessageById(int id)
    {
        return messages.Find(m => m.Id == id);
    }

    public void DisplayAllMessageTitles()
    {
        Console.WriteLine("\nLista wiadomości:");
        if (messages.Count == 0)
        {
            Console.WriteLine("Brak wiadomości w skrzynce.");
        }
        else
        {
            foreach (var message in messages)
            {
                Console.WriteLine($"ID: {message.Id} - {message.Title}");
            }
        }
    }

}
public abstract class MessageBoxDecorator : IMessageBox
{
    protected readonly IMessageBox innerMessageBox;

    public MessageBoxDecorator(IMessageBox messageBox)
    {
        innerMessageBox = messageBox;
    }

    public virtual void AddMessage(IMessage message)
    {
        innerMessageBox.AddMessage(message);
    }

    public virtual IMessage GetMessageById(int id)
    {
        return innerMessageBox.GetMessageById(id);
    }

    public virtual void DisplayAllMessageTitles()
    {
        innerMessageBox.DisplayAllMessageTitles();
    }
}
public class BlockedWordsDecorator : MessageBoxDecorator
{
    private readonly List<string> blockedWords;

    public BlockedWordsDecorator(IMessageBox messageBox, List<string> blockedWords)
        : base(messageBox)
    {
        this.blockedWords = blockedWords;
    }

    public override void AddMessage(IMessage message)
    {
        foreach (var word in blockedWords)
        {
            if (message.Content.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Wiadomość \"{message.Title}\" została zablokowana (zawiera zakazane słowo: \"{word}\").");
                return;
            }
        }
        base.AddMessage(message); // Dodanie wiadomości, jeśli przeszła filtr.
    }
}
public class HiddenMessageDecorator : MessageBoxDecorator
{
    private readonly List<string> blockedWords;

    // Obiekt Null Object (ukryta wiadomość).
    private readonly IMessage hiddenMessage = new Message("Ukryta wiadomość", "Treść tej wiadomości została ukryta.");

    public HiddenMessageDecorator(IMessageBox messageBox, List<string> blockedWords)
        : base(messageBox)
    {
        this.blockedWords = blockedWords;
    }

    public override IMessage GetMessageById(int id)
    {
        var message = base.GetMessageById(id);
        if (message == null) return null;

        foreach (var word in blockedWords)
        {
            if (message.Content.Contains(word, StringComparison.OrdinalIgnoreCase))
            {
                return hiddenMessage;
            }
        }
        return message; // Zwraca wiadomość, jeśli nie zawiera zakazanych słów.
    }
}

class Program
{
    static void Main(string[] args)
    {
        MessageBox messageBox = new MessageBox();

        // Dodanie przykładowych wiadomości
        messageBox.AddMessage(new Message("Powiadomienie o spotkaniu", "Spotkanie zespołu odbędzie się w piątek o godzinie 10:00."));
        messageBox.AddMessage(new Message("Nowe zasady pracy zdalnej", "Od przyszłego miesiąca obowiązują nowe zasady pracy zdalnej."));
        messageBox.AddMessage(new Message("Wyniki kwartalne", "Wyniki finansowe za ostatni kwartał pokazują wzrost o 15%."));

        bool running = true;
        while (running)
        {
            // Wyświetlanie wszystkich tematów wiadomości
            messageBox.DisplayAllMessageTitles();

            Console.WriteLine("\nWybierz ID wiadomości do wyświetlenia (lub 0, aby zakończyć): ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (id == 0)
                {
                    running = false;
                    Console.WriteLine("Koniec programu.");
                }
                else
                {
                    var message = messageBox.GetMessageById(id);
                    if (message != null)
                    {
                        Console.WriteLine($"\nTytuł: {message.Title}");
                        Console.WriteLine($"Treść: {message.Content}");
                    }
                    else
                    {
                        Console.WriteLine("Nie znaleziono wiadomości o podanym ID.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór.");
            }
        }
    }
}
