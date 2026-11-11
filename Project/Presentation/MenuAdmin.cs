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
            Console.WriteLine("1) View menu");
            Console.WriteLine("2) Edit menu item");
            Console.WriteLine("3) View item reservations");
            Console.WriteLine("4) Log out");
            Console.WriteLine("0) Back to main menu");

            try
            {
                switch (Console.ReadLine()?.Trim())
                {
                    case "1": ViewItems(); break;
                    case "2": EditItem(); break;
                    case "3": ViewReservations(); break;
                    case "4": UserLogic.Logout(); return;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice."); Pause(); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}");
                Pause();
            }
        }
    }

    private static void ViewItems()
    {
        Clear();
        PrintItems();
        Pause();
    }

    private static void PrintItems()
    {
        MenuView.PrintItems(MenuAdminLogic.GetItems());
    }

    private static void EditItem()
    {
        try
        {
            Clear();
            Console.WriteLine("EDIT MENU ITEM");
            Console.WriteLine("==============\n");
            PrintItems();
            Console.WriteLine("\n[0] Back to admin panel");

            int menuNumber = ReadMenuNumber();
            if (menuNumber == 0) return;

            MenuItemModel item = MenuAdminLogic.GetItem(menuNumber);
            Console.WriteLine("Warning: saved changes are immediately visible on existing reservations containing this item.");
            Console.WriteLine("Reservations containing this menu item:");
            List<AdminReservationModel> reservations = MenuAdminLogic.GetReservations(menuNumber);
            if (reservations.Count == 0) Console.WriteLine("No reservations found.");
            foreach (AdminReservationModel reservation in reservations)
                Console.WriteLine($"Reservation #{reservation.ReservationId} | {reservation.Date} {reservation.TimeSlot} | {reservation.FirstName} {reservation.LastName} | {reservation.Email}");

            Console.Write($"Name [{item.Name}]: ");
            string name = ReadOrDefault(item.Name);
            Console.Write($"Price [{item.UnitPrice:F2}]: ");
            double price = ReadPrice(item.UnitPrice);
            Console.Write($"Description [{item.Description}]: ");
            string description = ReadOrDefault(item.Description);

            MenuAdminLogic.Edit(menuNumber, name, price, description);
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
            Clear();
            Console.WriteLine("VIEW ITEM RESERVATIONS");
            Console.WriteLine("======================\n");
            PrintItems();
            Console.WriteLine("\n[0] Back to admin panel");

            int menuNumber = ReadMenuNumber();
            if (menuNumber == 0) return;

            List<AdminReservationModel> reservations = MenuAdminLogic.GetReservations(menuNumber);
            if (reservations.Count == 0) Console.WriteLine("No reservations found.");
            foreach (AdminReservationModel reservation in reservations)
                Console.WriteLine($"Reservation #{reservation.ReservationId} | {reservation.Date} {reservation.TimeSlot} | {reservation.FirstName} {reservation.LastName} | {reservation.Email}");

            WaitForAdminBack();
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Pause();
    }

    private static int ReadMenuNumber()
    {
        while (true)
        {
            Console.Write("Choose a menu number (0 to go back): ");
            string? input = Console.ReadLine();
            if (input == null) return 0;
            if (int.TryParse(input.Trim(), out int menuNumber) && menuNumber >= 0) return menuNumber;
            Console.WriteLine("Enter a number from the menu, or 0 to go back.");
        }
    }

    private static string ReadOrDefault(string fallback)
    {
        string value = Console.ReadLine()?.Trim() ?? string.Empty;
        return value.Length == 0 ? fallback : value;
    }

    private static double ReadPrice(double fallback)
    {
        while (true)
        {
            string value = Console.ReadLine()?.Trim() ?? string.Empty;
            if (value.Length == 0) return fallback;
            if (double.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out double price) && price > 0)
                return price;
            Console.Write("Price must be greater than zero. Try again: ");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    private static void WaitForAdminBack()
    {
        Console.Write("\nEnter 0 to return to the admin panel: ");
        Console.ReadLine();
    }

    private static void Clear()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
    }
}
