using Microsoft.Data.Sqlite;
using Dapper;

public class TableAccess
{
    private static string _table = "\"Table\"";


    public static TableModel? GetById(int id)
    {
        string query = $"SELECT * FROM {_table} WHERE id = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<TableModel>(query, new { Id = id });
    }

    public static List<TableModel> GetWithMinimumCapacity(int capacity)
    {
        string query = $"SELECT * FROM {_table} WHERE capacity >= @Capacity";
        using var connection = Database.CreateConnection();
        connection.Open();
        List<TableModel> availableTables = connection.Query<TableModel>(query, new { Capacity = capacity }).ToList();
        return availableTables;
    }
}
