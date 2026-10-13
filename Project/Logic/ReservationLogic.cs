
public class ReservationLogic
{
    TableAccess TableAccess = new TableAccess();
    public static List<ReservationModel> GetWithUserID(int User)
    {
        return ReservationAccess.GetWithUserID(User);
    }

    //NOT FINISHED YET
    public static List<TableModel> GetUniqueTables(int partySize)
    {
        List<TableModel> availableTables = TableAccess.GetWithMinimumCapacity(partySize);
        return availableTables;
    }

}
