using System;
using System.Linq;

public static class TableLogic
{
    public const int MaxX = 4;   // kolommen 0-4
    public const int MaxY = 5;   // rijen 0-5

    private static readonly int[] AllowedCapacities = { 2, 4, 6 };

    public static void ValidateCapacity(int capacity)
    {
        if (!AllowedCapacities.Contains(capacity))
            throw new ArgumentException(
                "Invalid table capacity. A table must have a capacity of 2, 4, or 6 persons.");
    }

    public static void ValidateColumn(int x)
    {
        if (x < 0 || x > MaxX)
            throw new ArgumentException($"Invalid column. X must be between 0 and {MaxX}.");
    }

    public static void ValidateRow(int y)
    {
        if (y < 0 || y > MaxY)
            throw new ArgumentException($"Invalid row. Y must be between 0 and {MaxY}.");
    }

    public static void ValidatePosition(int x, int y, int? ignoreTableId = null)
    {
        ValidateColumn(x);
        ValidateRow(y);

        bool taken = TableAccess.GetAll()
            .Any(t => t.X == x && t.Y == y && t.Id != ignoreTableId);

        if (taken)
            throw new ArgumentException($"Position ({x}, {y}) is already occupied by another table.");
    }

    public static void AddNewTable(int capacity, int x, int y)
    {
        ValidateCapacity(capacity);
        ValidatePosition(x, y);
        TableAccess.Insert(capacity, x, y);
    }

    public static void MoveTable(int id, int x, int y)
    {
        var table = TableAccess.GetById(id)
            ?? throw new InvalidOperationException($"Table {id} could not be found.");

        ValidatePosition(x, y, ignoreTableId: table.Id);
        TableAccess.UpdateLocation(id, x, y);
    }

    public static void DeleteTable(int id)
    {
        var table = TableAccess.GetById(id)
            ?? throw new InvalidOperationException($"Table {id} could not be found.");

        TableAccess.Delete(table.Id);
    }
}