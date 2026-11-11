using System;
using System.Collections.Generic;
using System.Linq;

// Hier staan de regels van het restaurant. De logica-laag controleert de invoer
// en roept daarna TableAccess aan. Bij een fout gooit hij een exception met een
// duidelijke melding; de UI laat die zien.
public static class TableLogic
{

 public const int MaxX = 2;   // kolommen 0-2 (3 kolommen)
public const int MaxY = 5;   // rijen 0-5

    // Toegestane tafelgroottes volgens de regels van White Smoke
    private static readonly int[] AllowedCapacities = { 2, 4, 6 };

    // Geeft alle tafels door aan de UI, zodat de UI niet zelf bij de database hoeft
    public static List<TableModel> GetAllTables()
    {
        return TableAccess.GetAll();
    }

    // Controleert of een tafel bestaat (voor de directe foutmelding in de UI)
    public static void ValidateTableExists(int id)
    {
        if (TableAccess.GetById(id) == null)
            throw new ArgumentException($"Table {id} could not be found.");
    }

    // Controleert of de capaciteit 2, 4 of 6 is
    public static void ValidateCapacity(int capacity)
    {
        if (!AllowedCapacities.Contains(capacity))
            throw new ArgumentException(
                "Invalid table capacity. A table must have a capacity of 2, 4, or 6 persons.");
    }

    // Controleert of de kolom binnen het raster valt
    public static void ValidateColumn(int x)
    {
        if (x < 0 || x > MaxX)
            throw new ArgumentException($"Invalid column. X must be between 0 and {MaxX}.");
    }

    // Controleert of de rij binnen het raster valt
    public static void ValidateRow(int y)
    {
        if (y < 0 || y > MaxY)
            throw new ArgumentException($"Invalid row. Y must be between 0 and {MaxY}.");
    }

    // Controleert een complete positie: binnen het raster én nog vrij.
    // ignoreTableId is voor het verplaatsen: de tafel zelf telt dan niet als "bezette plek".
    public static void ValidatePosition(int x, int y, int? ignoreTableId = null)
    {
        ValidateColumn(x);
        ValidateRow(y);

        // Staat er al een andere tafel op deze plek?
        bool taken = TableAccess.GetAll()
            .Any(t => t.X == x && t.Y == y && t.Id != ignoreTableId);

        if (taken)
            throw new ArgumentException($"Position ({x}, {y}) is already occupied by another table.");
    }

    // Voegt een tafel toe nadat alles gecontroleerd is.
    // Dubbele controle zodat ongeldige data nooit in de database kan komen.
    public static void AddNewTable(int capacity, int x, int y)
    {
        ValidateCapacity(capacity);
        ValidatePosition(x, y);
        TableAccess.Insert(capacity, x, y);
    }

    // Verplaatst een tafel naar een nieuwe, geldige en vrije positie
    public static void MoveTable(int id, int x, int y)
    {
        var table = TableAccess.GetById(id)
            ?? throw new InvalidOperationException($"Table {id} could not be found.");

        ValidatePosition(x, y, ignoreTableId: table.Id);
        TableAccess.UpdateLocation(id, x, y);
    }

    // Verwijdert een tafel. De bevestiging ("weet je het zeker?") gebeurt in de UI.
    public static void DeleteTable(int id)
    {
        var table = TableAccess.GetById(id)
            ?? throw new InvalidOperationException($"Table {id} could not be found.");

        if (!TableAccess.Delete(id))
            throw new InvalidOperationException(
                $"Table {id} can't be deleted because it still has reservations.");
    }
}
