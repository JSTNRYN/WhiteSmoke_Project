public class ReservationLogic
{
    public static List<ReservationModel> GetWithUserID(long UserId)
    {
        List<ReservationModel> reservations =
            MyReservationAccess.GetWithUserID(UserId);

        reservations.Sort();

        return reservations;
    }
}