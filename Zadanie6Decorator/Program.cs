using System;
using System.Collections.Generic;
using Zadanie6Decorator;

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
        IMessage decoratedMessage = new MessageWithReadFlag(message);
        messages.Add(decoratedMessage);
    }

    public IMessage GetMessageById(int id)
    {
        var message = messages.Find(m => m.Id == id);
        if (message is MessageWithReadFlag messageWithReadFlag)
        {
            messageWithReadFlag.MarkAsRead(); // Oznacz jako odczytaną
        }
        return message;
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



class Program
{
    static void Main(string[] args)
    {
        IMessageBox messageBox = new MessageBox();

        // Test 1: Wiadomość z dekoratorem daty
        var message1 = new Message("Powiadomienie o spotkaniu", "Spotkanie zespołu odbędzie się w piątek o godzinie 10:00.");
        var messageWithDate = new MessageWithDate(message1, DateTime.Now.AddDays(-2));
        messageBox.AddMessage(messageWithDate);

        // Test 2: Wiadomość z dekoratorem flagi odczytu
        var message2 = new Message("Nowe zasady pracy zdalnej", "Od przyszłego miesiąca obowiązują nowe zasady pracy zdalnej.");
        var messageWithReadFlag = new MessageWithReadFlag(message2);
        messageBox.AddMessage(messageWithReadFlag);

        // Test 3: Kombinacja dekoratorów: flaga odczytu + data wysłania
        var message3 = new Message("Wyniki kwartalne", "Wyniki finansowe za ostatni kwartał pokazują wzrost o 15%.");
        var messageWithDateAndReadFlag = new MessageWithReadFlag(new MessageWithDate(message3, DateTime.Now.AddDays(-5)));
        messageBox.AddMessage(messageWithDateAndReadFlag);

        bool running = true;

        while (running)
        {
            // Wyświetlanie wszystkich tematów wiadomości
            Console.WriteLine("\nLista wszystkich wiadomości:");
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
                        Console.WriteLine($"--- Wiadomość została odczytana: {(message is MessageWithReadFlag readFlag && readFlag.IsRead)}");
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

