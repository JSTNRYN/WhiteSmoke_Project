public class TableModel
{
    public int Id { get; set; }

    public int Capacity { get; set; }

    public TableModel() { }

    public TableModel(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
    }
}