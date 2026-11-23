public class OrderModel
{
    public long Id { get; set; }
    public long ReservationId { get; set; }
    public List<OrderLineModel> Lines { get; set; } = new List<OrderLineModel>();
    public double TotalPrice { get; set; }

    public OrderModel()
    {
    }

    public OrderModel(long reservationId, List<OrderLineModel> lines)
    {
        ReservationId = reservationId;
        Lines = lines;
    }
}

public class OrderLineModel
{
    public MenuItemModel MenuItem { get; set; }
    public int Quantity { get; set; }

    public OrderLineModel(MenuItemModel menuItem, int quantity)
    {
        MenuItem = menuItem;
        Quantity = quantity;
    }
}
