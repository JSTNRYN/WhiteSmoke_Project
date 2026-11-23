public static class OrderLogic
{
    public static double GetTotal(OrderModel order)
    {
        return GetTotal(order.Lines);
    }

    public static double GetTotal(List<OrderLineModel> orderLines)
    {
        double total = 0;
        foreach (OrderLineModel line in orderLines)
        {
            total += line.MenuItem.UnitPrice * line.Quantity;
        }
        return total;
    }

    public static void Save(OrderModel order)
    {
        OrderAccess.Save(order);
    }

    public static void Save(long reservationId, List<OrderLineModel> orderLines)
    {
        OrderAccess.Save(new OrderModel(reservationId, orderLines));
    }

    public static List<OrderLineModel> GetOrder(long reservationId)
    {
        return OrderAccess.GetLines(reservationId);
    }

    public static bool CanOrder(UserModel? user, MenuItemModel item)
    {
        if (MenuLogic.IsAdultItem(item))
        {
            return UserLogic.IsAdult(user?.DateOfBirth);
        }
        return true;
    }

    public static void AddOrUpdateLine(List<OrderLineModel> lines, MenuItemModel item, int quantity)
    {
        OrderLineModel? existing = null;
        foreach (OrderLineModel line in lines)
        {
            if (line.MenuItem.Id == item.Id)
            {
                existing = line;
                break;
            }
        }

        if (existing == null)
        {
            lines.Add(new OrderLineModel(item, quantity));
        }
        else
        {
            existing.Quantity += quantity;
        }
    }
}
