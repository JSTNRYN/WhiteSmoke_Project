using Microsoft.Data.Sqlite;
using Dapper;

public static class ReservationAccess
{
    private static string _table = "Reservation";


    public static long Write(UserModel user, Int64 partySize, string date, string timeslot, Int64 table)
    {
        string query = $"INSERT INTO {_table} (Timeslot, Date, PartySize, UserID, TableID) VALUES (@Timeslot, @Date, @PartySize, @UserID, @TableID) RETURNING ID";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<long>(query, new
        {
            Timeslot = timeslot,
            Date = date,
            PartySize = partySize,
            UserID = user.Id,
            TableID = table
        });
    }

    public static List<ReservationModel> GetWithUserID(long User)
    {
        string query = @"
        SELECT Reservation.*, User.FirstName, User.LastName
        FROM Reservation
        JOIN User ON Reservation.UserID = User.ID
        WHERE Reservation.UserID = @User";
        using var connection = Database.CreateConnection();
        connection.Open();
        List<ReservationModel> MyReservation =
            connection.Query<ReservationModel>(query, new { User = User }).ToList();

        return MyReservation;
    }

    public static List<ReservationModel> GetAllByDate(string date)
    {
        string query = $"SELECT * FROM {_table} WHERE Date = @Date";
        using var connection = Database.CreateConnection();
        connection.Open();
        List<ReservationModel> reservations = connection.Query<ReservationModel>(query, new { Date = date }).ToList();

        return reservations;
    }

    public static List<AdminReservationModel> GetByMenuItemId(long menuItemId)
    {
        const string query = @"
            SELECT DISTINCT
                Reservation.ID AS ReservationId,
                Reservation.Date,
                Reservation.Timeslot AS TimeSlot,
                Reservation.PartySize,
                Reservation.UserID AS UserId,
                User.FirstName,
                User.LastName,
                User.Email
            FROM OrderItem
            JOIN ""Order"" ON ""Order"".ID = OrderItem.OrderID
            JOIN Reservation ON Reservation.ID = ""Order"".ReservationID
            JOIN User ON User.ID = Reservation.UserID
            WHERE OrderItem.MenuItemID = @MenuItemId
            ORDER BY Reservation.Date, Reservation.Timeslot, Reservation.ID";

        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<AdminReservationModel>(query, new { MenuItemId = menuItemId }).ToList();
    }

}
