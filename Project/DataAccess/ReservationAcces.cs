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

        public static List<ReservationModel> GetAll()
    {
        string query = $"SELECT * FROM {_table} ORDER BY Date, Timeslot";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<ReservationModel>(query).ToList();
    }

    public static ReservationModel? GetById(long id)
    {
        string query = $"SELECT * FROM {_table} WHERE ID = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<ReservationModel>(query, new { Id = id });
    }

    public static void Update(ReservationModel reservation)
    {
        string query = $"UPDATE {_table} SET Timeslot = @TimeSlot, Date = @Date, PartySize = @PartySize, TableID = @TableId WHERE ID = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(query, reservation);
    }

    public static void Delete(long id)
    {
        string query = $"DELETE FROM {_table} WHERE ID = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(query, new { Id = id });
    }

    public static bool DeleteReservationsByUserId(long userId)
    {
        string query = $"DELETE FROM {_table} WHERE UserID = @UserId";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(query, new { UserId = userId });
        return true;
    }
    
}