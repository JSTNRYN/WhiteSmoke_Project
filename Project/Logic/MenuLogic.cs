public class MenuLogic
{
    public static bool IsAdultItem(MenuItemModel item)
    {
        return item.Adult
            || item.Category.Contains("beer", StringComparison.OrdinalIgnoreCase)
            || item.Category.Contains("wine", StringComparison.OrdinalIgnoreCase)
            || item.Category.Contains("cocktail", StringComparison.OrdinalIgnoreCase)
            || item.Category.Contains("digestive", StringComparison.OrdinalIgnoreCase)
            || item.Category.Contains("after dinner", StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsKnownChoice(string? input, bool isLoggedIn, bool isAdmin)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        return input.Trim() switch
        {
            "1" or "2" or "3" or "4" when isLoggedIn => true,
            "5" when isLoggedIn && isAdmin => true,
            "1" or "2" or "3" when !isLoggedIn => true,
            _ => false
        };
    }

    public Dictionary<string, List<MenuItemModel>> GetMenuGroupedByCategory()
    {
        List<MenuItemModel> allItems = GetMenuItemsForDisplay();

        if (allItems == null)
        {
            return new Dictionary<string, List<MenuItemModel>>();
        }

        Dictionary<string, List<MenuItemModel>> groupedMenu = new();

        foreach (var item in allItems)
        {
            string category = NormalizeCategory(item.Category);
            if (!groupedMenu.ContainsKey(category))
            {
                groupedMenu[category] = new List<MenuItemModel>();
            }

            item.Category = category;
            groupedMenu[category].Add(item);
        }

        return groupedMenu;
    }

    public static List<MenuItemModel> GetMenuItemsForDisplay()
    {
        List<MenuItemModel> items = MenuItemAccess.GetAll();
        foreach (MenuItemModel item in items)
            item.Category = NormalizeCategory(item.Category);

        return items
            .OrderBy(item => item.Category, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string NormalizeCategory(string category)
    {
        return category.Trim().ToLowerInvariant() switch
        {
            "dessert" or "desserts" or "sweets" => "Desserts",
            _ => category.Trim()
        };
    }
}
