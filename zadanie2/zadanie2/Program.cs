using System;
using System.Collections.Generic;
using System.Text;

// Klasa reprezentująca komórkę tabeli
abstract class Cell
{
    public abstract Cell Clone(); // Metoda klonująca
    public abstract void SetValue(object value); // Metoda do ustawiania wartości
    public abstract override string ToString();
}
class TextCell : Cell
{
    private string value;

    public TextCell(string value = "")
    {
        this.value = value;
    }

    public override Cell Clone()
    {
        return new TextCell(this.value); // Klonowanie obiektu
    }

    public override void SetValue(object value)
    {
        this.value = value.ToString();
    }

    public override string ToString()
    {
        return value.PadRight(15);
    }
}

// Konkretna klasa dla liczb
class NumberCell : Cell
{
    private int value;

    public NumberCell(int value = 0)
    {
        this.value = value;
    }

    public override Cell Clone()
    {
        return new NumberCell(this.value);
    }

    public override void SetValue(object value)
    {
        this.value = Convert.ToInt32(value);
    }

    public override string ToString()
    {
        return value.ToString().PadRight(15);
    }
}

// Konkretna klasa dla wartości logicznych
class BooleanCell : Cell
{
    private bool value;

    public BooleanCell(bool value = false)
    {
        this.value = value;
    }

    public override Cell Clone()
    {
        return new BooleanCell(this.value);
    }

    public override void SetValue(object value)
    {
        this.value = Convert.ToBoolean(value);
    }

    public override string ToString()
    {
        return value.ToString().PadRight(15);
    }
}
// Klasa reprezentująca nagłówek kolumny w tabeli
class Header
{
    public string Name { get; }
    private readonly Cell cellPrototype; // Prototyp komórki

    public Header(string name, Cell cellPrototype)
    {
        Name = name;
        this.cellPrototype = cellPrototype;
    }

    public Cell CreateCell(object value)
    {
        var newCell = cellPrototype.Clone(); // Klonowanie prototypu
        newCell.SetValue(value); // Ustawienie wartości w nowo sklonowanej komórce
        return newCell;
    }

    public Cell CreateDefaultCell()
    {
        return cellPrototype.Clone(); // Tworzenie komórki domyślnej (pustej)
    }
}
// Klasa reprezentująca tabelę
class Table
{
    private readonly List<Header> headers;  // Lista nagłówków kolumn
    private readonly List<List<Cell>> rows; // Lista wierszy (każdy wiersz to lista komórek)

    public Table()
    {
        headers = new List<Header>();
        rows = new List<List<Cell>>();
    }

    public void AddColumn(Header header)
    {
        headers.Add(header);

        // Dodajemy puste komórki do każdego z istniejących wierszy
        foreach (var row in rows)
        {
            row.Add(header.CreateDefaultCell());
        }
    }

    public void AddRow(params object[] cellValues)
    {
        if (cellValues.Length != headers.Count)
        {
            throw new ArgumentException("Liczba wartości nie zgadza się z liczbą kolumn.");
        }

        // Dodajemy wiersz wypełniony komórkami z wartością
        var newRow = new List<Cell>();
        for (int i = 0; i < cellValues.Length; i++)
        {
            newRow.Add(headers[i].CreateCell(cellValues[i]));
        }

        rows.Add(newRow);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        // Dodajemy nagłówki
        foreach (var header in headers)
        {
            sb.Append(header.Name.PadRight(15));
        }
        sb.AppendLine();

        // Dodajemy separator
        sb.AppendLine(new string('-', headers.Count * 15));

        // Dodajemy wiersze
        foreach (var row in rows)
        {
            foreach (var cell in row)
            {
                sb.Append(cell.ToString());
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Tworzymy nową tabelę
        Table table = new Table();

        // Dodajemy kolumny
        table.AddColumn(new Header("Name",new TextCell()));
        table.AddColumn(new Header("Age",new NumberCell()));
        table.AddColumn(new Header("Is Student", new BooleanCell()));

        // Dodajemy wiersze
        table.AddRow("Alice", 30, false);
        table.AddRow("Bob", 30, true);
        table.AddRow("Charlie", 35, false);

        // Wyświetlamy tabelę
        Console.WriteLine(table.ToString());

        Console.ReadKey();
    }
}
