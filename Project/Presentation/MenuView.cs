public static class MenuView
{
    public static void ShowMenu(UserModel currentUser)
    {
        if (currentUser == null)
        {
            Console.WriteLine("Je moet eerst inloggen om het menu te kunnen bekijken!");
            return;
        }

        MenuLogic logic = new MenuLogic();
        Dictionary<string, List<MenuItemModel>> groupedMenu = logic.GetMenuGroupedByCategory();

        if (groupedMenu == null)
        {
            Console.WriteLine("Er is een probleem opgetreden bij het laden van het menu uit de database.");
            return;
        }

        if (groupedMenu.Count == 0)
        {
            Console.WriteLine("Er zijn momenteel geen gerechten beschikbaar op het menu.");
            return;
        }

        Console.Clear();
        Console.WriteLine("=================================");
        Console.WriteLine("        WHITE SMOKE MENU         ");
        Console.WriteLine("=================================");

        foreach (var categoryPair in groupedMenu)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n--- {categoryPair.Key.ToUpper()} ---");
            Console.ResetColor();

            foreach (var item in categoryPair.Value)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{item.Name}");
                Console.ResetColor();
                Console.WriteLine($" - €{item.UnitPrice:F2}");

                Console.WriteLine($"   {item.Description}");
            }
        }

        Console.WriteLine("\nDruk op Enter om terug te gaan naar het hoofdmenu...");
        Console.ReadLine();
    }
}