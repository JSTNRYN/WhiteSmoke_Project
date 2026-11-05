using Microsoft.Data.Sqlite;
using Dapper;
public static class MenuItemAccess
{
    private static string connectionString = "Data Source=template.project.db;";
   

    public static List<MenuItemModel> LoadAllMenuItems()
    {
        try
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                string query = "SELECT ID, MENU, NAME, CATEGORY, UNITPRICE, DESCRIPTION FROM MenuItem";
                
                
                List<MenuItemModel> items = conn.Query<MenuItemModel>(query).ToList();
                return items;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database fout: {ex.Message}");
            return null;
        }
    }
}