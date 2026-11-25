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
