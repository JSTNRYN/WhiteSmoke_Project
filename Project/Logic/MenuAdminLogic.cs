public static class MenuAdminLogic
{
    public static List<MenuItemModel> GetItems()
    {
        RequireAdmin();
        return MenuItemAccess.GetAll();
    }

    public static MenuItemModel GetItem(int id)
    {
        RequireAdmin();
        return MenuItemAccess.GetById(id) ?? throw new ArgumentException("Menu item not found.");
    }

    public static void Edit(int id, string name, double price, string description)
    {
        RequireAdmin();
        MenuItemModel item = GetItem(id);
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

    public static List<AdminReservationModel> GetReservations(int menuItemId)
    {
        RequireAdmin();
        GetItem(menuItemId);
        return ReservationAccess.GetByMenuItemId(menuItemId);
    }

    private static void RequireAdmin()
    {
        if (!UserLogic.IsAdmin()) throw new UnauthorizedAccessException("Only admins can manage menu items.");
    }
}
