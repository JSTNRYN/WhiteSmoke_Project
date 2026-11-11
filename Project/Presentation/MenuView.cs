public static class MenuView
{
    public static void ShowMenu(UserModel currentUser)
    {
        if (currentUser == null)
        {
            Console.WriteLine("You must log in before viewing the menu.");
            return;
        }

        List<MenuItemModel> menuItems = MenuLogic.GetMenuItemsForDisplay();
        if (menuItems.Count == 0)
        {
            Console.WriteLine("There are currently no dishes available.");
            return;
        }

        if (!Console.IsOutputRedirected) Console.Clear();
        PrintItems(menuItems);

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Press Enter to return...");
        Console.ResetColor();
        Console.ReadLine();
    }

    public static void PrintItems(IReadOnlyList<MenuItemModel> menuItems)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("==========================================================================");
        Console.WriteLine("                            WHITE SMOKE MENU                              ");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        string? category = null;
        int menuNumber = 1;
        foreach (MenuItemModel item in menuItems)
        {
            if (!string.Equals(category, item.Category, StringComparison.OrdinalIgnoreCase))
            {
                category = item.Category;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n[{category.ToUpperInvariant()}]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('-', 74));
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{menuNumber++,-3}] {item.Name}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  EUR {item.UnitPrice,6:F2}");
            if (MenuLogic.IsAdultItem(item))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  18+ item");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  {item.Description}");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
