using Dapper;

public static class ReservationAccess
{
    private static string _table = "Reservation";

    public static long Write(ReservationModel reservation)
    {
        string query = $"INSERT INTO {_table} (Timeslot, Date, PartySize, UserID, TableID) VALUES (@TimeSlot, @Date, @PartySize, @UserId, @TableId) RETURNING ID";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<long>(query, reservation);
    }

    public static long Write(UserModel user, long partySize, string date, string timeslot, long table)
    {
        ReservationModel reservation = new ReservationModel(0, timeslot, date, partySize, user.Id, table);
        return Write(reservation);
    }

    public static List<ReservationModel> GetWithUserID(long userId)
    {
        string query = @"
            SELECT Reservation.*, User.FirstName, User.LastName
            FROM Reservation
            JOIN User ON Reservation.UserID = User.ID
            WHERE Reservation.UserID = @UserId";

        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<ReservationModel>(query, new { UserId = userId }).ToList();
    }

    public static List<ReservationModel> GetAllByDate(string date)
    {
        string query = $"SELECT * FROM {_table} WHERE Date = @Date";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<ReservationModel>(query, new { Date = date }).ToList();
    }
}
