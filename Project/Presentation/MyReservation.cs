
public static class MyReservation
{
    public static void ShowReservations(int UserId)
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
            Console.WriteLine($"Date: {reservation.Date:dd-MM-yyyy}");
            Console.WriteLine($"Time: {reservation.TimeSlot}");
            Console.WriteLine("Type S to see more, or press Enter to skip.");

            string choice = Console.ReadLine()?.ToLower();

            if (choice == "s")
            {

                Console.WriteLine($"Guests: {reservation.PartySize}");
                //Console.WriteLine($"Table: {reservation.Table}");
            }

            Console.WriteLine("--------------------");
        }
    }
}
