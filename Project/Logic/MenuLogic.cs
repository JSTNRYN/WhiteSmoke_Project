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
            "1" or "2" when !isLoggedIn => true,
            _ => false
        };
    }

    public Dictionary<string, List<MenuItemModel>> GetMenuGroupedByCategory()
    {
        List<MenuItemModel> allItems = MenuItemAccess.LoadAllMenuItems();

        if (allItems == null)
        {
            return new Dictionary<string, List<MenuItemModel>>();
        }

        Dictionary<string, List<MenuItemModel>> groupedMenu = new();

        foreach (var item in allItems)
        {
            if (!groupedMenu.ContainsKey(item.Category))
            {
                groupedMenu[item.Category] = new List<MenuItemModel>();
            }

            groupedMenu[item.Category].Add(item);
        }

        foreach (List<MenuItemModel> items in groupedMenu.Values)
            items.Sort((left, right) => string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase));

        return groupedMenu;
    }
}
