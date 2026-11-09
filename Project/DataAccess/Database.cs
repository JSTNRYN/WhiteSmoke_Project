using Microsoft.Data.Sqlite;
using Dapper;

public static class Database
{
    private static readonly string DatabasePath = ResolveDatabasePath();

    private static string ResolveDatabasePath()
    {
        DirectoryInfo? directory = Directory.GetParent(AppContext.BaseDirectory);
        while (directory != null)
        {
            string candidate = Path.Combine(directory.FullName, "DataSources", "project.db");
            if (File.Exists(candidate)) return candidate;
            directory = directory.Parent;
        }

        return Path.Combine(AppContext.BaseDirectory, "DataSources", "project.db");
    }

    public static SqliteConnection CreateConnection()
    {
        return new SqliteConnection($"Data Source={DatabasePath}");
    }

    public static void Initialize()
    {
        using var connection = CreateConnection();
        connection.Open();

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS MenuItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                MENU TEXT,
                NAME TEXT,
                CATEGORY TEXT,
                UNITPRICE DOUBLE,
                DESCRIPTION TEXT,
                ADULT BOOLEAN,
                ISAVAILABLE INTEGER NOT NULL DEFAULT 1
            )");

        connection.Execute("CREATE TABLE IF NOT EXISTS MenuCategory (ID INTEGER PRIMARY KEY AUTOINCREMENT, NAME TEXT NOT NULL UNIQUE COLLATE NOCASE, ISCUSTOM INTEGER NOT NULL DEFAULT 1)");
        foreach (string category in MenuCategoryNames.Fixed)
            connection.Execute("INSERT OR IGNORE INTO MenuCategory (NAME, ISCUSTOM) VALUES (@Name, 0)", new { Name = category });

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS ""Order"" (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                ReservationID INTEGER,
                TotalPrice REAL NOT NULL
            );
            CREATE TABLE IF NOT EXISTS OrderItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderID INTEGER NOT NULL,
                MenuItemID INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                ItemName TEXT,
                UnitPrice REAL,
                Category TEXT,
                Description TEXT,
                Adult INTEGER
            )");

        AddColumnIfMissing(connection, "MenuItem", "MENU", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "CATEGORY", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "DESCRIPTION", "TEXT");
        AddColumnIfMissing(connection, "MenuItem", "ADULT", "BOOLEAN");
        AddColumnIfMissing(connection, "MenuItem", "ISAVAILABLE", "INTEGER NOT NULL DEFAULT 1");
        AddColumnIfMissing(connection, "User", "DATEOFBIRTH", "TEXT");
        AddColumnIfMissing(connection, "Order", "RESERVATIONID", "INTEGER");
        AddColumnIfMissing(connection, "Order", "TOTALPRICE", "REAL");
        AddColumnIfMissing(connection, "OrderItem", "ITEMNAME", "TEXT");
        AddColumnIfMissing(connection, "OrderItem", "UNITPRICE", "REAL");
        AddColumnIfMissing(connection, "OrderItem", "CATEGORY", "TEXT");
        AddColumnIfMissing(connection, "OrderItem", "DESCRIPTION", "TEXT");
        AddColumnIfMissing(connection, "OrderItem", "ADULT", "INTEGER");
        connection.Execute(@"
            UPDATE OrderItem
            SET ItemName = COALESCE(ItemName, (SELECT NAME FROM MenuItem WHERE MenuItem.ID = OrderItem.MenuItemID)),
                UnitPrice = COALESCE(UnitPrice, (SELECT UNITPRICE FROM MenuItem WHERE MenuItem.ID = OrderItem.MenuItemID)),
                Category = COALESCE(Category, (SELECT CATEGORY FROM MenuItem WHERE MenuItem.ID = OrderItem.MenuItemID)),
                Description = COALESCE(Description, (SELECT DESCRIPTION FROM MenuItem WHERE MenuItem.ID = OrderItem.MenuItemID)),
                Adult = COALESCE(Adult, (SELECT ADULT FROM MenuItem WHERE MenuItem.ID = OrderItem.MenuItemID))");

        connection.Execute("DELETE FROM MenuItem WHERE NAME LIKE '%' || char(65533) || '%' OR CATEGORY LIKE '%' || char(65533) || '%' OR DESCRIPTION LIKE '%' || char(65533) || '%' OR NAME LIKE '%' || char(239) || char(191) || char(189) || '%' OR CATEGORY LIKE '%' || char(239) || char(191) || char(189) || '%' OR DESCRIPTION LIKE '%' || char(239) || char(191) || char(189) || '%'");
        connection.Execute("UPDATE MenuItem SET CATEGORY = 'Desserts' WHERE LOWER(TRIM(CATEGORY)) IN ('dessert', 'desserts', 'sweets')");
        connection.Execute("INSERT OR IGNORE INTO MenuCategory (NAME, ISCUSTOM) SELECT DISTINCT TRIM(CATEGORY), 1 FROM MenuItem WHERE TRIM(CATEGORY) <> ''");
        connection.Execute("DELETE FROM MenuCategory WHERE ISCUSTOM = 1 AND (NAME LIKE '%' || char(65533) || '%' OR NAME LIKE '%' || char(239) || char(191) || char(189) || '%' OR NOT EXISTS (SELECT 1 FROM MenuItem WHERE LOWER(TRIM(CATEGORY)) = LOWER(TRIM(MenuCategory.NAME))))");
        connection.Execute("CREATE TABLE IF NOT EXISTS ReservationNotice (ID INTEGER PRIMARY KEY AUTOINCREMENT, ReservationID INTEGER NOT NULL, MenuItemID INTEGER NOT NULL, Message TEXT NOT NULL, CreatedAt TEXT NOT NULL)");
        connection.Execute("INSERT INTO User (email, password, firstName, lastName, phonenumber, role, DateOfBirth) SELECT @Email, @Password, 'Test', 'Admin', '0000000000', 'Admin', '2000-01-01' WHERE NOT EXISTS (SELECT 1 FROM User WHERE email = @Email)", new { Email = "admin@admin.com", Password = BCrypt.Net.BCrypt.HashPassword("admin@admin.com") });
        connection.Execute("UPDATE User SET role = 'Admin', password = @Password WHERE email = @Email", new { Email = "admin@admin.com", Password = BCrypt.Net.BCrypt.HashPassword("admin@admin.com") });

        long menuItemCount = connection.ExecuteScalar<long>("SELECT COUNT(*) FROM MenuItem");
        if (menuItemCount == 0)
        {
            string seedPath = Path.Combine(AppContext.BaseDirectory, "DataSources", "menu-items.sql");
            string seed = File.ReadAllText(seedPath);
            connection.Execute(seed);
        }
    }

    private static void AddColumnIfMissing(SqliteConnection connection, string table, string column, string type)
    {
        bool columnExists = connection.Query<(int Id, string Name, string Type, int NotNull, string DefaultValue, int PrimaryKey)>($"PRAGMA table_info(\"{table}\")")
            .Any(item => item.Name.Equals(column, StringComparison.OrdinalIgnoreCase));

        if (!columnExists)
        {
            connection.Execute($"ALTER TABLE \"{table}\" ADD COLUMN \"{column}\" {type}");
        }
    }
}
