public static class MyReservation
{
    public static void ShowReservations(long userId)
    {
        while (true)
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            List<ReservationModel> reservations = ReservationLogic.GetWithUserID(userId);
            if (reservations.Count == 0)
            {
                Console.WriteLine("You have no reservations.");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("MY RESERVATIONS");
            Console.WriteLine("===============\n");
            Console.WriteLine("Select a reservation you want to view/edit:\n");

            List<string> options = new List<string>();
            foreach (ReservationModel reservation in reservations)
            {
                options.Add($"Reservation #{reservation.Id} - {reservation.Date}");
            }
            options.Add("Back to main menu");

            int selectedIndex = Menu.InteractiveMenu(options.ToArray());

            if (selectedIndex == options.Count - 1)
            {
                return;
            }

            ReservationModel selectedReservation = reservations[selectedIndex];
            AdminFunctions.EditReservation(selectedReservation);
        }
    }
}