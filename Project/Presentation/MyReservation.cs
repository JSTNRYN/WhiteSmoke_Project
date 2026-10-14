public static class MyReservation
{
    public static void ShowReservations(long UserId)
    {
        List<ReservationModel> reservations =
            ReservationLogic.GetWithUserID(UserId);

        if (reservations.Count == 0)
        {
            Console.WriteLine("You have no reservations.");
            Console.WriteLine("Would you like to make a reservation? (y/n)");

            string choice = Console.ReadLine()?.ToLower();

            if (choice == "y")
            {
                MakeReservation.Start();
            }

            return;
        }

        foreach (ReservationModel reservation in reservations)
        {
            Console.WriteLine($"Name: {reservation.FirstName} {reservation.LastName}");
            Console.WriteLine($"Date: {reservation.Date:dd-MM-yyyy}");
            Console.WriteLine($"Time: {reservation.TimeSlot}");
            Console.WriteLine($"Guests: {reservation.PartySize}");
            Console.WriteLine($"Table: {reservation.Table}");
            Console.WriteLine("--------------------");
        }
    }
}