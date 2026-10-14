public class ReservationModel : IComparable<ReservationModel>
{
    public int Id { get; private set; }

    public string TimeSlot { get; set; }

    public DateTime Date { get; set; }
    
    public int PartySize { get; set; }
    public int User { get; protected set;}
    public int Table { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }


    public ReservationModel(int id, string timeSlot, DateTime date, int partySize, int user, int table)
    {
        Id = id;
        TimeSlot = timeSlot;
        Date = date;
        PartySize = partySize;
        User = user;    
        Table = table;
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