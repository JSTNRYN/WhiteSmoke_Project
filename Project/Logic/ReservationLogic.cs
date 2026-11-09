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
            throw new InvalidOperationException("Je moet ingelogd zijn om een reservering te maken.");
        }

        return ReservationAccess.Write(UserLogic.CurrentAccount, partySize, dateInput, timeInput, table);
    }

    public static List<TableModel> GetAvailableTables(string date, int partySize, string time)
    {
        List<ReservationModel> reservations = ReservationAccess.GetAllByDate(date);
        List<TableModel> tables = TableAccess.GetWithMinimumCapacity(partySize);

        HashSet<long> reservedTableIds = reservations
            .Where(reservation => reservation.TimeSlot == time)
            .Select(reservation => reservation.TableId)
            .ToHashSet();

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

        return parsedDate.Date >= currentDate;
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
