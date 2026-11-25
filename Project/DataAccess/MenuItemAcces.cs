using Dapper;

public static class MenuItemAccess
{
    public static List<MenuItemModel> GetAll()
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<MenuItemModel>(@"
            SELECT ID AS Id, MENU AS Menu, NAME AS Name, CATEGORY AS Category,
                   CAST(UNITPRICE AS REAL) AS UnitPrice, DESCRIPTION AS Description, ADULT AS Adult
            FROM MenuItem
            ORDER BY CATEGORY, ID").ToList();
    }

}
