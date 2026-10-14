using Microsoft.Data.Sqlite;
using Dapper;

public static class ReservationAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private static string _table = "Reservation";

    public static List<ReservationModel> GetWithUserID(int User)
    {
        string query = @"
        SELECT Reservation.*, User.FirstName, User.LastName
        FROM Reservation
        JOIN User ON Reservation.User = User.ID
        WHERE Reservation.User = @User";
        List<ReservationModel> MyReservation =
        _connection.Query<ReservationModel>(query, new { User = User }).ToList();

        return MyReservation;
    }

    public static List<ReservationModel> GetAllByDate(DateTime date)
    {
        string query = $"SELECT * FROM {_table} WHERE Date = @date";
        List<ReservationModel> reservations = _connection.Query<ReservationModel>(query, new { Date = date}).ToList();

        return reservations;
    }
}