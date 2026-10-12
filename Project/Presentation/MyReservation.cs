
public class MyReservation
{
    public static void ShowReservations(int User)
    {
        List<ReservationModel> reservations = ReservationLogic.GetWithUserID(User);

        foreach (ReservationModel reservation in reservations)
        {
            Console.WriteLine($"Date: {reservation.Date}");
            Console.WriteLine($"Time: {reservation.TimeSlot}");
            Console.WriteLine($"Guests: {reservation.PartySize}");
            Console.WriteLine("--------------------");
        }
    }
}
