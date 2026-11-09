public static class MenuAdminLogic
{
    public static List<MenuItemModel> GetItems()
    {
        RequireAdmin();
        return MenuItemAccess.LoadAllMenuItems(true)
            .OrderBy(item => item.Category, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Id)
            .ToList();
    }

    public static List<MenuCategoryModel> GetCategories()
    {
        RequireAdmin();
        return MenuCategoryAccess.GetAll();
    }

    public static int AddCategory(string name)
    {
        RequireAdmin();
        name = name.Trim();
        if (name.Length == 0) throw new ArgumentException("Category name is required.");
        if (MenuCategoryAccess.NameExists(name)) throw new ArgumentException("That category already exists.");
        return MenuCategoryAccess.Create(name);
    }

    public static void DeleteCategory(int id)
    {
        RequireAdmin();
        MenuCategoryModel category = MenuCategoryAccess.GetById(id) ?? throw new ArgumentException("Category not found.");
        if (!category.IsCustom) throw new InvalidOperationException("Fixed categories cannot be deleted.");
        if (MenuCategoryAccess.HasItems(id)) throw new InvalidOperationException("Move or remove all items from this category first.");
        MenuCategoryAccess.Delete(id);
    }

    public static int Add(string name, string category, double price, string description, bool adult)
    {
        RequireAdmin();
        MenuItemModel item = Validate(0, name, category, price, description, adult);
        return MenuItemAccess.Create(item);
    }

    public static void Edit(int id, string name, string category, double price, string description, bool adult)
    {
        RequireAdmin();
        MenuItemModel existing = MenuItemAccess.GetById(id) ?? throw new ArgumentException("Menu item not found.");
        MenuItemModel item = Validate(id, name, category, price, description, adult);
        item.Menu = existing.Menu;
        item.IsAvailable = existing.IsAvailable;
        MenuItemAccess.Update(item);
    }

    public static List<AdminReservationModel> GetReservations(int id)
    {
        RequireAdmin();
        if (MenuItemAccess.GetById(id) == null) throw new ArgumentException("Menu item not found.");
        return ReservationAccess.GetByMenuItemId(id);
    }

    public static void NotifyReservations(int id)
    {
        RequireAdmin();
        if (MenuItemAccess.GetById(id) == null) throw new ArgumentException("Menu item not found.");
        ReservationNoticeAccess.CreateForMenuItem(id, "The menu item you ordered is no longer available. We are sorry for the inconvenience. Please contact us as soon as possible.");
    }

    public static void Remove(int id)
    {
        RequireAdmin();
        MenuItemModel item = MenuItemAccess.GetById(id) ?? throw new ArgumentException("Menu item not found.");
        if (!item.IsAvailable) throw new InvalidOperationException("Menu item is already removed.");
        MenuItemAccess.Archive(item.Id);
    }

    public static void Reactivate(int id)
    {
        RequireAdmin();
        MenuItemModel item = MenuItemAccess.GetById(id) ?? throw new ArgumentException("Menu item not found.");
        if (item.IsAvailable) throw new InvalidOperationException("Menu item is already active.");
        if (!MenuCategoryAccess.NameExists(item.Category)) throw new InvalidOperationException("The item's category no longer exists.");
        if (MenuItemAccess.NameExists(item.Name, item.Category, item.Id)) throw new InvalidOperationException("Another item with this name already exists in this category.");
        MenuItemAccess.Activate(item.Id);
    }

    private static MenuItemModel Validate(int id, string name, string category, double price, string description, bool adult)
    {
        name = name.Trim();
        category = category.Trim();
        description = description.Trim();
        if (name.Length == 0 || category.Length == 0 || description.Length == 0) throw new ArgumentException("Name, category and description are required.");
        if (!double.IsFinite(price) || price <= 0) throw new ArgumentException("Price must be greater than zero.");
        if (MenuItemAccess.NameExists(name, category, id == 0 ? null : id)) throw new ArgumentException("An item with this name already exists in this category.");
        if (!MenuCategoryAccess.NameExists(category)) throw new ArgumentException("Choose an existing category.");
        return new MenuItemModel(id, string.Empty, name, category, price, description) { Adult = adult };
    }

    private static void RequireAdmin()
    {
        if (!UserLogic.IsAdmin()) throw new UnauthorizedAccessException("Only admins can manage menu items.");
    }
}
