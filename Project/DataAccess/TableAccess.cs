using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Dapper;

// Deze klasse praat met de database. Hier staat alleen SQL, geen regels of controles.
// Static, dus je gebruikt hem als TableAccess.GetAll() zonder "new".
public static class TableAccess
{
    // Pad naar de SQLite-database
    private const string ConnectionString = "Data Source=DataSources/project.db";

    // "Table" is een gereserveerd woord in SQL, daarom staat het tussen aanhalingstekens
    private const string Table = "\"Table\"";

    // Maakt bij elke aanroep een nieuwe verbinding. Met "using" wordt die daarna netjes gesloten.
    private static SqliteConnection Open() => new SqliteConnection(ConnectionString);

    // Haalt alle tafels op (wordt gebruikt om de plattegrond te tekenen)
    public static List<TableModel> GetAll()
    {
        using var conn = Open();
        return conn.Query<TableModel>($"SELECT * FROM {Table}").ToList();
    }

    // Haalt één tafel op via het nummer. Geeft null terug als hij niet bestaat.
    public static TableModel? GetById(int id)
    {
        using var conn = Open();
        return conn.QueryFirstOrDefault<TableModel>(
            $"SELECT * FROM {Table} WHERE ID = @Id", new { Id = id });
    }

    // Nodig voor ReservationLogic: alle tafels met minimaal deze capaciteit, kleinste eerst
    public static List<TableModel> GetWithMinimumCapacity(int minimumCapacity)
    {
        using var conn = Open();
        return conn.Query<TableModel>(
            $"SELECT * FROM {Table} WHERE Capacity >= @MinimumCapacity ORDER BY Capacity",
            new { MinimumCapacity = minimumCapacity }).ToList();
    }

    // Voegt een nieuwe tafel toe. Een nieuwe tafel is altijd vrij (IsOccupied = 0).
    // De parameters (@Capacity enz.) voorkomen SQL-injectie.
    public static void Insert(int capacity, int x, int y)
    {
        using var conn = Open();
        conn.Execute(
            $"INSERT INTO {Table} (Capacity, X, Y, IsOccupied) VALUES (@Capacity, @X, @Y, 0)",
            new { Capacity = capacity, X = x, Y = y });
    }

    // Zet een bestaande tafel op een andere plek in het raster
    public static void UpdateLocation(int id, int x, int y)
    {
        using var conn = Open();
        conn.Execute(
            $"UPDATE {Table} SET X = @X, Y = @Y WHERE ID = @Id",
            new { Id = id, X = x, Y = y });
    }

    // Verwijdert een tafel definitief uit de database.
    // Geeft true terug als de tafel is verwijderd, false als dat niet mag omdat
    // er nog iets naar de tafel verwijst (bijvoorbeeld een reservering).
    public static bool Delete(int id)
    {
        try
        {
            using var conn = Open();
            conn.Execute($"DELETE FROM {Table} WHERE ID = @Id", new { Id = id });
            return true;
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // FOREIGN KEY constraint failed
        {
            return false;
        }
    }
}