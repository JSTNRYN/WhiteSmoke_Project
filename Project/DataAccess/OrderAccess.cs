using Dapper;

public static class OrderAccess
{
    public static void Save(long reservationId, List<OrderLineModel> orderLines)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        double totalPrice = 0;
        foreach (OrderLineModel line in orderLines)
        {
            totalPrice += line.MenuItem.UnitPrice * line.Quantity;
        }

        long orderId = connection.ExecuteScalar<long>(
            "INSERT INTO \"Order\" (ReservationID, TotalPrice) VALUES (@ReservationID, @TotalPrice) RETURNING ID",
            new { ReservationID = reservationId, TotalPrice = totalPrice },
            transaction);

        foreach (OrderLineModel line in orderLines)
        {
            connection.Execute(
                "INSERT INTO OrderItem (OrderID, MenuItemID, Quantity) VALUES (@OrderID, @MenuItemID, @Quantity)",
                new
                {
                    OrderID = orderId,
                    MenuItemID = line.MenuItem.Id,
                    Quantity = line.Quantity
                },
                transaction);
        }

        transaction.Commit();
    }
}
