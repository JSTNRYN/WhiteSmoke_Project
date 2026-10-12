
public class MyReservation
{
    public static void ShowReservations(int User)
    {
        List<ReservationModel> reservations = ReservationLogic.GetWithUserID(User);

        if (reservations.Count == 0)
        {
            Console.WriteLine("You have no reservations.");
            return;
        }

        foreach (ReservationModel reservation in reservations)
        {
            Console.WriteLine($"Date: {reservation.Date}");
            Console.WriteLine($"Time: {reservation.TimeSlot}");
            Console.WriteLine($"Guests: {reservation.PartySize}");
            Console.WriteLine("--------------------");
        }
    }
}
