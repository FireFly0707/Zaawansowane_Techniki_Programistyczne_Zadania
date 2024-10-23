using System;
using System.Collections.Generic;

// Klasa reprezentująca punkt
class Point
{
    // Współrzędne punktu
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

// Klasa reprezentująca figurę
class Figure
{
    // Punkty, które tworzą figurę
    public List<Point> Points { get; }

    public Figure(List<Point> points)
    {
        Points = points;
    }

    public override string ToString()
    {
        return $"Figura: {string.Join(", ", Points)}";
    }
}

// Klasa reprezentująca rysunek
class Drawing
{
    // Lista figur tworzących rysunek
    public List<Figure> Figures { get; }

    public Drawing(List<Figure> figures)
    {
        Figures = figures;
    }

    public override string ToString()
    {
        return $"Rysunek: \n{string.Join("\n", Figures)}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Tworzenie punktów dla pierwszej figury
        var punktyFigury1 = new List<Point>
        {
            new Point(100, 400),
            new Point(200, 50),
            new Point(450, 300),
            new Point(250, 250),
            new Point(100, 400)  // Zamknięcie figury - powrót do punktu początkowego
        };

        // Tworzenie punktów dla drugiej figury
        var punktyFigury2 = new List<Point>
        {
            new Point(300, 350),
            new Point(350, 100),
            new Point(50, 200)  // Figura otwarta - punkt początkowy i końcowy się różnią
        };

        // Tworzenie figur na podstawie list punktów
        var figura1 = new Figure(punktyFigury1);
        var figura2 = new Figure(punktyFigury2);

        // Tworzenie rysunku z dwóch figur
        var rysunek = new Drawing(new List<Figure> { figura1, figura2 });

        // Wypisanie rysunku do konsoli
        Console.WriteLine(rysunek);
    }
}
