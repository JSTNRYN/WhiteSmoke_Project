using System.Globalization;

public class ReservationLogic
{
    public static long Write(ReservationModel reservation)
    {
        if (UserLogic.CurrentAccount == null)
        {
            throw new InvalidOperationException("You must be logged in to make a reservation.");
        }

        if (!IsValidDateFormat(reservation.Date) || !IsValidateDate(reservation.Date))
        {
            throw new ArgumentException("Invalid reservation date.");
        }

        if (!SeatCount.WithinRange((int)reservation.PartySize))
        {
            throw new ArgumentException("Party size must be between 1 and 6.");
        }

        reservation.UserId = UserLogic.CurrentAccount.Id;
        return ReservationAccess.Write(reservation);
    }

    public static List<ReservationModel> GetWithUserID(long userId)
    {
        List<ReservationModel> reservations = ReservationAccess.GetWithUserID(userId);
        reservations.Sort();
        return reservations;
    }

    public static List<OrderLineModel> GetOrder(long reservationId)
    {
        return OrderAccess.GetLines(reservationId);
    }

    public static List<TableModel> GetAvailableTables(string date, int partySize, string time)
    {
        List<ReservationModel> reservations = ReservationAccess.GetAllByDate(date);
        List<TableModel> tables = TableAccess.GetWithMinimumCapacity(partySize);

        List<long> reservedTableIds = new List<long>();
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
        if (string.IsNullOrWhiteSpace(date))
        {
            return false;
        }

        if (!DateTime.TryParse(date, new CultureInfo("nl-NL"), DateTimeStyles.None, out DateTime parsedDate))
        {
            return false;
        }

        DateTime currentDate = DateTime.Today;
        DateTime oneYearAhead = currentDate.AddDays(365);

        return parsedDate.Date >= currentDate && parsedDate.Date <= oneYearAhead;
    }

    public static bool IsValidDateFormat(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
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

    public static bool Update(ReservationModel reservation)
    {
        ReservationModel? existing = ReservationAccess.GetById(reservation.Id);
        if (existing == null)
        {
            return false;
        }

        if (!IsValidDateFormat(reservation.Date) || !IsValidateDate(reservation.Date))
        {
            return false;
        }

        if (!SeatCount.WithinRange((int)reservation.PartySize))
        {
            return false;
        }

        ReservationAccess.Update(reservation);
        return true;
    }

    public static bool Delete(long id)
    {
        ReservationModel? existing = ReservationAccess.GetById(id);
        if (existing == null)
        {
            return false;
        }

        ReservationAccess.Delete(id);
        return true;
    }

    public static bool DeleteReservationsByUserId(long userId)
    {
        List<ReservationModel> reservations = GetWithUserID(userId);

        foreach (ReservationModel reservation in reservations)
        {
            OrderAccess.DeleteOrderItemsByReservationId(reservation.Id);
            OrderAccess.DeleteOrdersByReservationId(reservation.Id);
        }

        return ReservationAccess.DeleteReservationsByUserId(userId);
    }
}