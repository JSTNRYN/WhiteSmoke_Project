using Microsoft.Data.Sqlite;
using Dapper;

public class ReservationAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string _reservation = "Reservation";


    public static List<ReservationModel> GetWithUserID(int User)
    {
        string query = $"SELECT * FROM Reservation WHERE User = @User";
        List<ReservationModel> MyReservation = _connection.Query<ReservationModel>(query, new { User = User }).ToList();
        return MyReservation;
        
    }
}