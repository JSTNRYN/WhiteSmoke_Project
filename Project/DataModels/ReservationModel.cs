public class ReservationModel
{
    public int Id { get; private set; }

    public string TimeSlot { get; set; }

    public DateTime Date { get; set; }
    
    public int PartySize { get; set; }
    public int User { get; protected set;}
    public int Table { get; private set; }


    public ReservationModel(int id, string timeSlot, DateTime date, int partySize, int user, int table)
    {
        Id = id;
        TimeSlot = timeSlot;
        Date = date;
        PartySize = partySize;
        User = user;
        Table = table;
    }
}