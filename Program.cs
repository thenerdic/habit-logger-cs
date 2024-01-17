using System;
using System.Data.SQLite;

class Program {
    static void Main()
    {
       string databasePath = "habit_log.db";

       if (!File.Exists(databasePath))
       {
        CreateDatabase(databasePath);
       }

       Console.WriteLine("Habit logger CLI started");
       Console.ReadLine();
    }

    static void CreateDatabase(string databasePath)
    {
        SQLiteConnection.CreateFile(databasePath);

        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3"))
        {
            connection.Open();

            string CreateTableQuery = @"
                CREATE TABLE IF NOT EXISTS Habits (
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
}