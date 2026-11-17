public static class OrderLogic
{
    public static void Save(long reservationId, List<OrderLineModel> orderLines)
    {
        OrderAccess.Save(reservationId, orderLines);
    }

    public static double GetTotal(List<OrderLineModel> orderLines)
    {
        return orderLines.Sum(line => line.MenuItem.UnitPrice * line.Quantity);
    }
}
