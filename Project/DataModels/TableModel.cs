public class TableModel
{
    public int Id { get; private set; }

    public int Capacity { get; private set; }


    public TableModel(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
    }
}