public static class MenuAdminLogic
{
    public static List<MenuItemModel> GetItems()
    {
        RequireAdmin();
        return MenuLogic.GetMenuItemsForDisplay();
    }

    public static MenuItemModel GetItem(int menuNumber)
    {
        RequireAdmin();
        return GetItems().ElementAtOrDefault(menuNumber - 1) ?? throw new ArgumentException("Menu item not found.");
    }

    public static void Edit(int menuNumber, string name, double price, string description)
    {
        RequireAdmin();
        MenuItemModel item = GetItem(menuNumber);
        name = name.Trim();
        description = description.Trim();
        if (name.Length == 0) throw new ArgumentException("Name is required.");
        if (description.Length == 0) throw new ArgumentException("Description is required.");
        if (!double.IsFinite(price) || price <= 0) throw new ArgumentException("Price must be greater than zero.");
        item.Name = name;
        item.UnitPrice = price;
        item.Description = description;
        MenuItemAccess.Update(item);
    }

    public static List<AdminReservationModel> GetReservations(int menuNumber)
    {
        RequireAdmin();
        MenuItemModel item = GetItem(menuNumber);
        return ReservationAccess.GetByMenuItemId(item.Id);
    }

    private static void RequireAdmin()
    {
        if (!UserLogic.IsAdmin()) throw new UnauthorizedAccessException("Only admins can manage menu items.");
    }
}
