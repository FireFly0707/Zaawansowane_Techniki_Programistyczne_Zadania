using System;
using System.Linq;

interface IDatabaseConnection
{
    int AddRecord(string name, int age);

    void UpdateRecord(int id, string newName, int newAge);

    void DeleteRecord(int id);

    Record? GetRecord(int id);

    void ShowAllRecords();
}
interface IConnectionManager
{

    IDatabaseConnection GetConnection(string databaseName);

}
class ConnectionManager : IConnectionManager
{
    private static ConnectionManager instance;
    private const int MAX_CONNECTIONS = 3;
    private readonly Dictionary<string, List<IDatabaseConnection>> connectionPool = new();
    private readonly Dictionary<string, int> connectionIndex = new();

    private ConnectionManager()
    {
    }

    public static ConnectionManager GetInstance()
    {
        if (instance == null)
        {
            instance = new ConnectionManager();
        }
        return instance;
    }
    public IDatabaseConnection GetConnection(string databaseName)
    {
        var db = Database.GetInstance(databaseName);

        if (!connectionPool.ContainsKey(databaseName))
        {
            connectionPool[databaseName] = new List<IDatabaseConnection>();
            connectionIndex[databaseName] = 0;
        }

        var connections = connectionPool[databaseName];
        var index = connectionIndex[databaseName];

        // Jeśli liczba połączeń jest mniejsza niż limit, tworzymy nowe połączenie
        if (connections.Count < MAX_CONNECTIONS)
        {
            
            var connection = db.GetConnection();
            connections.Add(connection);
            return connection;
        }
        else
        {
            // Jeśli przekroczono limit, zwracamy istniejące połączenie
            connectionIndex[databaseName]++;
            return connections[connectionIndex[databaseName]-1 % MAX_CONNECTIONS];
            
        }
    }
}

// Prosty rekord w bazie danych
class Record
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public Record(int id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return $"Record [ID={Id}, Name={Name}, Age={Age}]";
    }
}

// Prosta baza danych
class Database
{
    private static readonly Dictionary<string, Database> instances = new();
    private readonly List<Record> records; // Lista przechowująca rekordy
    private int nextId = 1; // Licznik do generowania unikalnych ID

    private Database()
    {
        records = new();
    }

   
    // Multiton - jedna baza danych na nazwę
    public static Database GetInstance(string name)
    {
        if (!instances.ContainsKey(name))
        {
            instances[name] = new Database();
        }
        return instances[name];
    }
    // Zwraca implementację interfejsu DatabaseConnection
    public IDatabaseConnection GetConnection()
    {
        return new DatabaseConnection(this);
    }

    // Prywatna klasa wewnętrzna implementująca interfejs DatabaseConnection
    // W Javie korzystamy z cech klas wewnętrznych, w C# ta klasa musiałaby posiadać
    // referencję na obiekt klasy Database
    private class DatabaseConnection : IDatabaseConnection
    {
        private readonly Database db;

        public DatabaseConnection(Database database)
        {
            db = database;
        }

        // Dodawanie nowego rekordu
        public int AddRecord(string name, int age)
        {
            Record newRecord = new(db.nextId++, name, age);
            db.records.Add(newRecord);
            Console.WriteLine($"Inserted: {newRecord}");
            return db.nextId - 1; // zwracamy id dodanego rekordu
        }

        // Pobieranie rekordu po ID
        public Record? GetRecord(int id)
        {
            return db.records.Where(rec => rec.Id == id).FirstOrDefault();
        }

        // Aktualizowanie rekordu po ID
        public void UpdateRecord(int id, string newName, int newAge)
        {
            Record? optionalRecord = GetRecord(id);

            if (optionalRecord != null)
            {
                Record record = optionalRecord;
                record.Name = newName;
                record.Age = newAge;
                Console.WriteLine($"Updated: {record}");
            }
            else
            {
                Console.WriteLine($"Record with ID {id} not found.");
            }
        }

        // Usuwanie rekordu po ID
        public void DeleteRecord(int id)
        {
            Record? optionalRecord = GetRecord(id);

            if (optionalRecord != null)
            {
                db.records.Remove(optionalRecord);
                Console.WriteLine($"Deleted record with ID {id}");
            }
            else
            {
                Console.WriteLine($"Record with ID {id} not found.");
            }
        }

        // Wyświetlanie wszystkich rekordów
        public void ShowAllRecords()
        {
            if (db.records.Any())
            {
                Console.WriteLine("All records:");
                db.records.ForEach(r => Console.WriteLine(r));
            }
            else
            {
                Console.WriteLine("No records in the database.");
            }
        }
    }

}


public class Ztp01
{
    public static void Main(string[] args)
    {
        
        ConnectionManager connectionManager = ConnectionManager.GetInstance();

       
        IDatabaseConnection connection1 = connectionManager.GetConnection("DB1");
        connection1.AddRecord("Karol", 23);
        connection1.ShowAllRecords();

        
        IDatabaseConnection connection2 = connectionManager.GetConnection("DB1");
        connection2.AddRecord("Anna", 30);
        connection2.ShowAllRecords();

        
        IDatabaseConnection connection3 = connectionManager.GetConnection("DB1");
        connection3.AddRecord("Piotr", 25);
        connection3.ShowAllRecords();

       
        IDatabaseConnection connection4 = connectionManager.GetConnection("DB1");
        connection4.AddRecord("Marek", 35);
        connection4.ShowAllRecords();

        if (ReferenceEquals(connection1, connection4))
        {
            Console.WriteLine("connection1 and connection4 are the same.");
        }
        else
        {
            Console.WriteLine("connection1 and connection4 are different.");
        }
    }
}
