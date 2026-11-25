public static class MyReservation
{
    public static void ShowReservations(long userId)
    {
        if (!Console.IsOutputRedirected)
            Console.Clear();

        List<ReservationModel> reservations = MyReservationLogic.GetWithUserID(userId);
        if (reservations.Count == 0)
        {
            Console.WriteLine("You have no reservations.");
            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("MY RESERVATIONS");
        Console.WriteLine("===============");
        foreach (ReservationModel reservation in reservations)
        {
            Console.WriteLine($"Reservation #{reservation.Id}");
            Console.WriteLine($"Date: {reservation.Date}");
            Console.WriteLine($"Time: {reservation.TimeSlot}");
            Console.WriteLine($"Guests: {reservation.PartySize}");
            Console.WriteLine($"Table: #{reservation.TableId}");

            List<OrderLineModel> order = MyReservationLogic.GetOrder(reservation.Id);
            if (order.Count == 0)
            {
                Console.WriteLine("Order: none");
            }
            else
            {
                Console.WriteLine("Order:");
                foreach (OrderLineModel line in order)
                    Console.WriteLine($"  {line.Quantity}x {line.MenuItem.Name} - EUR {line.MenuItem.UnitPrice * line.Quantity:F2}");
                Console.WriteLine($"Order total: EUR {OrderLogic.GetTotal(order):F2}");
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
                options.Add($"Reservation #{reservation.Id} - {reservation.Date} ({reservation.TimeSlot}) - {reservation.PartySize}p - Table #{reservation.TableId}");
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
