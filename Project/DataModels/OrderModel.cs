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
