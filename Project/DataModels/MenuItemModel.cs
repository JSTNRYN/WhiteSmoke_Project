public class MenuItemModel
{
    public int Id { get; set; }
    public string Menu { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double UnitPrice { get; set; }
    public string Description { get; set; } = string.Empty;

    public MenuItemModel()
    {
    }

    public MenuItemModel(int id, string menu, string name, string category, double unitPrice, string description)
    {
        Id = id;
        Menu = menu;
        Name = name;
        Category = category;
        UnitPrice = unitPrice;
        Description = description;
    }
}
