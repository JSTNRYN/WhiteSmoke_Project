public class MyReservationLogic
{
    public static List<ReservationModel> GetWithUserID(long UserId)
    {
        List<ReservationModel> reservations =
            MyReservationAccess.GetWithUserID(UserId);

        reservations.Sort();

        return reservations;
    }
}