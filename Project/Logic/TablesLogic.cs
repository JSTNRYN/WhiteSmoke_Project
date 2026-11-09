using System;
using System.Linq;

// Hier staan de regels van het restaurant (welke capaciteit mag, welke posities bestaan).
// De logica-laag controleert de invoer en roept daarna TableAccess aan om op te slaan.
// Bij een fout gooit hij een exception met een duidelijke melding; de UI laat die zien.
public static class TableLogic
{
    // Grootte van het raster. 0 t/m 4 = 5 kolommen, 0 t/m 5 = 6 rijen.
    // Staat hier als constante zodat de UI dezelfde waardes gebruikt.
    public const int MaxX = 4;
    public const int MaxY = 5;

    // Toegestane tafelgroottes volgens de regels van White Smoke
    private static readonly int[] AllowedCapacities = { 2, 4, 6 };

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
    // ignoreTableId is voor het verplaatsen: de tafel zelf telt dan niet als "bezette plek",
    // anders zou je een tafel niet op zijn eigen plek kunnen laten staan.
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
    // Ook als de UI al gecontroleerd heeft, doen we het hier nog een keer
    // zodat ongeldige data nooit in de database kan komen.
    public static void AddNewTable(int capacity, int x, int y)
    {
        ValidateCapacity(capacity);
        ValidatePosition(x, y);
        TableAccess.Insert(capacity, x, y);
    }

    // Verplaatst een tafel naar een nieuwe, geldige en vrije positie
    public static void MoveTable(int id, int x, int y)
    {
        // Bestaat de tafel? Zo niet, dan melden we dat met een exception ("table not found")
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

        TableAccess.Delete(table.Id);
    }
}