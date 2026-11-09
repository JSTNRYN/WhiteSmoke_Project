using Microsoft.Data.Sqlite;
using Dapper;

public static class Database
{
    private static readonly string DatabasePath = Path.Combine(
        AppContext.BaseDirectory,
        "DataSources",
        "project.db");

    public static SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={DatabasePath}");
    }

    public static void Initialize()
    {
        using var connection = CreateConnection();
        connection.Open();

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS MenuItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                MENU TEXT,
                NAME TEXT,
                CATEGORY TEXT,
                UNITPRICE DOUBLE,
                DESCRIPTION TEXT,
                ADULT BOOLEAN
            )");

        AddColumnIfMissing(connection, "MenuItem", "MENU", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "CATEGORY", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "DESCRIPTION", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "ADULT", "BOOLEAN");

        long menuItemCount = connection.ExecuteScalar<long>("SELECT COUNT(*) FROM MenuItem");
        if (menuItemCount == 0)
        {
            string seedPath = Path.Combine(AppContext.BaseDirectory, "DataSources", "menu-items.sql");
            string seed = File.ReadAllText(seedPath);
            connection.Execute(seed);
        }
    }

    private static void AddColumnIfMissing(SqliteConnection connection, string table, string column, string type)
    {
        bool columnExists = connection.Query<(int Id, string Name, string Type, int NotNull, string DefaultValue, int PrimaryKey)>($"PRAGMA table_info({table})")
            .Any(item => item.Name.Equals(column, StringComparison.OrdinalIgnoreCase));

        if (!columnExists)
        {
            connection.Execute($"ALTER TABLE {table} ADD COLUMN {column} {type}");
        }
    }
}
