public class ReservationModel : IComparable<ReservationModel>
{
    public int Id { get; set; }
    public string TimeSlot { get; set; }
    public DateTime Date { get; set; }
    public int PartySize { get; set; }
    public int UserId { get; set; }
    public int TableId { get; set; }


    public ReservationModel(int id, string timeSlot, DateTime date, int partySize, int userId, int tableId)
    {
        Id = id;
        TimeSlot = timeSlot;
        Date = date;
        PartySize = partySize;
        UserId = userId;
        TableId = tableId;
    }

    public int CompareTo(ReservationModel? other)
    {
        if (other == null)
        {
            return 1;
        }
        int dateComparison = Date.CompareTo(other.Date);

        if (dateComparison != 0)
        {
            return dateComparison;
        }
        return TimeSlot.CompareTo(other.TimeSlot);

    }
}