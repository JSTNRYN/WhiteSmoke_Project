public static class MenuView
{
    private static readonly string[] _editOptions = ["Hide/Show", "Update", "Go back"];

    public static void ShowMenu()
    {
        List<MenuItemModel> menuItems = MenuLogic.GetMenuItemsForDisplay();
        if (menuItems.Count == 0)
        {
            Console.WriteLine("There are currently no dishes available.");
            return;
        }

        while (true)
        {
            menuItems = MenuLogic.GetMenuItemsForDisplay();
            Console.Clear();

            if (!Console.IsOutputRedirected) Console.Clear();
            PrintItems(menuItems, true);

            Console.WriteLine("Enter in id to select the item.");
            Console.WriteLine("Type 'back' to go back to admin panel.");

            string input = Console.ReadLine() ?? string.Empty;
            if (input.Equals("back", StringComparison.OrdinalIgnoreCase)) { return; }

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Invalid input, Enter a item id.");
                continue;
            }

            MenuItemModel? selectedItem = MenuLogic.FindById(id);
            if (selectedItem == null)
            {
                Console.WriteLine("No menu item was found with that id.");
                continue;
            }

            Edit(selectedItem);
        }
    }

    public static void PrintItems(IReadOnlyList<MenuItemModel> menuItems, bool isAdmin = false)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("==========================================================================");
        Console.WriteLine("                            WHITE SMOKE MENU                              ");
        Console.WriteLine("==========================================================================");
        Console.ResetColor();

        string? category = null;
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
            Console.Write($"[{item.Id}] {item.Name,-45}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" EUR {item.UnitPrice,6:F2}");
            if (MenuLogic.IsAdultItem(item))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  18+ item");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  {item.Description}");
            if(isAdmin)
            {
                Console.WriteLine($"  {(item.IsViewable ? "Visible" : "Hidden")}");
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }

    public static void Edit(MenuItemModel menu)
    {
        while (true)
        {
            MenuItemModel? menuCopy = MenuItemAccess.FindById(menu.Id);

            Console.Clear();
            Console.WriteLine($"ID:          {menu.Id}");
            Console.WriteLine($"Name:        {menu.Name}");
            Console.WriteLine($"Description: {menu.Description}");
            Console.WriteLine($"Menu type:   {menu.Menu}");
            Console.WriteLine($"Category:    {menu.Category}");
            Console.WriteLine($"Price:       {menu.UnitPrice}");
            Console.WriteLine($"For adults:  {(menu.Adult ? "Yes" : "No")}");
            Console.WriteLine($"Viewable:    {(menu.IsViewable ? "Yes" : "No")} {(menuCopy?.IsViewable == menu.IsViewable ? "" : "(New)")}");
            Console.WriteLine();

            switch (Menu.InteractiveMenu(_editOptions))
            {
                case 0:
                    menu.IsViewable = menu.IsViewable ? false : true;
                    break;
                case 1:
                    MenuLogic.Update(menu);
                    break;
                case 2:
                    return;
            }
        }
    }
}
