public static class MakeReservation
{
    private const string DateFormat = "dd-MM-yyyy";

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("=== MAKE A RESERVATION ===");
        Console.WriteLine("(You can cancel anytime by typing 'cancel')\n");

        string? date = AskDate();
        if (date == null) return;

        string? time = AskTime();
        if (time == null) return;

        int? partySize = AskPartySize();
        if (partySize == null) return;

        List<TableModel> tables = ReservationLogic.GetTablesForReservation(partySize);
        if (tables.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("No tables have enough seats for this party size.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return;
        }

        HashSet<long> reservedTableIds = ReservationLogic.GetReservedTableIds(dateInput, timeInput);
        List<TableModel> availableTables = tables.Where(table => !reservedTableIds.Contains(table.Id)).ToList();

        TableModel? selectedTable = null;
        while (selectedTable == null)
        {
            Console.Clear();
            Console.WriteLine($"Tables for {dateInput} at {timeInput} (party size: {partySize}):");
            FloorPlanUI.DrawReservationFloorPlan(tables, reservedTableIds);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Green = available");
            Console.ResetColor();
            Console.Write("; ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("red = already reserved.");
            Console.ResetColor();
            if (availableTables.Count == 0)
            {
                Console.WriteLine("No tables are available for this date and time.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return;
            }
            Console.Write("Enter the table number: ");
            string tableInput = Console.ReadLine() ?? string.Empty;
            if (dateInput.Equals("cancel", StringComparison.OrdinalIgnoreCase)) { Console.WriteLine(_cancelled); return; }
            if (!int.TryParse(tableInput, out int selectedId))
            {
                PrintCancelled();
                return null;
            }

            if (!ReservationLogic.IsValidDateFormat(input))
            {
                Console.WriteLine($"Invalid date format. Please use {DateFormat}.\n");
                continue;
            }
            selectedTable = availableTables.FirstOrDefault(table => table.Id == selectedId);
            if (selectedTable == null)
            {
                bool isReserved = reservedTableIds.Contains(selectedId);
                Console.WriteLine(isReserved
                    ? "That table is already reserved for this date and time. Choose a green table."
                    : "Table number does not exist or is too small for your party.");
            }
            }

            if (!ReservationLogic.IsValidateDate(input))
            {
                Console.WriteLine("You cannot make a reservation that is in the past or more than a year ahead.\n");
                continue;
            }

            return input;
        }

    private static string? AskTime()
    {
        while (true)
        {
            Console.WriteLine("\nChoose a time slot:");
            Console.WriteLine("1) 5:00 PM");
            Console.WriteLine("2) 9:30 PM");
            Console.Write("Choice: ");

            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (IsCancel(input))
            {
                PrintCancelled();
                return null;
            }

            if (input == "1") return "5:00 PM";
            if (input == "2") return "9:30 PM";

            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
        }
    }

    private static int? AskPartySize()
    {
        while (true)
        {
            Console.Write("\nEnter number of guests (1-6): ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (IsCancel(input))
            {
                PrintCancelled();
                return null;
            }

            if (int.TryParse(input, out int partySize) && SeatCount.WithinRange(partySize))
            {
                return partySize;
            }

            Console.WriteLine("Invalid number of guests. You can only reserve for 1 to 6 guests.");
        }
    }

    private static TableModel? ChooseTable(string date, int partySize, string time)
    {
        List<TableModel> availableTables = ReservationLogic.GetAvailableTables(date, partySize, time);
        if (availableTables.Count == 0)
        {
            Console.WriteLine("\nNo tables are available for this date, time and party size.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return null;
        }

        while (true)
        {
            Console.WriteLine("\nAvailable tables:");
            foreach (TableModel table in availableTables)
            {
                Console.WriteLine($"Table #{table.Id}  |  Seats: {table.Capacity}");
            }

            Console.Write("Enter the table number: ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (IsCancel(input))
            {
                PrintCancelled();
                return null;
            }

            if (int.TryParse(input, out int selectedId))
            {
                foreach (TableModel table in availableTables)
                {
                    if (table.Id == selectedId)
                    {
                        return table;
                    }
                }
            }

            Console.WriteLine("Table number does not exist. Please pick a table from the list.");
        }
    }

    private static void ConfirmAndSave(ReservationModel reservation, List<OrderLineModel> orderLines)
    {
        Console.Clear();
        Console.WriteLine("=== RESERVATION SUMMARY ===");
        Console.WriteLine($"Date:     {reservation.Date}");
        Console.WriteLine($"Time:     {reservation.TimeSlot}");
        Console.WriteLine($"Guests:   {reservation.PartySize}");
        Console.WriteLine($"Table:    #{reservation.TableId}");
        Console.WriteLine($"Order:    EUR {OrderLogic.GetTotal(orderLines):F2}");
        Console.WriteLine("---------------------------");

        while (true)
        {
            Console.Write("Do you want to confirm? (yes/no): ");
            string input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

            if (input == "no" || input == "n")
            {
                PrintCancelled();
                return;
            }

            if (input == "yes" || input == "y")
            {
                long reservationId = ReservationLogic.Write(reservation);
                reservation.Id = reservationId;

                if (orderLines.Count > 0)
                {
                    OrderModel order = new OrderModel(reservationId, orderLines);
                    OrderLogic.Save(order);
                }

                Console.WriteLine($"\nReservation successfully made for {reservation.PartySize} guests on {reservation.Date} at {reservation.TimeSlot}.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Invalid input! Only 'yes' or 'no' are allowed.");
        }
    }

    private static bool IsCancel(string input)
    {
        return input.Equals("cancel", StringComparison.OrdinalIgnoreCase);
    }

    private static void PrintCancelled()
    {
        Console.WriteLine("Process canceled, you can try again.");
    }
}
