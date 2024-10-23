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
class Director
{
    private DrawingBuilder builder;
    public Director(DrawingBuilder builder)
    {
        this.builder = builder;
    }
    public void ConstructFromString(string input)
    {
        List<string> instructions = input.Split(' ').ToList();
        for (int i = 0; i < instructions.Count; i++)
        {
            if (instructions[i] == "M")
            {
                builder.MoveTo(int.Parse(instructions[i + 1]), int.Parse(instructions[i + 2]));
                i += 2;
            }
            else if (instructions[i] == "L")
            {
                builder.LineTo(int.Parse(instructions[i + 1]), int.Parse(instructions[i + 2]));
                i += 2;
            }
            else if (instructions[i] == "Z")
            {
                builder.Close();
            }
            else
            {
                throw new Exception("Invalid instruction");
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var drawingBuilder = new DrawingBuilder();

            // Przykład użycia z Directorem
            Director director = new Director(drawingBuilder);
            director.ConstructFromString("M 100 400 L 200 50 L 450 300 L 250 250 Z M 300 350 L 350 100 L 50 200");

            // Odbieramy gotowy rysunek
            Drawing drawing = drawingBuilder.Build();

            // Wypisujemy rysunek do konsoli
            Console.WriteLine(drawing);
        }
    }
}
