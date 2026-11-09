using Dapper;

public static class MenuCategoryAccess
{
    public static List<MenuCategoryModel> GetAll()
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<MenuCategoryModel>("SELECT ID AS Id, NAME AS Name, ISCUSTOM AS IsCustom FROM MenuCategory ORDER BY IsCustom, Name").ToList();
    }

    public static MenuCategoryModel? GetById(int id)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<MenuCategoryModel>("SELECT ID AS Id, NAME AS Name, ISCUSTOM AS IsCustom FROM MenuCategory WHERE ID = @Id", new { Id = id });
    }

    public static int Create(string name)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<int>("INSERT INTO MenuCategory (NAME, ISCUSTOM) VALUES (@Name, 1) RETURNING ID", new { Name = name });
    }

    public static bool NameExists(string name)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<long>("SELECT COUNT(*) FROM MenuCategory WHERE LOWER(NAME) = LOWER(@Name)", new { Name = name }) > 0;
    }

    public static bool HasItems(int id)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<long>("SELECT COUNT(*) FROM MenuItem WHERE LOWER(TRIM(CATEGORY)) = LOWER(TRIM((SELECT NAME FROM MenuCategory WHERE ID = @Id)))", new { Id = id }) > 0;
    }

    public static void Delete(int id)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute("DELETE FROM MenuCategory WHERE ID = @Id AND ISCUSTOM = 1", new { Id = id });
    }
}
