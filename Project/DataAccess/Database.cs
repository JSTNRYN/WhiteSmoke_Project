using Dapper;
using Microsoft.Data.Sqlite;

public static class Database
{
    private static readonly string DatabasePath = ResolveDatabasePath();

    private static string ResolveDatabasePath()
    {
        string? projectDatabase = null;
        DirectoryInfo? directory = Directory.GetParent(AppContext.BaseDirectory);
        while (directory != null)
        {
            string path = Path.Combine(directory.FullName, "DataSources", "project.db");
            if (File.Exists(path)) projectDatabase = path;
            directory = directory.Parent;
        }

        return projectDatabase ?? Path.Combine(AppContext.BaseDirectory, "DataSources", "project.db");
    }

    public static SqliteConnection CreateConnection() => new($"Data Source={DatabasePath}");

    public static void Initialize()
    {
        using var connection = CreateConnection();
        connection.Open();

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS MenuItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                MENU TEXT,
                NAME TEXT NOT NULL,
                CATEGORY TEXT,
                UNITPRICE REAL NOT NULL,
                DESCRIPTION TEXT NOT NULL,
                ADULT INTEGER NOT NULL DEFAULT 0
            );
            CREATE TABLE IF NOT EXISTS ""Order"" (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                ReservationID INTEGER,
                TotalPrice REAL NOT NULL
            );
            CREATE TABLE IF NOT EXISTS OrderItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderID INTEGER NOT NULL,
                MenuItemID INTEGER NOT NULL,
                Quantity INTEGER NOT NULL
            );");

        AddColumnIfMissing(connection, "User", "DATEOFBIRTH", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "MENU", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "NAME", "TEXT NOT NULL DEFAULT ''");
        AddColumnIfMissing(connection, "MenuItem", "CATEGORY", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "UNITPRICE", "REAL NOT NULL DEFAULT 0");
        AddColumnIfMissing(connection, "MenuItem", "DESCRIPTION", "TEXT NOT NULL DEFAULT ''");
        AddColumnIfMissing(connection, "MenuItem", "ADULT", "INTEGER NOT NULL DEFAULT 0");
        EnsureAdmin(connection);

        if (connection.ExecuteScalar<long>("SELECT COUNT(*) FROM MenuItem") == 0)
        {
            string seedPath = Path.Combine(AppContext.BaseDirectory, "DataSources", "menu-items.sql");
            connection.Execute(File.ReadAllText(seedPath));
        }
    }

    private static void EnsureAdmin(SqliteConnection connection)
    {
        const string email = "admin@admin.com";
        string password = BCrypt.Net.BCrypt.HashPassword(email);
        connection.Execute(@"
            INSERT INTO User (email, password, firstName, lastName, phonenumber, role, DateOfBirth)
            SELECT @Email, @Password, 'Test', 'Admin', '0000000000', 'Admin', '2000-01-01'
            WHERE NOT EXISTS (SELECT 1 FROM User WHERE email = @Email)", new { Email = email, Password = password });
    }

    private static void AddColumnIfMissing(SqliteConnection connection, string table, string column, string type)
    {
        bool exists = connection.Query<(int Id, string Name, string Type, int NotNull, string DefaultValue, int PrimaryKey)>($"PRAGMA table_info(\"{table}\")")
            .Any(item => item.Name.Equals(column, StringComparison.OrdinalIgnoreCase));
        if (!exists) connection.Execute($"ALTER TABLE \"{table}\" ADD COLUMN \"{column}\" {type}");
    }
}
