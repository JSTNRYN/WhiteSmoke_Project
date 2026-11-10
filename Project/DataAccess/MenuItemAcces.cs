using Dapper;

public static class MenuItemAccess
{
    public static List<MenuItemModel> GetAll()
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<MenuItemModel>(@"
            SELECT ID AS Id, MENU AS Menu, NAME AS Name, CATEGORY AS Category,
                   UNITPRICE AS UnitPrice, DESCRIPTION AS Description, ADULT AS Adult
            FROM MenuItem
            ORDER BY CATEGORY, ID").ToList();
    }

    public static MenuItemModel? GetById(int id)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<MenuItemModel>(@"
            SELECT ID AS Id, MENU AS Menu, NAME AS Name, CATEGORY AS Category,
                   UNITPRICE AS UnitPrice, DESCRIPTION AS Description, ADULT AS Adult
            FROM MenuItem WHERE ID = @Id", new { Id = id });
    }

    public static void Update(MenuItemModel item)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(@"
            UPDATE MenuItem
            SET NAME = @Name, UNITPRICE = @UnitPrice, DESCRIPTION = @Description
            WHERE ID = @Id", item);
    }
}
