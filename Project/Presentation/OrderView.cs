public static class OrderView
{
    public static List<OrderLineModel>? AskForOrder()
    {
        List<MenuItemModel> menuItems = MenuLogic.GetNonAdminMenuItems();
        List<OrderLineModel> orderLines = new List<OrderLineModel>();

        if (menuItems.Count == 0)
        {
            Console.WriteLine("The menu is currently empty.");
            return null;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== PRE-ORDER DISHES ===");
            Console.WriteLine("1) View menu");
            Console.WriteLine("2) Add dishes to order");
            Console.WriteLine("3) View current order");
            Console.WriteLine("4) Finish order and proceed");
            Console.WriteLine("5) Cancel order");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine()?.Trim();
            switch (choice)
            {
                case "1":
                    menuItems = MenuLogic.GetNonAdminMenuItems();
                    MenuView.PrintItems(menuItems);
                    Console.WriteLine("Press Enter to return...");
                    Console.ReadLine();
                    break;
                case "2":
                    menuItems = MenuLogic.GetNonAdminMenuItems();
                    MakeOrder(menuItems, orderLines);
                    break;
                case "3":
                    ShowOrder(orderLines);
                    break;
                case "4":
                    return orderLines;
                case "5":
                    return null;
                default:
                    Console.WriteLine("Invalid input. Please choose 1 to 5.");
                    break;
            }
        }
    }

    private static void MakeOrder(List<MenuItemModel> menuItems, List<OrderLineModel> orderLines)
    {
        Console.WriteLine();
        MenuView.PrintItems(menuItems);

        while (true)
        {
            Console.Write("Enter item ID (or 0 to stop adding): ");
            if (!int.TryParse(Console.ReadLine(), out int itemNumber))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }

            if (itemNumber == 0)
            {
                return;
            }

            MenuItemModel? selectedItem = menuItems.FirstOrDefault(item => item.Id == itemNumber);
            if (selectedItem == null)
            {
                Console.WriteLine("Please enter an item ID shown in the menu.");
                continue;
            }

            if (!OrderLogic.CanOrder(UserLogic.CurrentAccount, selectedItem))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must be at least 18 years old to order this item.");
                Console.ResetColor();
                continue;
            }

            Console.Write($"How many of '{selectedItem.Name}' would you like? ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 1)
            {
                Console.WriteLine("Quantity must be at least 1.");
                continue;
            }

            OrderLogic.AddOrUpdateLine(orderLines, selectedItem, quantity);

            Console.WriteLine($"Added {quantity}x {selectedItem.Name} to your order.");

            Console.Write("Add another item? (yes/no): ");
            string? answer = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (answer == "n" || answer == "no")
            {
                return;
            }
        }
    }

    private static void ShowOrder(List<OrderLineModel> orderLines)
    {
        Console.WriteLine();
        Console.WriteLine("=== YOUR CURRENT ORDER ===");

        if (orderLines.Count == 0)
        {
            Console.WriteLine("Your order is currently empty.");
            return;
        }

        foreach (OrderLineModel line in orderLines)
        {
            double lineTotal = line.MenuItem.UnitPrice * line.Quantity;
            Console.WriteLine($"{line.Quantity}x {line.MenuItem.Name,-35} EUR {lineTotal,7:F2}");
        }

        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"Total: EUR {OrderLogic.GetTotal(orderLines):F2}");
    }
}
