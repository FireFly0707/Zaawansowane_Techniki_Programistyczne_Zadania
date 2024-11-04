using System;
using System.Collections.Generic;
using System.Linq;

public interface ITableDataSource
{
    int GetRowCount(); // Liczba wierszy w tabeli
    int GetColumnCount(); // Liczba kolumn w tabeli
    string GetColumnName(int columnIndex); // Nazwa kolumny (np. "Name", "Age")
    string GetCellData(int rowIndex, int columnIndex); // Dane w komórce (wiersz, kolumna)
}
public class DictionaryAdapter : ITableDataSource
{
    public Dictionary<string, int> dictionary;
    private List<string> keys;
    public DictionaryAdapter(Dictionary<string, int> dictionary)
    {
        this.dictionary = dictionary;
        this.keys = dictionary.Keys.ToList();
    }
    public string GetCellData(int rowIndex, int columnIndex)
    {
        if (rowIndex < 0 || rowIndex >= keys.Count)
        {
            throw new Exception("Invalid row index");
        }

        string key = keys[rowIndex];

        if (columnIndex == 0)
        {
            return key;
        }
        else if (columnIndex == 1)
        {
            return dictionary[key].ToString();
        }
        else
        {
            throw new Exception("Invalid column index");
        }
    }


    public int GetColumnCount()
    {
        return 2;
    }

    public string GetColumnName(int columnIndex)
    {
       if(columnIndex == 0)
        {
            return "Key";
        }
        else if (columnIndex == 1)
        {
            return "Value";
        }
        else throw new Exception("Invalid column index");
    }

    public int GetRowCount()
    {
        return dictionary.Count;
    }
}
public class ArrayAdapter : ITableDataSource
{
    private int[] array;
    public ArrayAdapter(int[] array)
    {
        this.array = array;
    }
    public string GetCellData(int rowIndex, int columnIndex)
    {
        if(columnIndex == 0)
        {
            return rowIndex.ToString();
        }
        else if (columnIndex == 1)
        {
            return array[rowIndex].ToString();
        }
        else throw new Exception("Invalid column index");
    }

    public int GetColumnCount()
    {
        return 2;
    }

    public string GetColumnName(int columnIndex)
    {
       string response;
        if (columnIndex == 0)
        {
            response = "Index";
        }
        else if (columnIndex == 1)
        {
            response = "Value";
        }
        else throw new Exception("Invalid column index");
        return response;
    }

    public int GetRowCount()
    {
        return array.Length;
    }
}
public class UserListAdapter : ITableDataSource
{
    public List<User> users;
    public UserListAdapter(List<User> users)
    {
        this.users = users;
    }
    public string GetCellData(int rowIndex, int columnIndex)
    {

        if(columnIndex == 0)
        {
            return users[rowIndex].Name;
        }
        else if (columnIndex == 1)
        {
            return users[rowIndex].Age.ToString();
        }
        else if (columnIndex == 2)
        {
            return users[rowIndex].Status;
        }
        else throw new Exception("Invalid column index");
    }

    public int GetColumnCount()
    {
        return 3;
    }

    public string GetColumnName(int columnIndex)
    {
       if(columnIndex == 0 )
        {
            return "Name";
        }
        else if (columnIndex == 1)
        {
            return "Age";
        }
        else if (columnIndex == 2)
        {
            return "Status";
        }
        else throw new Exception("Invalid column index");
    }

    public int GetRowCount()
    {
        return users.Count;
    }
}
public class TableService
{
    public void DisplayTable(ITableDataSource dataSource)
    {
        // Wyświetlanie nagłówków kolumn
        for (int col = 0; col < dataSource.GetColumnCount(); col++)
        {
            Console.Write(dataSource.GetColumnName(col).PadRight(15));
        }
        Console.WriteLine();

        // Linie oddzielające nagłówki od danych
        Console.WriteLine(new string('-', dataSource.GetColumnCount() * 16));


        // Wyświetlanie wierszy danych
        for (int row = 0; row < dataSource.GetRowCount(); row++)
        {
            for (int col = 0; col < dataSource.GetColumnCount(); col++)
            {
                Console.Write(dataSource.GetCellData(row, col).PadRight(15));
            }
            Console.WriteLine();
        }
    }
}

public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Status { get; set; }

    public User(string name, int age, string status)
    {
        Name = name;
        Age = age;
        Status = status;
    }
}

public class Program
{
    public static void Main()
    {
        TableService tableService = new TableService();

        // Test adaptera dla tablicy liczb całkowitych
        int[] numbersArray = { 10, 20, 30, 40, 50 };
        ITableDataSource arrayAdapter = new ArrayAdapter(numbersArray);
        Console.WriteLine("Array Table:");
        tableService.DisplayTable(arrayAdapter);

        // Test adaptera dla słownika
        Dictionary<string, int> dictionary = new Dictionary<string, int>
        {
            { "One", 1 },
            { "Two", 2 },
            { "Three", 3 }
        };
        ITableDataSource dictionaryAdapter = new DictionaryAdapter(dictionary);
        Console.WriteLine("\nDictionary Table:");
        tableService.DisplayTable(dictionaryAdapter);

        // Test adaptera dla listy użytkowników
        List<User> users = new List<User>
        {
            new User("Alice", 25, "Active"),
            new User("Bob", 30, "Inactive"),
            new User("Charlie", 35, "Active")
        };
        ITableDataSource userListAdapter = new UserListAdapter(users);
        Console.WriteLine("\nUser List Table:");
        tableService.DisplayTable(userListAdapter);
    }
}

