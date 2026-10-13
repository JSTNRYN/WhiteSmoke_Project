public static class MakeReservation
{

    public static void Start()
    {
        Console.WriteLine("Enter the date for your reservation (YYYY-MM-DD):");
        string dateInput = Console.ReadLine();
        DateTime date;

        while (!DateTime.TryParse(dateInput, out date))
        {
            Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format:");
            dateInput = Console.ReadLine();
        }

        Console.WriteLine("Enter the time for your reservation (HH:MM):");
        string timeInput = Console.ReadLine();
        TimeSpan time;

        while (!TimeSpan.TryParse(timeInput, out time))
        {
            Console.WriteLine("Invalid time format. Please enter the time in HH:MM format:");
            timeInput = Console.ReadLine();
        }

        Console.WriteLine("Enter the number of guests:");
        string partySizeInput = Console.ReadLine();
        int partySize;

        while (!int.TryParse(partySizeInput, out partySize) || partySize <= 0)
        {
            Console.WriteLine("Invalid number of guests. Please enter a positive integer:");
            partySizeInput = Console.ReadLine();
        }

        Console.WriteLine($"Available tables:");
        List<TableModel> availableTables = ReservationLogic.GetUniqueTables(partySize);
        foreach (var table in availableTables)
        {
            Console.WriteLine($"Table Number: {table.Id}, Capacity: {table.Capacity}");
        }

        Console.WriteLine("Enter the Number of the table you want to reserve:");
        string tableNumberInput = Console.ReadLine();
        int tableNumber;
        while (!int.TryParse(tableNumberInput, out tableNumber) || !availableTables.Any(t => t.Id == tableNumber))
        {
            Console.WriteLine("Invalid table number. Please enter a valid table number from the list above:");
            tableNumberInput = Console.ReadLine();
        }
    


        Console.WriteLine($"Reservation made for {partySize} guests on {date.ToShortDateString()} at {time}.");
    }
}