using Dapper;

public static class ReservationNoticeAccess
{
    public static void CreateForMenuItem(long menuItemId, string message)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(@"
            INSERT INTO ReservationNotice (ReservationID, MenuItemID, Message, CreatedAt)
            SELECT DISTINCT o.ReservationID, @MenuItemId, @Message, @CreatedAt
            FROM OrderItem oi
            JOIN ""Order"" o ON o.ID = oi.OrderID
            WHERE oi.MenuItemID = @MenuItemId
              AND o.ReservationID IS NOT NULL
              AND NOT EXISTS (
                  SELECT 1 FROM ReservationNotice rn
                  WHERE rn.ReservationID = o.ReservationID AND rn.MenuItemID = @MenuItemId
              )", new
        {
            MenuItemId = menuItemId,
            Message = message,
            CreatedAt = DateTime.UtcNow.ToString("O")
        });
    }

    public static List<ReservationNoticeModel> GetForReservation(long reservationId)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<ReservationNoticeModel>("SELECT ID AS Id, ReservationID AS ReservationId, MENUITEMID AS MenuItemId, Message, CreatedAt FROM ReservationNotice WHERE ReservationID = @ReservationId ORDER BY ID", new { ReservationId = reservationId }).ToList();
    }
}
