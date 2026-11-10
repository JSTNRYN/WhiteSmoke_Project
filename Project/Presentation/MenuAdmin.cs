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
            Console.WriteLine("1) View menu items");
            Console.WriteLine("2) Edit menu item");
            Console.WriteLine("3) View item reservations");
            Console.WriteLine("4) Log out");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ViewItems(); break;
                case "2": EditItem(); break;
                case "3": ViewReservations(); break;
                case "4": UserLogic.Logout(); return;
                default: Console.WriteLine("Invalid choice."); Pause(); break;
            }
        }
    }

    private static void ViewItems()
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

            Console.WriteLine($"ID {item.Id,-4} {item.Name,-40} EUR {item.UnitPrice,7:F2}");
        }
        Pause();
    }

    private static void EditItem()
    {
        try
        {
            int id = ReadId();
            MenuItemModel item = MenuAdminLogic.GetItem(id);
            Console.WriteLine("Warning: saved changes are immediately visible on existing reservations containing this item.");
            Console.WriteLine("Reservations containing this menu item:");
            List<AdminReservationModel> reservations = MenuAdminLogic.GetReservations(id);
            if (reservations.Count == 0) Console.WriteLine("No reservations found.");
            foreach (AdminReservationModel reservation in reservations)
                Console.WriteLine($"Reservation #{reservation.ReservationId} | {reservation.Date} {reservation.TimeSlot} | {reservation.FirstName} {reservation.LastName} | {reservation.Email}");

            Console.Write($"Name [{item.Name}]: ");
            string name = ReadOrDefault(item.Name);
            Console.Write($"Price [{item.UnitPrice:F2}]: ");
            double price = ReadPrice(item.UnitPrice);
            Console.Write($"Description [{item.Description}]: ");
            string description = ReadOrDefault(item.Description);

            MenuAdminLogic.Edit(id, name, price, description);
            Console.WriteLine("Menu item updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Pause();
    }

    private static void ViewReservations()
    {
        try
        {
            int id = ReadId();
            List<AdminReservationModel> reservations = MenuAdminLogic.GetReservations(id);
            if (reservations.Count == 0) Console.WriteLine("No reservations found.");
            foreach (AdminReservationModel reservation in reservations)
                Console.WriteLine($"Reservation #{reservation.ReservationId} | {reservation.Date} {reservation.TimeSlot} | {reservation.FirstName} {reservation.LastName} | {reservation.Email}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Pause();
    }

    private static int ReadId()
    {
        Console.Write("Menu item ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0) throw new ArgumentException("ID must be a positive number.");
        return id;
    }

    private static string ReadOrDefault(string fallback)
    {
        string value = Console.ReadLine()?.Trim() ?? string.Empty;
        return value.Length == 0 ? fallback : value;
    }

    private static double ReadPrice(double fallback)
    {
        string value = Console.ReadLine()?.Trim() ?? string.Empty;
        if (value.Length == 0) return fallback;
        if (!double.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out double price)) throw new ArgumentException("Price must be a valid number.");
        return price;
    }

    private static void Pause()
    {
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private static void Clear()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
    }
}
