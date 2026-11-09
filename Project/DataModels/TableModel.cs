public class TableModel
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public int? X { get; set; }
    public int? Y { get; set; }
    public bool? IsOccupied { get; set; }

    public TableModel() { }

    public TableModel(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
    }
}