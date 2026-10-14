using Microsoft.Data.Sqlite;
using Dapper;

public class MyReservationAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public static List<ReservationModel> GetWithUserID(long UserId)
    {
        string query = @"
        SELECT Reservation.*
        FROM Reservation
        JOIN User ON Reservation.UserID = User.ID
        WHERE Reservation.UserID = @UserId";
        List<ReservationModel> MyReservation =
        _connection.Query<ReservationModel>(query, new { UserId }).ToList();

        return MyReservation;
    }
    
}