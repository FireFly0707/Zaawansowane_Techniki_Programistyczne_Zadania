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
class DrawingBuilder {
    private List<Figure> figures = new List<Figure>();
   public DrawingBuilder MoveTo(int x, int y)
    {
        figures.Add(new Figure(new List<Point> { new Point(x, y) }));
        return this;
    }
    public DrawingBuilder LineTo(int x, int y)
    {
        figures[figures.Count - 1].Points.Add(new Point(x, y));
        return this;
    }
    public DrawingBuilder Close()
    {
        figures[figures.Count - 1].Points.Add(figures[figures.Count - 1].Points[0]);
        return this;
    }
    public Drawing Build()
    {
        return new Drawing(figures);
    }

}

class Program
{
    static void Main(string[] args)
    {
        // Tworzenie punktów dla pierwszej figury
        // Używamy Buildera, aby stworzyć rysunek z dwóch figur
        var drawingBuilder = new DrawingBuilder();

        // Pierwsza figura
        drawingBuilder
            .MoveTo(100, 400)  // Początek figury
            .LineTo(200, 50)   // Dodajemy kolejne punkty
            .LineTo(450, 300)
            .LineTo(250, 250)
            .Close();          // Zamykamy figurę

        // Druga figura
        drawingBuilder
            .MoveTo(300, 350)  // Początek figury
            .LineTo(350, 100)
            .LineTo(50, 200);  // Figura może być otwarta

        // Odbieramy gotowy rysunek
        Drawing drawing = drawingBuilder.Build();

        // Wypisujemy rysunek do konsoli
        Console.WriteLine(drawing);
        // Wypisanie rysunku do konsoli
    }
}
