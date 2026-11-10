public static class MenuView
{
    public static void ShowMenu(UserModel currentUser)
    {
        if (currentUser == null)
        {
            Console.WriteLine("You must log in before viewing the menu.");
            return;
        }

        Dictionary<string, List<MenuItemModel>> groupedMenu = new MenuLogic().GetMenuGroupedByCategory();
        if (groupedMenu.Count == 0)
        {
            Console.WriteLine("There are currently no dishes available.");
            return;
        }

        if (!Console.IsOutputRedirected) Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("==========================================================================");
        Console.WriteLine("                            WHITE SMOKE MENU                              ");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        foreach (KeyValuePair<string, List<MenuItemModel>> category in groupedMenu)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n[{category.Key.ToUpperInvariant()}]");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 74));
            Console.ResetColor();

            foreach (MenuItemModel item in category.Value)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"ID {item.Id,-4} {item.Name}");
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

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Press Enter to return...");
        Console.ResetColor();
        Console.ReadLine();
    }
}
