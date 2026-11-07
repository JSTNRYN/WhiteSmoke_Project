using Microsoft.Data.Sqlite;

public static class Database
{
    private static readonly string DatabasePath = Path.Combine(
        AppContext.BaseDirectory,
        "DataSources",
        "project.db");

    private static readonly string MenuDatabasePath = Path.Combine(
        AppContext.BaseDirectory,
        "DataSources",
        "template.project.db");

    public static SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={DatabasePath}");
    }

    public static SqliteConnection CreateMenuConnection()
    {
        return new SqliteConnection($"Data Source={MenuDatabasePath}");
    }
}
