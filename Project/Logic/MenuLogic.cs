public class MenuLogic
{
    public static void Update(MenuItemModel item)
    {
        MenuItemAccess.Update(item);
    }

    public static MenuItemModel? FindById(int id)
    {
        MenuItemModel? item = MenuItemAccess.FindById(id);
        if (item == null) { return null; }

        return item;
    }

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

        string choice = input.Trim();
        if (isLoggedIn)
        {
            return choice == "1" || choice == "2" || choice == "3" || choice == "4";
        }

        return choice == "1" || choice == "2" || choice == "3";
    }

    public Dictionary<string, List<MenuItemModel>> GetMenuGroupedByCategory()
    {
        List<MenuItemModel> allItems = GetMenuItemsForDisplay();

        if (allItems == null)
        {
            return new Dictionary<string, List<MenuItemModel>>();
        }

        Dictionary<string, List<MenuItemModel>> groupedMenu = new Dictionary<string, List<MenuItemModel>>();

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
        {
            item.Category = NormalizeCategory(item.Category);
        }

        return items
            .OrderBy(item => item.Category, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static List<MenuItemModel> GetNonAdminMenuItems()
    {
        List<MenuItemModel> items = MenuItemAccess.GetAll();
        foreach (MenuItemModel item in items)
        {
            item.Category = NormalizeCategory(item.Category);
        }

        return items
            .Where(item => item.IsViewable is true)
            .OrderBy(item => item.Category, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string NormalizeCategory(string category)
    {
        string trimmed = category.Trim();
        string lower = trimmed.ToLowerInvariant();

        if (lower == "dessert" || lower == "desserts" || lower == "sweets")
        {
            return "Desserts";
        }

        return trimmed;
    }
}
