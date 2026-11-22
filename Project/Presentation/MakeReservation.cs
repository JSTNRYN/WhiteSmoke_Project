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

        TableModel? selectedTable = ChooseTable(date, partySize.Value, time);
        if (selectedTable == null) return;

        List<OrderLineModel>? orderLines = OrderView.AskForOrder();
        if (orderLines == null)
        {
            Console.WriteLine("Reservation cancelled.");
            return;
        }

        ReservationModel reservation = new ReservationModel(0, time, date, partySize.Value, 0, selectedTable.Id);
        ConfirmAndSave(reservation, orderLines);
    }

    private static string? AskDate()
    {
        while (true)
        {
            Console.WriteLine($"Enter the date for your reservation ({DateFormat}):");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (IsCancel(input))
            {
                PrintCancelled();
                return null;
            }

            if (!ReservationLogic.IsValidDateFormat(input))
            {
                Console.WriteLine($"Invalid date format. Please use {DateFormat}.\n");
                continue;
            }

            if (!ReservationLogic.IsValidateDate(input))
            {
                Console.WriteLine("You cannot make a reservation that is in the past or more than a year ahead.\n");
                continue;
            }

            return input;
        }
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
