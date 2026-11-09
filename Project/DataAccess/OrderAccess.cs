using Dapper;
using Microsoft.Data.Sqlite;

public static class OrderAccess
{
    public static List<OrderLineModel> GetLines(long reservationId)
    {
        const string query = @"
            SELECT MenuItem.ID AS Id, MenuItem.MENU AS Menu, MenuItem.NAME AS Name,
                   MenuItem.CATEGORY AS Category, MenuItem.UNITPRICE AS UnitPrice,
                   MenuItem.DESCRIPTION AS Description, MenuItem.ADULT AS Adult,
                   OrderItem.Quantity
            FROM OrderItem
            JOIN ""Order"" ON ""Order"".ID = OrderItem.OrderID
            JOIN MenuItem ON MenuItem.ID = OrderItem.MenuItemID
            WHERE ""Order"".ReservationID = @ReservationId
            ORDER BY MenuItem.CATEGORY, MenuItem.NAME";

        using var connection = Database.CreateConnection();
        connection.Open();
        List<dynamic> rows = connection.Query(query, new { ReservationId = reservationId }).ToList();
        return rows.Select(row => new OrderLineModel(
            new MenuItemModel(
                checked(Convert.ToInt32(row.Id)),
                Convert.ToString(row.Menu) ?? string.Empty,
                Convert.ToString(row.Name) ?? string.Empty,
                Convert.ToString(row.Category) ?? string.Empty,
                Convert.ToDouble(row.UnitPrice),
                Convert.ToString(row.Description) ?? string.Empty)
            {
                Adult = Convert.ToInt64(row.Adult) != 0
            },
            checked(Convert.ToInt32(row.Quantity)))).ToList();
    }

    public static void Save(long reservationId, List<OrderLineModel> orderLines)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        long orderId = GetOrCreateOrder(connection, transaction, reservationId);

        foreach (OrderLineModel line in orderLines)
        {
            connection.Execute(
                "INSERT INTO OrderItem (OrderID, MenuItemID, Quantity) VALUES (@OrderID, @MenuItemID, @Quantity)",
                new { OrderID = orderId, MenuItemID = line.MenuItem.Id, Quantity = line.Quantity }, transaction);
        }

        RecalculateTotal(connection, transaction, orderId);
        transaction.Commit();
    }

    private static long GetOrCreateOrder(SqliteConnection connection, SqliteTransaction transaction, long reservationId)
    {
        long? existing = FindOrderId(connection, transaction, reservationId);
        if (existing.HasValue) return existing.Value;

        return connection.ExecuteScalar<long>(
            "INSERT INTO \"Order\" (ReservationID, TotalPrice) VALUES (@ReservationID, 0) RETURNING ID",
            new { ReservationID = reservationId }, transaction);
    }

    private static long? FindOrderId(SqliteConnection connection, SqliteTransaction transaction, long reservationId)
    {
        return connection.ExecuteScalar<long?>(
            "SELECT ID FROM \"Order\" WHERE ReservationID = @ReservationId ORDER BY ID LIMIT 1",
            new { ReservationId = reservationId }, transaction);
    }

    private static void RecalculateTotal(SqliteConnection connection, SqliteTransaction transaction, long orderId)
    {
        connection.Execute(@"
            UPDATE ""Order""
            SET TotalPrice = COALESCE((
                SELECT SUM(OrderItem.Quantity * MenuItem.UNITPRICE)
                FROM OrderItem
                JOIN MenuItem ON MenuItem.ID = OrderItem.MenuItemID
                WHERE OrderItem.OrderID = ""Order"".ID
            ), 0)
            WHERE ID = @OrderID", new { OrderID = orderId }, transaction);
    }

}
