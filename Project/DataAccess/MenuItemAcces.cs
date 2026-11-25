using Microsoft.Data.Sqlite;
using Dapper;

public static class MenuItemAccess
{
    private static readonly string _table = "MenuItem";

    public static List<MenuItemModel> GetAll()
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<MenuItemModel>(@"
            SELECT ID AS Id, MENU AS Menu, NAME AS Name, CATEGORY AS Category,
                   CAST(UNITPRICE AS REAL) AS UnitPrice, DESCRIPTION AS Description, ADULT AS Adult, IsViewable
            FROM MenuItem
            ORDER BY CATEGORY, ID").ToList();
    }

    public static int Update(MenuItemModel item)
    {
        using var connection = Database.CreateConnection();
        string query = $"UPDATE {_table} SET MENU=@Menu, NAME=@Name, CATEGORY=@Category, UNITPRICE=@UnitPrice, DESCRIPTION=@Description, ADULT=@Adult, IsViewable=@IsViewable WHERE ID=@Id";
        
        return connection.Execute(query, item);
    }

    public static MenuItemModel? FindById(int id)
    {
        using var connection = Database.CreateConnection();
        string sql = $"SELECT * FROM {_table} WHERE ID = @Id";

        return connection.QueryFirstOrDefault<MenuItemModel>(sql, new { Id = id });
    }
}
