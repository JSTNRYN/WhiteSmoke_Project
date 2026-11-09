using Dapper;

public static class MenuItemAccess
{
    public static List<MenuItemModel> LoadAllMenuItems(bool includeUnavailable = false)
    {
        try
        {
            using var conn = Database.CreateConnection();
            conn.Open();

            const string query = @"
                SELECT
                    ID AS Id,
                    MENU AS Menu,
                    NAME AS Name,
                    CATEGORY AS Category,
                    CAST(UNITPRICE AS REAL) AS UnitPrice,
                    DESCRIPTION AS Description,
                    CAST(ADULT AS INTEGER) AS Adult,
                    COALESCE(ISAVAILABLE, 1) AS IsAvailable
                FROM MenuItem
                WHERE @IncludeUnavailable = 1 OR COALESCE(ISAVAILABLE, 1) = 1
                ORDER BY CATEGORY, NAME";

            List<MenuItemModel> items = conn.Query<MenuItemModel>(query, new { IncludeUnavailable = includeUnavailable ? 1 : 0 }).ToList();
            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
            return new List<MenuItemModel>();
        }
    }

    public static MenuItemModel? GetById(int id)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<MenuItemModel>("SELECT ID AS Id, MENU AS Menu, NAME AS Name, CATEGORY AS Category, UNITPRICE AS UnitPrice, DESCRIPTION AS Description, ADULT AS Adult, COALESCE(ISAVAILABLE, 1) AS IsAvailable FROM MenuItem WHERE ID = @Id", new { Id = id });
    }

    public static bool NameExists(string name, string category, int? exceptId = null)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<long>("SELECT COUNT(*) FROM MenuItem WHERE LOWER(NAME) = LOWER(@Name) AND LOWER(CATEGORY) = LOWER(@Category) AND (@ExceptId IS NULL OR ID <> @ExceptId)", new { Name = name, Category = category, ExceptId = exceptId }) > 0;
    }

    public static int Create(MenuItemModel item)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<int>("INSERT INTO MenuItem (MENU, NAME, CATEGORY, UNITPRICE, DESCRIPTION, ADULT, ISAVAILABLE) VALUES (@Menu, @Name, @Category, @UnitPrice, @Description, @Adult, 1) RETURNING ID", item);
    }

    public static void Update(MenuItemModel item)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute("UPDATE MenuItem SET MENU = @Menu, NAME = @Name, CATEGORY = @Category, UNITPRICE = @UnitPrice, DESCRIPTION = @Description, ADULT = @Adult WHERE ID = @Id", item);
    }

    public static void Archive(int id)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute("UPDATE MenuItem SET ISAVAILABLE = 0 WHERE ID = @Id", new { Id = id });
    }

}
