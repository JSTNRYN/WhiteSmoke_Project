using System.Globalization;

public class ReservationLogic
{
    public ReservationModel? currentReservation { get; private set; }

    public static List<ReservationModel> GetWithUserID(long User)
    {
        return ReservationAccess.GetWithUserID(User);
    }

    public static long Write(ReservationModel reservation)
    {
        if (UserLogic.CurrentAccount is null)
        {
            throw new InvalidOperationException("You must be logged in to make a reservation.");
        }

        return ReservationAccess.Write(
            UserLogic.CurrentAccount,
            reservation.PartySize,
            reservation.Date,
            reservation.TimeSlot,
            reservation.TableId);
    }

    public static List<TableModel> GetTablesForReservation(int partySize)
    {
        ReservationModel reservation = new ReservationModel
        {
            PartySize = partySize
        };
        return GetTablesForReservation(reservation);
    }

    public static List<TableModel> GetTablesForReservation(ReservationModel reservation)
    {
        return TableAccess.GetWithMinimumCapacity((int)reservation.PartySize);
    }

    public static List<TableModel> GetAvailableTables(ReservationModel reservation)
    {
        List<long> reservedTableIds = GetReservedTableIds(reservation);
        List<TableModel> availableTables = new List<TableModel>();

        foreach (TableModel table in TableAccess.GetWithMinimumCapacity((int)reservation.PartySize))
        {
            if (!reservedTableIds.Contains(table.Id))
            {
                availableTables.Add(table);
            }
        }

        return availableTables;
    }

    public static List<long> GetReservedTableIds(ReservationModel reservation)
    {
        List<long> reservedTableIds = new List<long>();

        foreach (var existing in ReservationAccess.GetAllByDate(reservation.Date))
        {
            if (existing.TimeSlot == reservation.TimeSlot)
            {
                reservedTableIds.Add(existing.TableId);
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
