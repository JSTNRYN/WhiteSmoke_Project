public class ReservationLogic
{
    public static List<ReservationModel> GetWithUserID(int User)
    {
        List<ReservationModel> reservations =
            ReservationAccess.GetWithUserID(User);

        reservations.Sort();

        return reservations;
    }
}