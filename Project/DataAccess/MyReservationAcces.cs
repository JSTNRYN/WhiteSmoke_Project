using Microsoft.Data.Sqlite;
using Dapper;

public class MyReservationAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public static List<ReservationModel> GetWithUserID(long UserId)
    {
        string query = @"
        SELECT Reservation.*, User.FirstName, User.LastName
        FROM Reservation
        JOIN User ON Reservation.User = User.ID
        WHERE Reservation.User = @User";
        List<ReservationModel> MyReservation =
        _connection.Query<ReservationModel>(query, new { User = UserId }).ToList();

        return MyReservation;
    }
    
}