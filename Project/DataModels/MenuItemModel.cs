public class MenuItemModel
{
    public int Id { get; set; }
    public string Menu { get; set; }
    public string Name { get; set; }
    public string Category { get; set; } 
    public double UnitPrice { get; set; }
    public string Description { get; set; }

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