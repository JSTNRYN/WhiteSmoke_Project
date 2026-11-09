public static class OrderLogic
{
    public static List<OrderLineModel>? AskForOrder()
    {
        List<MenuItemModel> menuItems = MenuItemAccess.LoadAllMenuItems();
        List<OrderLineModel> orderLines = new();

        if (menuItems.Count == 0)
        {
            Console.WriteLine("The menu is currently empty.");
            return null;
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1) View menu");
            Console.WriteLine("2) Make an order");
            Console.WriteLine("3) View my order");
            Console.WriteLine("4) Finish");
            Console.WriteLine("5) Cancel");
            Console.Write("Choice: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": MenuView.ShowMenu(UserLogic.CurrentAccount!); break;
                case "2": MakeOrder(menuItems, orderLines); break;
                case "3": ShowOrder(orderLines); break;
                case "4": return orderLines;
                case "5": return null;
                default: Console.WriteLine("Invalid input."); break;
            }
        }
    }

    private static void MakeOrder(List<MenuItemModel> menuItems, List<OrderLineModel> orderLines)
    {
        Console.WriteLine();
        for (int i = 0; i < menuItems.Count; i++)
        {
            MenuItemModel item = menuItems[i];
            string adultLabel = MenuLogic.IsAdultItem(item) ? " [ADULT ITEM]" : string.Empty;
            Console.WriteLine($"{i + 1,3}) [{item.Category}] {item.Name}{adultLabel} - EUR {item.UnitPrice:F2}");
        }

        while (true)
        {
            Console.Write("Dish number: ");
            if (!int.TryParse(Console.ReadLine(), out int itemNumber)
                || itemNumber < 1
                || itemNumber > menuItems.Count)
            {
                Console.WriteLine("Invalid dish number.");
                continue;
            }

            MenuItemModel selectedItem = menuItems[itemNumber - 1];
            if (MenuLogic.IsAdultItem(selectedItem)
                && !UserLogic.IsAdult(UserLogic.CurrentAccount?.DateOfBirth))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are not old enough to order this adult item.");
                Console.ResetColor();
                continue;
            }

            Console.Write("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 1)
            {
                Console.WriteLine("Quantity must be at least 1.");
                continue;
            }

            OrderLineModel? existingLine = orderLines.FirstOrDefault(line => line.MenuItem.Id == selectedItem.Id);
            if (existingLine == null)
                orderLines.Add(new OrderLineModel(selectedItem, quantity));
            else
                existingLine.Quantity += quantity;

            Console.WriteLine($"Added {quantity}x {selectedItem.Name}.");
            while (true)
            {
                Console.Write("Add another item? (y/n): ");
                string? answer = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (answer is "n" or "no") return;
                if (answer is "y" or "yes") break;
                Console.WriteLine("Please answer y or n.");
            }
        }
    }

    public static double GetTotal(List<OrderLineModel> orderLines)
    {
        return orderLines.Sum(line => line.MenuItem.UnitPrice * line.Quantity);
    }

    private static void ShowOrder(List<OrderLineModel> orderLines)
    {
        Console.WriteLine();
        Console.WriteLine("Your order:");

        if (orderLines.Count == 0)
        {
            Console.WriteLine("Your order is empty.");
            return;
        }

        foreach (OrderLineModel line in orderLines)
        {
            double lineTotal = line.MenuItem.UnitPrice * line.Quantity;
            Console.WriteLine($"{line.Quantity}x {line.MenuItem.Name,-35} EUR {lineTotal,7:F2}");
        }

        Console.WriteLine(new string('-', 55));
        Console.WriteLine($"Total: EUR {GetTotal(orderLines):F2}");
    }
}
