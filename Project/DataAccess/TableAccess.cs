using Microsoft.Data.Sqlite;
using Dapper;

public class TableAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string _table = "Table";


    public static TableModel? GetById(int id)
    {
        string query = $"SELECT * FROM {_table} WHERE id = @Id";
        return _connection.QueryFirst<TableModel>(query, new { Id = id });
    }

    public static List<TableModel> GetWithMinimumCapacity(int capacity)
    {
        string query = $"SELECT * FROM {_table} WHERE capacity >= @capacity";
        List<TableModel> availableTables = _connection.Query<TableModel>(query, capacity).ToList();
        return availableTables;
    }
}