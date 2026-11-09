public static class MenuAdmin
{
    public static void Start()
    {
        if (!UserLogic.IsAdmin()) return;
        while (true)
        {
            Clear();
            Console.WriteLine("MENU ADMINISTRATION");
            Console.WriteLine("===================");
            Console.WriteLine("1) View items");
            Console.WriteLine("2) Add item");
            Console.WriteLine("3) Edit item");
            Console.WriteLine("4) Remove item");
            Console.WriteLine("5) View reservations for item");
            Console.WriteLine("6) Preview customer menu");
            Console.WriteLine("7) Manage categories");
            Console.WriteLine("8) Log out");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ListItems(); break;
                case "2": AddItem(); break;
                case "3": EditItem(); break;
                case "4": RemoveItem(); break;
                case "5": ShowReservations(); break;
                case "6": MenuView.ShowMenu(UserLogic.CurrentAccount!); break;
                case "7": ManageCategories(); break;
                case "8": UserLogic.Logout(); return;
                default: Console.WriteLine("Invalid choice."); Pause(); break;
            }
        }
    }

    private static void ListItems()
    {
        Clear();
        string? category = null;
        foreach (MenuItemModel item in MenuAdminLogic.GetItems())
        {
            if (!string.Equals(category, item.Category, StringComparison.OrdinalIgnoreCase))
            {
                category = item.Category;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n[{category.ToUpperInvariant()}]");
                Console.ResetColor();
            }

            Console.ForegroundColor = item.IsAvailable ? ConsoleColor.White : ConsoleColor.DarkGray;
            Console.WriteLine($"ID {item.Id,-4} {item.Name,-40} EUR {item.UnitPrice,7:F2}  {(item.IsAvailable ? "ACTIVE" : "REMOVED")}");
            Console.ResetColor();
        }
        Pause();
    }

    private static void AddItem()
    {
        try
        {
            Console.WriteLine("1) Create a new item");
            Console.WriteLine("2) Reactivate a removed item");
            Console.WriteLine("3) Cancel");
            string action = Console.ReadLine()?.Trim() ?? string.Empty;
            if (action == "2")
            {
                ReactivateItem();
                return;
            }
            if (action != "1") return;
            MenuItemModel input = ReadItem(null);
            int id = MenuAdminLogic.Add(input.Name, input.Category, input.UnitPrice, input.Description, input.Adult);
            Console.WriteLine($"Item {id} was added.");
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        Pause();
    }

    private static void ReactivateItem()
    {
        List<MenuItemModel> removedItems = MenuAdminLogic.GetItems().Where(item => !item.IsAvailable).ToList();
        if (removedItems.Count == 0)
        {
            Console.WriteLine("There are no removed items to reactivate.");
            Pause();
            return;
        }

        foreach (MenuItemModel item in removedItems)
            Console.WriteLine($"ID {item.Id} | {item.Name} | {item.Category} | EUR {item.UnitPrice:F2}");
        Console.Write("Menu item ID to reactivate: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) throw new ArgumentException("ID must be a number.");
        MenuAdminLogic.Reactivate(id);
        Console.WriteLine($"Item {id} was reactivated.");
        Pause();
    }

    private static void EditItem()
    {
        try
        {
            int id = ReadId();
            MenuItemModel current = MenuItemAccess.GetById(id) ?? throw new ArgumentException("Menu item not found.");
            MenuItemModel input = ReadItem(current);
            MenuAdminLogic.Edit(id, input.Name, input.Category, input.UnitPrice, input.Description, input.Adult);
            Console.WriteLine($"Item {id} was updated. Existing orders keep their original data.");
            AskForNotice(id);
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); Pause(); }
    }

    private static void RemoveItem()
    {
        try
        {
            int id = ReadId();
            MenuItemModel item = MenuItemAccess.GetById(id) ?? throw new ArgumentException("Menu item not found.");
            Console.WriteLine($"Item {item.Id}: {item.Name}");
            Console.WriteLine("Remove from the customer menu? (yes/no)");
            if (Console.ReadLine()?.Trim().ToLowerInvariant() is not ("yes" or "y")) return;

            MenuAdminLogic.Remove(id);
            Console.WriteLine("1) Keep existing orders valid");
            Console.WriteLine("2) Mark customers with this item for contact");
            if (Console.ReadLine()?.Trim() == "2") MenuAdminLogic.NotifyReservations(id);
            Console.WriteLine("Item is no longer available for new orders.");
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        Pause();
    }

    private static void AskForNotice(int id)
    {
        List<AdminReservationModel> reservations = MenuAdminLogic.GetReservations(id);
        if (reservations.Count == 0) return;
        Console.WriteLine($"There are {reservations.Count} reservation(s) containing this item.");
        Console.WriteLine("Mark customers for contact? (yes/no)");
        if (Console.ReadLine()?.Trim().ToLowerInvariant() is "yes" or "y") MenuAdminLogic.NotifyReservations(id);
    }

    private static void ShowReservations()
    {
        try
        {
            int id = ReadId();
            MenuItemModel item = MenuItemAccess.GetById(id) ?? throw new ArgumentException("Menu item not found.");
            List<AdminReservationModel> reservations = MenuAdminLogic.GetReservations(id);
            Console.WriteLine($"Reservations for ID {item.Id}: {item.Name}");
            if (reservations.Count == 0) Console.WriteLine("No reservations found.");
            foreach (AdminReservationModel reservation in reservations)
                Console.WriteLine($"Reservation #{reservation.ReservationId} | {reservation.Date} {reservation.TimeSlot} | {reservation.FirstName} {reservation.LastName} | {reservation.Email} | {reservation.PartySize} guests");
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
        Pause();
    }

    private static MenuItemModel ReadItem(MenuItemModel? current)
    {
        Console.Write($"Name{Default(current?.Name)}: ");
        string name = Required(current?.Name);
        string category = ReadCategory(current?.Category);
        Console.Write($"Price{Default(current?.UnitPrice.ToString("F2"))}: ");
        double price = Price(current?.UnitPrice);
        Console.Write($"Description{Default(current?.Description)}: ");
        string description = Required(current?.Description);
        Console.Write($"18+ item (yes/no){Default(current?.Adult == true ? "yes" : "no")}: ");
        bool adult = BooleanValue(current?.Adult);
        return new MenuItemModel(0, string.Empty, name, category, price, description) { Adult = adult };
    }

    private static int ReadId()
    {
        Console.Write("Menu item ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0) throw new ArgumentException("ID must be a positive number.");
        return id;
    }

    private static string ReadCategory(string? current)
    {
        List<MenuCategoryModel> categories = MenuAdminLogic.GetCategories();
        Console.WriteLine("Categories:");
        foreach (MenuCategoryModel category in categories)
            Console.WriteLine($"{category.Id}) {category.Name}{(category.IsCustom ? " (custom)" : "")}");
        Console.Write($"Category ID{(current == null ? "" : $" [{categories.FirstOrDefault(c => c.Name.Equals(current, StringComparison.OrdinalIgnoreCase))?.Id}]")}: ");
        string input = Console.ReadLine()?.Trim() ?? string.Empty;
        if (input.Length == 0 && current != null) return current;
        if (!int.TryParse(input, out int id)) throw new ArgumentException("Category ID must be a number.");
        MenuCategoryModel selected = categories.FirstOrDefault(category => category.Id == id) ?? throw new ArgumentException("Category not found.");
        return selected.Name;
    }

    private static void ManageCategories()
    {
        while (true)
        {
            Clear();
            Console.WriteLine("CATEGORIES");
            foreach (MenuCategoryModel category in MenuAdminLogic.GetCategories())
                Console.WriteLine($"{category.Id}) {category.Name}{(category.IsCustom ? " (custom)" : " (fixed)")}");
            Console.WriteLine("N) New category");
            Console.WriteLine("D) Delete custom category");
            Console.WriteLine("B) Back");
            string input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (input.Equals("B", StringComparison.OrdinalIgnoreCase)) return;
            if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("New category name: ");
                try { Console.WriteLine($"Category {MenuAdminLogic.AddCategory(Console.ReadLine() ?? string.Empty)} created."); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                Pause();
                continue;
            }
            if (input.Equals("D", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    Console.Write("Custom category ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int id)) throw new ArgumentException("ID must be a number.");
                    MenuAdminLogic.DeleteCategory(id);
                    Console.WriteLine("Category deleted.");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                Pause();
                continue;
            }
            Console.WriteLine("Invalid choice.");
            Pause();
        }
    }

    private static string Required(string? fallback)
    {
        string value = Console.ReadLine()?.Trim() ?? string.Empty;
        return value.Length == 0 && fallback != null ? fallback : value;
    }

    private static double Price(double? fallback)
    {
        string value = Console.ReadLine()?.Trim() ?? string.Empty;
        if (value.Length == 0 && fallback.HasValue) return fallback.Value;
        if (!double.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out double price)) throw new ArgumentException("Price must be a valid number.");
        return price;
    }

    private static bool BooleanValue(bool? fallback)
    {
        string value = Console.ReadLine()?.Trim().ToLowerInvariant() ?? string.Empty;
        if (value.Length == 0 && fallback.HasValue) return fallback.Value;
        if (value is "yes" or "y") return true;
        if (value is "no" or "n") return false;
        throw new ArgumentException("Choose yes or no.");
    }

    private static string Default(string? value) => value == null ? string.Empty : $" [{value}]";
    private static void Pause() { Console.WriteLine("Press Enter to continue."); Console.ReadLine(); }
    private static void Clear() { if (!Console.IsOutputRedirected) Console.Clear(); }
}
