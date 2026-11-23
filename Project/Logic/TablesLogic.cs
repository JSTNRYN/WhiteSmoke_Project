using System.Collections.Generic;
using System.Linq;

public static class TableLogic
{
    public const int MaxX = 2;   
    public const int MaxY = 5;   

    private static readonly int[] AllowedCapacities = { 2, 4, 6 };

    public static List<TableModel> GetAllTables()
    {
        return TableAccess.GetAll();
    }

    public static void ValidateTableExists(int id)
    {
        if (TableAccess.GetById(id) == null)
            throw new TableException(TableError.NotFound);
    }

    public static void ValidateCapacity(int capacity)
    {
        if (!AllowedCapacities.Contains(capacity))
            throw new TableException(TableError.InvalidCapacity);
    }

    public static void ValidateColumn(int x)
    {
        if (x < 0 || x > MaxX)
            throw new TableException(TableError.InvalidColumn);
    }

    public static void ValidateRow(int y)
    {
        if (y < 0 || y > MaxY)
            throw new TableException(TableError.InvalidRow);
    }

    public static void ValidatePosition(int x, int y, int? ignoreTableId = null)
    {
        ValidateColumn(x);
        ValidateRow(y);

        bool taken = TableAccess.GetAll()
            .Any(t => t.X == x && t.Y == y && t.Id != ignoreTableId);

        if (taken)
            throw new TableException(TableError.PositionTaken);
    }

    public static void AddNewTable(int capacity, int x, int y)
    {
        ValidateCapacity(capacity);
        ValidatePosition(x, y);
        TableAccess.Insert(capacity, x, y);
    }

    public static void MoveTable(int id, int x, int y)
    {
        ValidateTableExists(id);
        ValidatePosition(x, y, ignoreTableId: id);
        TableAccess.UpdateLocation(id, x, y);
    }

    public static void DeleteTable(int id)
    {
        ValidateTableExists(id);

        if (!TableAccess.Delete(id))
            throw new TableException(TableError.HasReservations);
    }
}