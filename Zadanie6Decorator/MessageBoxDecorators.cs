using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie6Decorator
{
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
    public class SentDateDecorator : MessageBoxDecorator
    {
        private DateTime currentDate;

        public SentDateDecorator(IMessageBox innerMessageBox, DateTime startDate)
            : base(innerMessageBox)
        {
            currentDate = startDate;
        }

        public override void AddMessage(IMessage message)
        {
            // Dodawanie daty do treści wiadomości
            message.Content += $"\nData wysłania: {currentDate:dd.MM.yyyy}";
            base.AddMessage(message); // Wywołanie oryginalnego AddMessage
            currentDate = currentDate.AddDays(1); // Zwiększ datę o 1 dzień dla kolejnych wiadomości
        }
    }
    public class MessageWithDate : IMessage
    {
        private readonly IMessage originalMessage;
        public DateTime SentDate { get; }

        public int Id
        {
            get => originalMessage.Id;
            set => originalMessage.Id = value; // Setter przekazuje wartość
        }

        public string Title
        {
            get => originalMessage.Title;
            set => originalMessage.Title = value; // Setter przekazuje wartość
        }

        public string Content
        {
            get => $"{originalMessage.Content}\nData wysłania: {SentDate:dd.MM.yyyy}";
            set => originalMessage.Content = value; // Setter przekazuje wartość
        }

        public MessageWithDate(IMessage originalMessage, DateTime sentDate)
        {
            this.originalMessage = originalMessage;
            SentDate = sentDate;
        }
    }
    public class MessageDateWrapperDecorator : MessageBoxDecorator
    {
        private DateTime currentDate;

        public MessageDateWrapperDecorator(IMessageBox innerMessageBox, DateTime startDate)
            : base(innerMessageBox)
        {
            currentDate = startDate;
        }

        public override void AddMessage(IMessage message)
        {
            // Opakowanie wiadomości w dekorator z datą
            var decoratedMessage = new MessageWithDate(message, currentDate);
            base.AddMessage(decoratedMessage);
            currentDate = currentDate.AddDays(1); // Zwiększ datę o 1 dzień
        }
    }
    public class MessageWithReadFlag : IMessage
    {
        private readonly IMessage originalMessage;
        private bool isRead;

        public int Id
        {
            get => originalMessage.Id;
            set => originalMessage.Id = value;
        }

        public string Title
        {
            get => isRead ? $"{originalMessage.Title} [Odczytana]" : $"{originalMessage.Title} [Nowa]";
            set => originalMessage.Title = value;
        }

        public string Content
        {
            get => originalMessage.Content;
            set => originalMessage.Content = value;
        }

        public MessageWithReadFlag(IMessage originalMessage)
        {
            this.originalMessage = originalMessage;
            isRead = false; // Domyślnie wiadomość jest "Nowa"
        }

        public void MarkAsRead()
        {
            isRead = true; // Oznacz wiadomość jako "Odczytana"
        }
    }



}