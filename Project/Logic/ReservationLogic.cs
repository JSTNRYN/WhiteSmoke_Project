using System.Globalization;

public class ReservationLogic
{
    public ReservationModel? currentReservation { get; private set; }


    public static List<ReservationModel> GetWithUserID(long User)
    {
        return ReservationAccess.GetWithUserID(User);
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
        List<long> reservedTableIds = GetReservedTableIds(date, time);
        List<TableModel> availableTables = new List<TableModel>();

        foreach (TableModel table in TableAccess.GetWithMinimumCapacity(partySize))
        {
            if (!reservedTableIds.Contains(table.Id))
            {
                availableTables.Add(table);
            }
        }

        return availableTables;
    }

    public static List<long> GetReservedTableIds(string date, string time)
    {
        List<long> reservedTableIds = new List<long>();

        foreach (var reservation in ReservationAccess.GetAllByDate(date))
        {
            if (reservation.TimeSlot == time)
            {
                reservedTableIds.Add(reservation.TableId);
            }
        }

        return reservedTableIds;
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
}
