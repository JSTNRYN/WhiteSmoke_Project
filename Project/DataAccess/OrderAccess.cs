using System.Globalization;
using Dapper;
using Microsoft.Data.Sqlite;

public static class OrderAccess
{
    public static List<OrderLineModel> GetLines(long reservationId)
    {
        const string query = @"
            SELECT MenuItem.ID AS Id, MenuItem.MENU AS Menu, MenuItem.NAME AS Name,
                   MenuItem.CATEGORY AS Category, CAST(MenuItem.UNITPRICE AS REAL) AS UnitPrice,
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
        List<OrderLineModel> lines = new List<OrderLineModel>();

        foreach (var row in rows)
        {
            double unitPrice = 0;
            if (row.UnitPrice != null)
            {
                double.TryParse(Convert.ToString(row.UnitPrice), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice);
            }

            MenuItemModel item = new MenuItemModel(
                Convert.ToInt32(row.Id),
                Convert.ToString(row.Menu) ?? string.Empty,
                Convert.ToString(row.Name) ?? string.Empty,
                Convert.ToString(row.Category) ?? string.Empty,
                unitPrice,
                Convert.ToString(row.Description) ?? string.Empty);

            item.Adult = Convert.ToInt64(row.Adult) != 0;
            lines.Add(new OrderLineModel(item, Convert.ToInt32(row.Quantity)));
        }

        return lines;
    }

    public static OrderModel GetOrder(long reservationId)
    {
        List<OrderLineModel> lines = GetLines(reservationId);
        return new OrderModel(reservationId, lines);
    }

    public static void Save(OrderModel order)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        long orderId = GetOrCreateOrder(connection, transaction, order.ReservationId);
        order.Id = orderId;

        foreach (OrderLineModel line in order.Lines)
        {
            connection.Execute(
                "INSERT INTO OrderItem (OrderID, MenuItemID, Quantity) VALUES (@OrderID, @MenuItemID, @Quantity)",
                new { OrderID = orderId, MenuItemID = line.MenuItem.Id, Quantity = line.Quantity },
                transaction);
        }

        UpdateTotal(connection, transaction, orderId);
        transaction.Commit();
    }

    public static void Save(long reservationId, List<OrderLineModel> orderLines)
    {
        Save(new OrderModel(reservationId, orderLines));
    }

    private static long GetOrCreateOrder(SqliteConnection connection, SqliteTransaction transaction, long reservationId)
    {
        long? existing = connection.ExecuteScalar<long?>(
            "SELECT ID FROM \"Order\" WHERE ReservationID = @ReservationId ORDER BY ID LIMIT 1",
            new { ReservationId = reservationId },
            transaction);

        if (existing.HasValue)
        {
            return existing.Value;
        }

        return connection.ExecuteScalar<long>(
            "INSERT INTO \"Order\" (ReservationID, TotalPrice) VALUES (@ReservationID, 0) RETURNING ID",
            new { ReservationID = reservationId },
            transaction);
    }

    private static void UpdateTotal(SqliteConnection connection, SqliteTransaction transaction, long orderId)
    {
        connection.Execute(@"
            UPDATE ""Order""
            SET TotalPrice = COALESCE((
                SELECT SUM(OrderItem.Quantity * MenuItem.UNITPRICE)
                FROM OrderItem
                JOIN MenuItem ON MenuItem.ID = OrderItem.MenuItemID
                WHERE OrderItem.OrderID = ""Order"".ID
            ), 0)
            WHERE ID = @OrderID",
            new { OrderID = orderId },
            transaction);
    }
}
