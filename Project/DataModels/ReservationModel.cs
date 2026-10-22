public class ReservationModel : IComparable<ReservationModel>
{
    public Int64 Id { get; set; }
    public string TimeSlot { get; set; }
    public string Date { get; set; }
    public Int64 PartySize { get; set; }
    public Int64 UserId { get; set; }
    public Int64 TableId { get; set; }


    public ReservationModel(Int64 id, string timeSlot, string date, Int64 partySize, Int64 userId, Int64 tableId)
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