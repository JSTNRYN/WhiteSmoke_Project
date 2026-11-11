using Microsoft.Data.Sqlite;
using Dapper;

public class MyReservationAccess
{
    public static List<ReservationModel> GetWithUserID(long UserId)
    {
        string query = @"
        SELECT *
        FROM Reservation
        WHERE UserID = @UserId";
        
        using var connection = Database.CreateConnection();
        connection.Open();
        List<ReservationModel> MyReservation =
            connection.Query<ReservationModel>(query, new { UserId }).ToList();

        return MyReservation;
    }
}
