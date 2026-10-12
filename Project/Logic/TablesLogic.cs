public class TableLogic
{
    public static TableModel currentTable { get; private set; }

    private TableAccess _access = new();

    // @TODO: Validate if table capacity adheres to business rules when the admin adds a new table to the database
}