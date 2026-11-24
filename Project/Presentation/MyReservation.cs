public static class MyReservation
{
    public static void ShowReservations(long userId)
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
        Console.WriteLine("===============");

        foreach (ReservationModel reservation in reservations)
        {
            Console.WriteLine($"Reservation #{reservation.Id}");
            Console.WriteLine($"Date: {reservation.Date}");
            Console.WriteLine($"Time: {reservation.TimeSlot}");
            Console.WriteLine($"Guests: {reservation.PartySize}");
            Console.WriteLine($"Table: #{reservation.TableId}");

            List<OrderLineModel> order = ReservationLogic.GetOrder(reservation.Id);
            if (order.Count == 0)
            {
                Console.WriteLine("Order: none");
            }
            else
            {
                Console.WriteLine("Order:");
                foreach (OrderLineModel line in order)
                {
                    double lineTotal = line.MenuItem.UnitPrice * line.Quantity;
                    Console.WriteLine($"  {line.Quantity}x {line.MenuItem.Name} - EUR {lineTotal:F2}");
                }

                double total = OrderLogic.GetTotal(order);
                Console.WriteLine($"Order total: EUR {total:F2}");
            }

            Console.WriteLine(new string('-', 50));
        }

        Console.WriteLine("Press Enter to return...");
        Console.ReadLine();
    }
}
