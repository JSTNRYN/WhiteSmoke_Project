public static class OrderLogic
{
    public static List<OrderLineModel>? AskForOrder()
    {
        List<MenuItemModel> menuItems = MenuItemAccess.LoadAllMenuItems();
        List<OrderLineModel> orderLines = new List<OrderLineModel>();

        while (true)
        {
            Console.WriteLine("\n1) Menu bekijken");
            Console.WriteLine("2) Gerecht bestellen");
            Console.WriteLine("3) Bestelling afronden");
            Console.WriteLine("4) Reservering annuleren");
            Console.Write("Kies een optie: ");
            string input = Console.ReadLine() ?? string.Empty;

            if (input == "1")
            {
                MenuView.ShowMenu(UserLogic.CurrentAccount!);
            }
            else if (input == "2")
            {
                AddItem(menuItems, orderLines);
            }
            else if (input == "3")
            {
                ShowOrder(orderLines);
                return orderLines;
            }
            else if (input == "4")
            {
                return null;
            }
            else
            {
                Console.WriteLine("Ongeldige keuze.");
            }
        }
    }

    private static void AddItem(List<MenuItemModel> menuItems, List<OrderLineModel> orderLines)
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            MenuItemModel item = menuItems[i];
            Console.WriteLine($"{i + 1}) {item.Name} - €{item.UnitPrice:F2}");
        }

        Console.Write("Kies het nummer van een gerecht: ");
        if (!int.TryParse(Console.ReadLine(), out int itemNumber) || itemNumber < 1 || itemNumber > menuItems.Count)
        {
            Console.WriteLine("Ongeldig gerecht.");
            return;
        }

        Console.Write("Hoeveel wil je bestellen? ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 1)
        {
            Console.WriteLine("Ongeldige hoeveelheid.");
            return;
        }

        MenuItemModel selectedItem = menuItems[itemNumber - 1];
        OrderLineModel? existingLine = orderLines.FirstOrDefault(line => line.MenuItem.Id == selectedItem.Id);

        if (existingLine == null)
        {
            orderLines.Add(new OrderLineModel(selectedItem, quantity));
        }
        else
        {
            existingLine.Quantity += quantity;
        }

        Console.WriteLine("Gerecht toegevoegd aan de bestelling.");
    }

    public static double GetTotal(List<OrderLineModel> orderLines)
    {
        double total = 0;
        foreach (OrderLineModel line in orderLines)
        {
            total += line.MenuItem.UnitPrice * line.Quantity;
        }

        return total;
    }

    private static void ShowOrder(List<OrderLineModel> orderLines)
    {
        Console.WriteLine("\nJouw bestelling:");
        foreach (OrderLineModel line in orderLines)
        {
            Console.WriteLine($"{line.Quantity}x {line.MenuItem.Name} - €{line.MenuItem.UnitPrice * line.Quantity:F2}");
        }

        Console.WriteLine($"Totaal: €{GetTotal(orderLines):F2}");
    }
}
