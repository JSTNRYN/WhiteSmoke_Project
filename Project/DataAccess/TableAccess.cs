using Dapper;

public static class TableAccess
{
    private const string Table = "\"Table\"";

    private static Microsoft.Data.Sqlite.SqliteConnection Open()
    {
        var connection = Database.CreateConnection();
        connection.Open();
        return connection;
    }

    public static List<TableModel> GetAll()
    {
        using var connection = Open();
        return connection.Query<TableModel>($"SELECT * FROM {Table}").ToList();
    }

    public static TableModel? GetById(int id)
    {
        using var connection = Open();
        return connection.QueryFirstOrDefault<TableModel>($"SELECT * FROM {Table} WHERE ID = @Id", new { Id = id });
    }

    public static List<TableModel> GetWithMinimumCapacity(int minimumCapacity)
    {
        using var connection = Open();
        return connection.Query<TableModel>($"SELECT * FROM {Table} WHERE Capacity >= @MinimumCapacity ORDER BY Capacity", new { MinimumCapacity = minimumCapacity }).ToList();
    }

    public static void Insert(int capacity, int x, int y)
    {
        using var connection = Open();
        connection.Execute($"INSERT INTO {Table} (Capacity, X, Y, IsOccupied) VALUES (@Capacity, @X, @Y, 0)", new { Capacity = capacity, X = x, Y = y });
    }

    public static void UpdateLocation(int id, int x, int y)
    {
        using var connection = Open();
        connection.Execute($"UPDATE {Table} SET X = @X, Y = @Y WHERE ID = @Id", new { Id = id, X = x, Y = y });
    }

    public static void Delete(int id)
    {
        using var connection = Open();
        connection.Execute($"DELETE FROM {Table} WHERE ID = @Id", new { Id = id });
    }
}
