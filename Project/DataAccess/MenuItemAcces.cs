using Dapper;
public static class MenuItemAccess
{
    public static List<MenuItemModel> LoadAllMenuItems()
    {
        try
        {
            using (var conn = Database.CreateConnection())
            {
                conn.Open();
                string query = @"
                    SELECT
                        ID AS Id,
                        MENU AS Menu,
                        NAME AS Name,
                        CATEGORY AS Category,
                        CAST(UNITPRICE AS REAL) AS UnitPrice,
                        DESCRIPTION AS Description
                    FROM MenuItem
                    ORDER BY CATEGORY, NAME";

                List<MenuItemModel> items = conn.Query<MenuItemModel>(query).ToList();
                return items;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database fout: {ex.Message}");
            return new List<MenuItemModel>();
        }
    }
}
