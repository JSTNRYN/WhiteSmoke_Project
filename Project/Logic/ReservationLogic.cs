
public class ReservationLogic
{
    public static List<ReservationModel> GetWithUserID(int User)
    {
        return ReservationAccess.GetWithUserID(User);
    }
}
