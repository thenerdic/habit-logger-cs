using System;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        string DatabasePath = "habit_log.db";
        string? userInput;

        if (!File.Exists(DatabasePath))
        {
            Console.WriteLine("Database not found. Creating...");
            CreateDatabase(DatabasePath);
        }

        MainMenu();
        userInput = Console.ReadLine();

        switch (userInput)
        {
            case "0":
                break;
            case "1":
                // view records
                Console.Clear();
                Console.WriteLine("Viewing records...");
                ViewRecords(DatabasePath);
                break;
            case "2":
                // insert records
                Console.WriteLine("Inserting records...");
                InsertRecords(DatabasePath);
                break;
            case "3":
                // delete records
                Console.WriteLine("Deleting records...");
                DeleteRecords(DatabasePath);
                break;
            case "4":
                // update records
                Console.WriteLine("Updating records...");
                break;
            default:
                break;
        }
    }

    static void CreateDatabase(string DatabasePath)
    {
        SQLiteConnection.CreateFile(DatabasePath);

        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3"))
        {
            connection.Open();

            string CreateTableQuery = @"
                CREATE TABLE IF NOT EXISTS Habit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitName TEXT NOT NULL,
                    Quantity INTEGER NOT NULL
                );
                ";
            using (SQLiteCommand command = new SQLiteCommand(CreateTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        Console.WriteLine($"Database created at: {Path.GetFullPath(DatabasePath)}");
    }


    static void ViewRecords(string DatabasePath)
    {
        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3"))
        {
            connection.Open();

            // select all records from habits table
            string SelectQuery = "SELECT * FROM Habit";
            using (SQLiteCommand command = new SQLiteCommand(SelectQuery, connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Habit Table:");
                Console.WriteLine("ID\tHabitName\tQuantity");

                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}\t{reader["HabitName"]}\t\t{reader["Quantity"]}");
                }
                Console.WriteLine("---------------------------------");
            }
            connection.Close();
        }

    }

    static void InsertRecords(string DatabasePath)
    {
        string? NewHabit;
        int HabitQuantity;

        Console.Write("Add habit: ");
        NewHabit = Console.ReadLine();
        Console.Write("Quantity: ");
        HabitQuantity = Convert.ToInt32(Console.ReadLine());

        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3"))
        {
            connection.Open();
            string InsertQuery = "INSERT INTO Habit (HabitName, Quantity) VALUES (@habitname, @quantity)"; // query to insert into table
            using (SQLiteCommand command = new SQLiteCommand(InsertQuery, connection))
            {
                // insert record into table 
                command.Parameters.AddWithValue("@habitname", NewHabit);
                command.Parameters.AddWithValue("@quantity", HabitQuantity);

                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    static void DeleteRecords(string DatabasePath)
    {
        int HabitID;
        Console.Write("Enter ID of table you wish to delete: ");
        HabitID = Convert.ToInt32(Console.ReadLine());

        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3"))
        {
            connection.Open();
            string DeleteQuery = "DELETE FROM Habit WHERE Id  = @id";
            using (SQLiteCommand command = new SQLiteCommand(DeleteQuery, connection))
            {
                command.Parameters.AddWithValue("@id", HabitID);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    static void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("MAIN MENU\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("Type 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
    }
}