using System.Globalization;

public class ReservationLogic
{
    public ReservationModel? currentReservation { get; private set; }


    public static List<ReservationModel> GetWithUserID(long User)
    {
        List<ReservationModel> reservations = ReservationAccess.GetWithUserID(User);
        reservations.Sort();
        return reservations;
    }

    public static List<OrderLineModel> GetOrder(long reservationId)
    {
        return OrderAccess.GetLines(reservationId);
    }

    public static long Write(Int64 partySize, string dateInput, string timeInput, Int64 table)
    {
        if (UserLogic.CurrentAccount is null)
        {
            throw new InvalidOperationException("You must be logged in to make a reservation.");
        }

        return ReservationAccess.Write(UserLogic.CurrentAccount, partySize, dateInput, timeInput, table);
    }

    public static List<TableModel> GetAvailableTables(string date, int partySize, string time)
    {
        List<ReservationModel> reservations = ReservationAccess.GetAllByDate(date);
        List<TableModel> tables = TableAccess.GetWithMinimumCapacity(partySize);

        HashSet<long> reservedTableIds = new HashSet<long>();
        foreach (ReservationModel reservation in reservations)
        {
            if (reservation.TimeSlot == time)
            {
                reservedTableIds.Add(reservation.TableId);
            }
        }

        List<TableModel> availableTables = new List<TableModel>();
        foreach (TableModel table in tables)
        {
            if (!reservedTableIds.Contains(table.Id))
            {
                availableTables.Add(table);
            }
        }

        return availableTables;
    }

    public static bool IsValidateDate(string date)
    {
        if (date == null || date == "")
        {
            return false;
        }

        if (!DateTime.TryParse(date, new CultureInfo("nl-NL"), DateTimeStyles.None, out DateTime parsedDate))
        {
            return false;
        }

        DateTime currentDate = DateTime.Today;
        DateTime OneYearAhead = currentDate.AddDays(365);

        return parsedDate.Date >= currentDate && parsedDate.Date <= OneYearAhead;
    }

    public static bool IsValidDateFormat(string date)
    {
        if (date == null || date == "")
        {
            return false;
        }

        return DateTime.TryParseExact(date, "dd-MM-yyyy", new CultureInfo("nl-NL"), DateTimeStyles.None, out _);
    }

    public static List<ReservationModel> GetAll()
    {
        return ReservationAccess.GetAll(); 
    }

    public static ReservationModel? GetById(long id)
    {
        return ReservationAccess.GetById(id);
    }

    public static bool Delete(long id)
    {
        ReservationModel? reservation = ReservationAccess.GetById(id);
        if (reservation == null)
        {
            return false;
        }

        ReservationAccess.Delete(id);
        return true;
    }

    public static bool Update(long id, string date, string timeSlot, int partySize, long tableId)
    {
        ReservationModel? reservation = ReservationAccess.GetById(id);
        if (reservation == null)
        {
            return false;
        }

        if (!IsValidDateFormat(date) || !IsValidateDate(date) || partySize < 1)
        {
            return false;
        }

        reservation.Date = date;
        reservation.TimeSlot = timeSlot;
        reservation.PartySize = partySize;
        reservation.TableId = tableId;

        ReservationAccess.Update(reservation);
        return true;
    }

    
}
