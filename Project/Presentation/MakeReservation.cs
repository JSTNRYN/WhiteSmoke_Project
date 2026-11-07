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
        if (dateInput.ToLower() == "cancel")
        {
            Console.Clear();
            Console.WriteLine(_cancelled);
            return;
        }

        // Validates if it is in the right format
        while (!ReservationLogic.IsValidDateFormat(dateInput))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid date format. Please enter the date in {_dateFormat} format:");
            Console.ResetColor();

            Console.WriteLine($"Enter the date for your reservation ({_dateFormat}):");
            dateInput = Console.ReadLine() ?? string.Empty;
        }

        // Validates if its not set in the past
        while (!ReservationLogic.IsValidateDate(dateInput))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You cannot make a reservation that is in the past!");
            Console.ResetColor();

            Console.WriteLine($"Enter the date for your reservation ({_dateFormat}):");
            dateInput = Console.ReadLine() ?? string.Empty;
        }

        // Validates if the picked option is 1 or 2
        Console.Clear();
        Console.WriteLine("Enter the time for your reservation");
        Console.WriteLine("1) 5:00 PM");
        Console.WriteLine("2) 9:30 PM");
        Console.WriteLine(_toCancel);
        string timeInput = Console.ReadLine() ?? string.Empty;
        if (timeInput.ToLower() == "cancel")
        {
            Console.Clear();
            Console.WriteLine(_cancelled);
            return;
        }

        while (timeInput != "1" && timeInput != "2")
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can only pick the mentioned options!");
            Console.ResetColor();

            Console.WriteLine("Pick one of the available times");
            Console.WriteLine("1) 5:00 PM");
            Console.WriteLine("2) 9:30 PM");
            timeInput = Console.ReadLine() ?? string.Empty;
        }

        if (timeInput == "1")
        {
            timeInput = "5:00 PM";
        }
        else if (timeInput == "2")
        {
            timeInput = "9:30 PM";
        }

        Console.Clear();
        Console.WriteLine("Enter the number of guests (1-6):");
        Console.WriteLine(_toCancel);
        string partySizeInput = Console.ReadLine() ?? string.Empty;
        if (partySizeInput.ToLower() == "cancel")
        {
            Console.Clear();
            Console.WriteLine(_cancelled);
            return;
        }

        int partySize;
        if (partySizeInput != "")
        {
            partySize = Int32.Parse(partySizeInput);
        }
        else
        {
            partySize = 0;
        }

        while (!SeatCount.WithinRange(partySize))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid number of guests. You can only have between 1-6:");
            Console.ResetColor();

            Console.WriteLine("Enter the number of guests (1-6):");
            partySizeInput = Console.ReadLine() ?? string.Empty;
            if (partySizeInput != "")
            {
                partySize = Int32.Parse(partySizeInput);
            }
            else
            {
                partySize = 0;
            }
        }

        TableModel? selectedTable = null;
        
        List<TableModel>? availableTables = ReservationLogic.GetAvailableTables(dateInput, partySize, timeInput);
        if (availableTables.Count == 0)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No tables are available for this date, time, and party size.");
            Console.ResetColor();
            return;
        }

        while (selectedTable == null)
        {
            Console.Clear();
            Console.WriteLine("Select a available table:");

            foreach (TableModel table in availableTables)
            {
                Console.WriteLine($"# {table.Id}");
                Console.WriteLine($"Seats: {table.Capacity}");
                Console.WriteLine("-------------------\n");
            }
            string tableInput = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(tableInput, out int selectedId))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must pick table number.");
                Console.ResetColor();
                continue;
            }

            selectedTable = availableTables.FirstOrDefault(table => table.Id == selectedId);

            if (selectedTable == null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Table number does not exist.");
                Console.ResetColor();
            }
        }


        Conformation(partySize, dateInput, timeInput, selectedTable.Id);
    }

    public static void Conformation(Int64 partySize, string dateInput, string timeInput, Int64 table)
    {
        Console.Clear();
        Console.WriteLine("Your reservation:\n");
        Console.WriteLine($"Date: {dateInput}");
        Console.WriteLine($"Time: {timeInput}");
        Console.WriteLine($"People attending: {partySize}");
        Console.WriteLine($"Table: #{table}\n");
        Console.WriteLine($"Do you want to confirm? (Yes/No)");
        string userInput = Console.ReadLine() ?? string.Empty;

        while (userInput.ToLower() is not ("yes" or "y" or "no" or "n"))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input! only Yes/No are allowed.");
            Console.ResetColor();

            Console.WriteLine($"Do you want to confirm? (Yes/No)");
            userInput = Console.ReadLine() ?? string.Empty;
        }

        if (userInput.ToLower() is "no" or "n")
        {
            Console.Clear();
            Console.WriteLine(_cancelled);
            return;
        }

        if (userInput.ToLower() is "yes" or "y")
        {
            ReservationLogic.Write(partySize, dateInput, timeInput, table);
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Reservation made for {partySize} guests on {dateInput} at {timeInput}.");
        Console.ResetColor();
    }
}