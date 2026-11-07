public class MenuLogic
{
    public Dictionary<string, List<MenuItemModel>> GetMenuGroupedByCategory()
    {
        List<MenuItemModel> allItems = MenuItemAccess.LoadAllMenuItems();

        if (allItems == null)
        {
            return new Dictionary<string, List<MenuItemModel>>();
        }

        Dictionary<string, List<MenuItemModel>> groupedMenu = new Dictionary<string, List<MenuItemModel>>();

        foreach (var item in allItems)
        {
            if (!groupedMenu.ContainsKey(item.Category))
            {
                groupedMenu[item.Category] = new List<MenuItemModel>();
            }

            groupedMenu[item.Category].Add(item);
        }

        return groupedMenu;
    }
}
