public static class MakeReservation
{
    private static readonly string _dateFormat = "dd-MM-yyyy";
    private static readonly string _toCancel = "You can cancel by entering 'cancel'.";
    private static readonly string _cancelled = "Process canceled, you can try again.";

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine($"Enter the date for your reservation ({_dateFormat}):");
        Console.WriteLine(_toCancel);
        string dateInput = Console.ReadLine() ?? string.Empty;
        if (dateInput.Equals("cancel", StringComparison.OrdinalIgnoreCase)) { Console.WriteLine(_cancelled); return; }

        while (!ReservationLogic.IsValidDateFormat(dateInput))
        {
            Console.Clear();
            Console.WriteLine($"Invalid date format. Please enter the date in {_dateFormat} format:");
            dateInput = Console.ReadLine() ?? string.Empty;
        }
        while (!ReservationLogic.IsValidateDate(dateInput))
        {
            Console.Clear();
            Console.WriteLine("You cannot make a reservation that is in the past!");
            Console.WriteLine($"Enter the date for your reservation ({_dateFormat}):");
            dateInput = Console.ReadLine() ?? string.Empty;
        }

        Console.Clear();
        Console.WriteLine("Enter the time for your reservation");
        Console.WriteLine("1) 5:00 PM");
        Console.WriteLine("2) 9:30 PM");
        Console.WriteLine(_toCancel);
        string timeInput = Console.ReadLine() ?? string.Empty;
        if (timeInput.Equals("cancel", StringComparison.OrdinalIgnoreCase)) { Console.WriteLine(_cancelled); return; }
        while (timeInput is not ("1" or "2"))
        {
            Console.WriteLine("You can only pick the mentioned options!");
            timeInput = Console.ReadLine() ?? string.Empty;
        }
        timeInput = timeInput == "1" ? "5:00 PM" : "9:30 PM";

        Console.Clear();
        Console.WriteLine("Enter the number of guests (1-6):");
        Console.WriteLine(_toCancel);
        string partySizeInput = Console.ReadLine() ?? string.Empty;
        if (partySizeInput.Equals("cancel", StringComparison.OrdinalIgnoreCase)) { Console.WriteLine(_cancelled); return; }
        int.TryParse(partySizeInput, out int partySize);
        while (!SeatCount.WithinRange(partySize))
        {
            Console.WriteLine("Invalid number of guests. You can only have between 1-6:");
            partySizeInput = Console.ReadLine() ?? string.Empty;
            int.TryParse(partySizeInput, out partySize);
        }

        List<TableModel> availableTables = ReservationLogic.GetAvailableTables(dateInput, partySize, timeInput);
        if (availableTables.Count == 0) { Console.WriteLine("No tables are available for this date, time, and party size."); return; }

        TableModel? selectedTable = null;
        while (selectedTable == null)
        {
            Console.Clear();
            Console.WriteLine("Available tables:");
            foreach (TableModel table in availableTables) Console.WriteLine($"Table #{table.Id}  |  Seats: {table.Capacity}");
            Console.Write("Enter the table number: ");
            string tableInput = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(tableInput, out int selectedId))
            {
                Console.WriteLine("You must pick a table number.");
                continue;
            }
            selectedTable = availableTables.FirstOrDefault(table => table.Id == selectedId);
            if (selectedTable == null) Console.WriteLine("Table number does not exist.");
        }

        List<OrderLineModel>? orderLines = OrderLogic.AskForOrder();
        if (orderLines == null) { Console.WriteLine("Reservering geannuleerd."); return; }
        Conformation(partySize, dateInput, timeInput, selectedTable.Id, orderLines);
    }

    public static void Conformation(long partySize, string dateInput, string timeInput, long table, List<OrderLineModel> orderLines)
    {
        Console.Clear();
        Console.WriteLine($"Your reservation:\nDate: {dateInput}\nTime: {timeInput}\nPeople attending: {partySize}\nTable: #{table}");
        Console.WriteLine($"Order total: EUR {OrderLogic.GetTotal(orderLines):F2}");
        Console.WriteLine("Do you want to confirm? (Yes/No)");
        string input = Console.ReadLine() ?? string.Empty;
        while (input.ToLowerInvariant() is not ("yes" or "y" or "no" or "n"))
        {
            Console.WriteLine("Wrong input! only Yes/No are allowed.");
            input = Console.ReadLine() ?? string.Empty;
        }
        if (input.ToLowerInvariant() is "no" or "n") { Console.WriteLine(_cancelled); return; }
        SaveReservation(partySize, dateInput, timeInput, table, orderLines);
        Console.WriteLine($"Reservation made for {partySize} guests on {dateInput} at {timeInput}.");
    }

    private static void SaveReservation(long partySize, string dateInput, string timeInput, long table, List<OrderLineModel> orderLines)
    {
        long reservationId = ReservationLogic.Write(partySize, dateInput, timeInput, table);
        if (orderLines.Count > 0) OrderAccess.Save(reservationId, orderLines);
    }
}
