public static class MakeReservation
{
    private static readonly string _dateFormat = "dd-MM-yyyy";

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine($"Enter the date for your reservation ({_dateFormat}):");
        string? dateInput = Console.ReadLine();

        // Validates if it is in the right format
        while (!ReservationLogic.IsValidDateFormat(dateInput))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid date format. Please enter the date in {_dateFormat} format:");
            Console.ResetColor();

            Console.WriteLine($"Enter the date for your reservation ({_dateFormat}):");
            dateInput = Console.ReadLine();
        }

        // Validates if its not set in the past
        while (!ReservationLogic.IsValidateDate(dateInput))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You cannot make a reservation that is in the past!");
            Console.ResetColor();

            Console.WriteLine($"Enter the date for your reservation ({_dateFormat}):");
            dateInput = Console.ReadLine();
        }

        // Validates if the picked option is 1 or 2
        Console.WriteLine("Enter the time for your reservation");
        Console.WriteLine("1) 5:00 PM");
        Console.WriteLine("2) 9:30 PM");
        string? timeInput = Console.ReadLine();

        while (timeInput is not ("1" or "2"))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can only pick the mentioned options!");
            Console.ResetColor();

            Console.WriteLine("Pick one of the available times");
            Console.WriteLine("1) 5:00 PM");
            Console.WriteLine("2) 9:30 PM");
            timeInput = Console.ReadLine();
        }

        if (timeInput == "1")
        {
            timeInput = "5:00 PM";
        }
        else if (timeInput == "2")
        {
            timeInput = "9:30 PM";
        }

        Console.WriteLine("Enter the number of guests (1-6):");
        string? partySizeInput = Console.ReadLine();

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
            partySizeInput = Console.ReadLine();
            if (partySizeInput != "")
            {
                partySize = Int32.Parse(partySizeInput);
            }
            else
            {
                partySize = 0;
            }
        }

        // @TODO: get available tables for that day
        TableModel? selectedTable = null;

        while (selectedTable == null)
        {
            Console.WriteLine("Select a available table:");
            List<TableModel> availableTables = ReservationLogic.GetAvailableTables(DateTime.Parse(dateInput), partySize);
            foreach (TableModel table in availableTables)
            {
                Console.WriteLine($"# {table.Id}");
                Console.WriteLine($"Seats: {table.Id}");
                Console.WriteLine("-------------------\n");
            }
            string? tableInput = Console.ReadLine();

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

    public static void Conformation(int partySize, string dateInput, string timeInput, int table)
    {
        Console.Clear();
        Console.WriteLine("Your reservation:\n");
        Console.WriteLine($"Date: {dateInput}");
        Console.WriteLine($"Time: {timeInput}");
        Console.WriteLine($"People attending: {partySize}");
        Console.WriteLine($"Table: #{table}\n");
        Console.WriteLine($"Do you want to confirm? (Yes/No)");
        string? userInput = Console.ReadLine();
        bool retry = true;

        if (userInput.ToLower() == "yes" || userInput.ToLower() == "y")
        {
            retry = false;
            // @TODO: Add to database
        }
        else if (userInput.ToLower() == "no" || userInput.ToLower() == "n")
        {
            retry = false;
        }

        while (retry)
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input! only Yes/No are allowed.");
            Console.ResetColor();

            Console.WriteLine($"Do you want to confirm? (Yes/No)");
            userInput = Console.ReadLine();
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Reservation made for {partySize} guests on {dateInput} at {timeInput}.");
        Console.ResetColor();
    }
}