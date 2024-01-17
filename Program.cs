using System;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        string databasePath = "habit_log.db";
        string? userInput;

        if (!File.Exists(databasePath))
        {
            CreateDatabase(databasePath);
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
                ViewRecords(databasePath);
                break;
            case "2":
                // insert records
                Console.WriteLine("Inserting records...");
                break;
            case "3":
                // delete records
                Console.WriteLine("Deleting records...");
                break;
            case "4":
                // update records
                Console.WriteLine("Updating records...");
                break;
            default:
                break;
        }
    }

    static void CreateDatabase(string databasePath)
    {
        SQLiteConnection.CreateFile(databasePath);

        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3"))
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

        Console.WriteLine($"Database created at: {Path.GetFullPath(databasePath)}");
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

    static void ViewRecords(string databasePath)
    {
        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
        {
            connection.Open();

            // select all records from habits table
            string selectQuery = "SELECT * FROM Habit";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Habit Table:");
                Console.WriteLine("ID\tHabitName\tQuantity");

                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}\t{reader["HabitName"]}\t\t{reader["Quantity"]}");
                }
            }

            connection.Close();
        }

    }
}