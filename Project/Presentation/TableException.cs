
public class TableException : Exception
{
    public TableError Error { get; }

    public TableException(TableError error)
    {
        Error = error;
    }
}