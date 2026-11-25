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
}
