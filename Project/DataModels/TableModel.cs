// Dit model beschrijft één rij uit de "Table"-tabel in de database.
// Dapper vult de eigenschappen automatisch met de kolommen met dezelfde naam.
public class TableModel
{
    public int Id { get; set; }          // Uniek tafelnummer (primary key)
    public int Capacity { get; set; }    // Aantal personen: 2, 4 of 6
    public int? X { get; set; }          // Kolom in het raster (null = nog geen positie)
    public int? Y { get; set; }          // Rij in het raster (null = nog geen positie)
    public bool? IsOccupied { get; set; } // true = bezet, false = vrij

    // Lege constructor: Dapper heeft deze nodig om objecten aan te maken
    public TableModel() { }

    // Handige constructor voor als je zelf snel een tafel met alleen id en capaciteit wilt maken
    public TableModel(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
    }
}