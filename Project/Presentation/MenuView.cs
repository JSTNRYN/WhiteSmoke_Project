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

        Console.WriteLine("========================================");
        Console.WriteLine("             WHITE SMOKE MENU           ");
        Console.WriteLine("========================================");

        foreach (KeyValuePair<string, List<MenuItemModel>> category in groupedMenu)
        {
            Console.WriteLine($"\n--- {category.Key.ToUpperInvariant()} ---");
            foreach (MenuItemModel item in category.Value)
            {
                Console.WriteLine($"{item.Name}  |  EUR {item.UnitPrice:F2}");
                if (MenuLogic.IsAdultItem(item))
                    Console.WriteLine("  ADULT ITEM (18+)");
                if (!string.IsNullOrWhiteSpace(item.Description))
                    Console.WriteLine($"  {item.Description}");
            }
        }

        Console.WriteLine("\nPress Enter to return...");
        Console.ReadLine();
    }
}
