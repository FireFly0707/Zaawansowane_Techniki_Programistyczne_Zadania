using System;
using System.Collections.Generic;
using System.Linq;

using System;
using System.Collections.Generic;
using System.Linq;

// Wspólny interfejs dla zadań i grup zadań
public interface ITaskComponent
{
    string Name { get; }
    bool IsCompleted { get; }
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    void MarkAsCompleted(DateTime completionDate);
    void Display(int indentLevel = 0); // Wyświetlenie hierarchii zadań z wcięciem
    int GetCompletedCount(bool late = false);
    int GetPendingCount(bool late = false);
}

// Klasa Task implementująca interfejs ITaskComponent
public class Task : ITaskComponent
{
    public string Name { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public bool IsCompleted { get; private set; } = false;
    public bool IsLate { get; private set; } = false;

    public Task(string name, DateTime startDate, DateTime endDate)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void MarkAsCompleted(DateTime completionDate)
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            IsLate = completionDate > EndDate;
        }
    }

    public void Display(int indentLevel = 0)
    {
        Console.WriteLine(new string(' ', indentLevel * 2) + ToString());
    }

    public int GetCompletedCount(bool late = false) => IsCompleted && IsLate == late ? 1 : 0;

    public int GetPendingCount(bool late = false) => !IsCompleted && (late == (DateTime.Now > EndDate)) ? 1 : 0;

    public override string ToString()
    {
        return $"{Name} ({StartDate:dd.MM.yyyy} to {EndDate:dd.MM.yyyy}) - Status: {GetStatus()}";
    }

    private string GetStatus()
    {
        if (IsCompleted)
            return IsLate ? "Completed Late" : "Completed";
        return "Pending";
    }
}

// Klasa TaskGroup implementująca interfejs ITaskComponent
public class TaskGroup : ITaskComponent
{
    public string Name { get; }
    private readonly List<ITaskComponent> _components = new List<ITaskComponent>();

    public TaskGroup(string name)
    {
        Name = name;
    }

    public DateTime StartDate => _components.Min(c => c.StartDate);
    public DateTime EndDate => _components.Max(c => c.EndDate);
    public bool IsCompleted => _components.All(c => c.IsCompleted);

    public void AddComponent(ITaskComponent component)
    {
        _components.Add(component);
    }

    public void RemoveComponent(ITaskComponent component)
    {
        _components.Remove(component);
    }

    public void MarkAsCompleted(DateTime completionDate)
    {
        foreach (var component in _components)
        {
            component.MarkAsCompleted(completionDate);
        }
    }

    public void Display(int indentLevel = 0)
    {
        Console.WriteLine(new string(' ', indentLevel * 2) + $"{Name} (Group)");
        foreach (var component in _components)
        {
            component.Display(indentLevel + 1);
        }
    }

    public int GetCompletedCount(bool late = false)
    {
        return _components.Sum(c => c.GetCompletedCount(late));
    }

    public int GetPendingCount(bool late = false)
    {
        return _components.Sum(c => c.GetPendingCount(late));
    }
}


public class Program
{
    public static void Main()
    {
        // Zadania
        var task1 = new Task("1A - Implementacja algorytmu", new DateTime(2024, 10, 21), new DateTime(2024, 10, 27));
        var task2 = new Task("1B - Analiza złożoności", new DateTime(2024, 10, 24), new DateTime(2024, 10, 31));
        var task3 = new Task("2A - Projektowanie schematu bazy", new DateTime(2024, 10, 28), new DateTime(2024, 11, 3));
        var task4 = new Task("2B - Tworzenie zapytań SQL", new DateTime(2024, 11, 1), new DateTime(2024, 11, 30));

        // Oznaczanie wykonanych zadań
        task1.MarkAsCompleted(new DateTime(2024, 10, 25)); // Wykonane na czas
        task2.MarkAsCompleted(new DateTime(2024, 11, 1)); // Wykonane z opóźnieniem

        // Grupy zadań
        var group1 = new TaskGroup("Grupa 1");
        group1.AddComponent(task1);
        group1.AddComponent(task2);

        var group2 = new TaskGroup("Grupa 2");
        group2.AddComponent(task3);
        group2.AddComponent(task4);

        var mainGroup = new TaskGroup("Projekt");
        mainGroup.AddComponent(group1);
        mainGroup.AddComponent(group2);

        // Wyświetlanie hierarchii
        Console.WriteLine("Hierarchia zadań:");
        mainGroup.Display();

        // Podsumowanie
        Console.WriteLine("\nPodsumowanie:");
        Console.WriteLine($"Wykonane na czas: {mainGroup.GetCompletedCount(false)}");
        Console.WriteLine($"Wykonane z opóźnieniem: {mainGroup.GetCompletedCount(true)}");
        Console.WriteLine($"Oczekujące: {mainGroup.GetPendingCount(false)}");
        Console.WriteLine($"Oczekujące z opóźnieniem: {mainGroup.GetPendingCount(true)}");
    }
}