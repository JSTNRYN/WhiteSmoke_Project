using Dapper;

public static class MenuItemAccess
{
    public static List<MenuItemModel> LoadAllMenuItems()
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
                    CAST(ADULT AS INTEGER) AS Adult
                FROM MenuItem
                ORDER BY CATEGORY, NAME";

            List<MenuItemModel> items = conn.Query<MenuItemModel>(query).ToList();
            foreach (MenuItemModel item in items)
            {
                item.Name = CleanEncoding(item.Name);
                item.Description = CleanEncoding(item.Description);
                item.Category = CleanEncoding(item.Category);
            }

            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
            return new List<MenuItemModel>();
        }
    }

    private static string CleanEncoding(string value)
    {
        const string replacementCharacter = "\uFFFD";

        return value
            .Replace($"K{replacementCharacter}sekrainer", "K\u00E4sekrainer")
            .Replace($"C{replacementCharacter}te", "C\u00F4te")
            .Replace($"jalape{replacementCharacter}os", "jalape\u00F1os")
            .Replace($"saut{replacementCharacter}ed", "saut\u00E9ed")
            .Replace($"Spinach {replacementCharacter} la Cr{replacementCharacter}me", "Spinach \u00E0 la Cr\u00E8me")
            .Replace($"Ard{replacementCharacter}che", "Ard\u00E8che")
            .Replace($"Ros{replacementCharacter}", "Ros\u00E9")
            .Replace($"Mo{replacementCharacter}t", "Mo\u00EBt")
            .Replace($"Imp{replacementCharacter}rial", "Imp\u00E9rial")
            .Replace($"Marqu{replacementCharacter}s", "Marqu\u00E9s")
            .Replace($"A{replacementCharacter}n{replacementCharacter}", "A\u00EEn\u00E9")
            .Replace($"C{replacementCharacter}dres", "C\u00E8dres")
            .Replace($"Ch{replacementCharacter}teauneuf", "Ch\u00E2teauneuf")
            .Replace($"Rh{replacementCharacter}ne", "Rh\u00F4ne");
    }
}
