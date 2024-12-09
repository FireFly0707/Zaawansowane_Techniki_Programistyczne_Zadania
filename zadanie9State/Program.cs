using System;
using System.Collections.Generic;
using zadanie9State;

public class Order
{
    public Dictionary<string, bool> Products { get; } = new(); // Produkty: nazwa -> czy spakowany
    public bool isPaid; // Czy zamówienie jest opłacone
    private IOrderState currentState;

    public Order()
    {
        currentState = new CreatedState(this);
    }

    // Dodaje produkt do zamówienia
    public void AddProduct(string product)
    {
        currentState.AddProduct(product);
    }
    public void SetState(IOrderState state)
    {
        currentState = state;
    }

    // Zatwierdza zamówienie
    public void SubmitOrder()
    {
        currentState.SubmitOrder();
    }

    // Oznacza zamówienie jako opłacone
    public void ConfirmPayment()
    {
       currentState.ConfirmPayment();
    }

    // Oznacza dany produkt jako spakowany
    public void PackProduct(string product)
    {
       currentState.PackProduct(product);
    }

    // Wysyła zamówienie
    public void ShipOrder()
    {
        currentState.ShipOrder();
    }

    // Anuluje zamówienie
    public void CancelOrder()
    {
        currentState.CancelOrder();
    }

    // Wyświetla szczegóły zamówienia
    public void ShowOrderDetails()
    {
        Console.WriteLine($"Zamówienie [{(isPaid ? "Opłacone" : "Nieopłacone")}]:");
        foreach (var product in Products)
        {
            Console.WriteLine($" - {product.Key}: {(product.Value ? "Spakowany" : "Nie spakowany")}");
        }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        Order order = new Order();

        // Dodawanie produktów
        order.AddProduct("Laptop");
        order.AddProduct("Myszka");
        order.ShowOrderDetails();

        // Próba spakowania produktu przed złożeniem zamówienia
        order.PackProduct("Laptop");

        // Próba wysłania zamówienia przed złożeniem zamówienia
        order.ShipOrder();

        // Próba potwierdzenia płatności przed złożeniem zamówienia
        order.ConfirmPayment();

        // Złożenie zamówienia
        order.SubmitOrder();

        // Próba dodania produktu po złożeniu zamówienia
        order.AddProduct("Klawiatura");

        // Potwierdzenie płatności
        order.ConfirmPayment();

        // Próba ponownego potwierdzenia płatności
        order.ConfirmPayment();

        // Spakowanie produktów
        order.PackProduct("Laptop");
        order.PackProduct("Myszka");
        order.ShowOrderDetails();

        // Próba spakowania produktu, który już został spakowany
        order.PackProduct("Laptop");

        // Próba dodania produktu po opłaceniu zamówienia
        order.AddProduct("Monitor");

        // Wysłanie zamówienia
        order.ShipOrder();

        // Próba wysłania zamówienia, które już zostało wysłane
        order.ShipOrder();

        // Próba anulowania zamówienia, które już zostało wysłane
        order.CancelOrder();

        // Wyświetlenie szczegółów zamówienia
        order.ShowOrderDetails();
    }
}
