public class TableModel
{
    public Int64 Id { get; set; }

    public Int64 Capacity { get; set; }

    public TableModel(Int64 id, Int64 capacity)
    {
        Id = id;
        Capacity = capacity;
    }
}