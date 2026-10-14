public class ReservationLogic
{
    public static List<ReservationModel> GetWithUserID(int User)
    {
        List<ReservationModel> reservations =
            MyReservationAccess.GetWithUserID(User);

        reservations.Sort();

        return reservations;
    }
}