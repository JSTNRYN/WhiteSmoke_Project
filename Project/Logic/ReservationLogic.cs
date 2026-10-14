using System.Globalization;

public class ReservationLogic
{
    public ReservationModel? currentReservation { get; private set; }


    public static List<ReservationModel> GetWithUserID(int User)
    {
        return ReservationAccess.GetWithUserID(User);
    }

    public static List<TableModel> GetAvailableTables(DateTime date, int partySize)
    {
        List<ReservationModel> reservations = ReservationAccess.GetAllByDate(date);
        List<TableModel> tables = TableAccess.GetWithMinimumCapacity(partySize);

        HashSet<int> reservedTableIds = new HashSet<int>();
        foreach (ReservationModel reservation in reservations)
        {
            reservedTableIds.Add(reservation.TableId);
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

        return parsedDate.Date >= currentDate;
    }

    public static bool IsValidDateFormat(string date)
    {
        if (date == null || date == "")
        {
            return false;
        }

        string[] dateFormats = ["dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy"];

        return DateTime.TryParseExact(date, dateFormats, new CultureInfo("nl-NL"), DateTimeStyles.None, out _);
    }
}