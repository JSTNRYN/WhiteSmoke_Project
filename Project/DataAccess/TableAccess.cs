using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Dapper;

public static class TableAccess
{
    private const string ConnectionString = "Data Source=DataSources/project.db";
    private const string Table = "\"Table\"";

    private static SqliteConnection Open() => new SqliteConnection(ConnectionString);

    public static List<TableModel> GetAll()
    {
        using var conn = Open();
        return conn.Query<TableModel>($"SELECT * FROM {Table}").ToList();
    }

    public static TableModel? GetById(int id)
    {
        using var conn = Open();
        return conn.QueryFirstOrDefault<TableModel>(
            $"SELECT * FROM {Table} WHERE ID = @Id", new { Id = id });
    }

    // Nodig voor ReservationLogic (alle tafels met minimaal deze capaciteit, kleinste eerst)
    public static List<TableModel> GetWithMinimumCapacity(int minimumCapacity)
    {
        using var conn = Open();
        return conn.Query<TableModel>(
            $"SELECT * FROM {Table} WHERE Capacity >= @MinimumCapacity ORDER BY Capacity",
            new { MinimumCapacity = minimumCapacity }).ToList();
    }

    public static void Insert(int capacity, int x, int y)
    {
        using var conn = Open();
        conn.Execute(
            $"INSERT INTO {Table} (Capacity, X, Y, IsOccupied) VALUES (@Capacity, @X, @Y, 0)",
            new { Capacity = capacity, X = x, Y = y });
    }

    public static void UpdateLocation(int id, int x, int y)
    {
        using var conn = Open();
        conn.Execute(
            $"UPDATE {Table} SET X = @X, Y = @Y WHERE ID = @Id",
            new { Id = id, X = x, Y = y });
    }

    public static void Delete(int id)
    {
        using var conn = Open();
        conn.Execute($"DELETE FROM {Table} WHERE ID = @Id", new { Id = id });
    }
}