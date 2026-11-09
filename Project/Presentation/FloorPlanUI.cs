using System;
using System.Linq;

public static class FloorPlanUI
{
    // Hoofdscherm van de plattegrond. Blijft herhalen tot de admin 0 kiest.
    public static void Show()
    {
        while (true)
        {
            Console.Clear();        // Scherm leegmaken zodat de plattegrond vers wordt getekend
            DrawBarArea();          // Vaste tekening van de bar
            Console.WriteLine();
            DrawDiningArea();       // Tafels uit de database
            Console.WriteLine();
            Console.WriteLine("1. Add table");
            Console.WriteLine("2. Move table");
            Console.WriteLine("3. Delete table");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Choice: ");

            // Doe wat de admin kiest
            switch (Console.ReadLine())
            {
                case "1": AddTableMenu(); break;
                case "2": MoveTableMenu(); break;
                case "3": DeleteTableMenu(); break;
                case "0": return;   // Verlaat dit scherm: terug naar het hoofdmenu
                default:
                    PrintError("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    // Tekent de bar. Dit is vaste tekst: de krukken staan niet in de database.
    public static void DrawBarArea()
    {
        Console.WriteLine("BAR");
        Console.WriteLine("+--------------------------+");
        Console.WriteLine("| (B1) (B2) (B3) (B4)      |");
        Console.WriteLine("| (B5) (B6) (B7) (B8)      |");
        Console.WriteLine("| (8 Bar Stools)           |");
        Console.WriteLine("+--------------------------+");
    }

    // Tekent het eetgedeelte op basis van de tafels in de database
    public static void DrawDiningArea()
    {
        // Aantal kolommen en rijen komt uit TableLogic, zodat ze altijd overeenkomen
        const int Cols = TableLogic.MaxX + 1;
        const int Rows = TableLogic.MaxY + 1;
        const int CellWidth = 14;   // Breedte per cel, zodat de kolommen netjes onder elkaar staan

        var tables = TableAccess.GetAll();

        Console.WriteLine("TABLES & DINING (X = column, Y = row)");
        Console.WriteLine(new string('-', Cols * CellWidth));

        // Loop door elke rij (y) en daarbinnen door elke kolom (x)
        for (int y = 0; y < Rows; y++)
        {
            string line = "";
            for (int x = 0; x < Cols; x++)
            {
                // Zoek de tafel die op deze plek staat (null als er geen is)
                var t = tables.FirstOrDefault(t => t.X == x && t.Y == y);

                // Lege plek = lege cel. Anders bijvoorbeeld [T3: 4p] of [T3: 4p*] als hij bezet is.
                string cell = t == null
                    ? ""
                    : $"[T{t.Id}: {t.Capacity}p{(t.IsOccupied == true ? "*" : "")}]";

                // PadRight vult aan met spaties tot CellWidth
                line += cell.PadRight(CellWidth);
            }
            Console.WriteLine(line);
        }

        Console.WriteLine(new string('-', Cols * CellWidth));
        Console.WriteLine("* = occupied");
    }

    // ---------- Helpers ----------

    // Print een foutmelding in het rood en zet de kleur daarna terug
    private static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    // Vraagt een getal en blijft vragen tot het geldig is.
    // "validate" is de controle die per vraag anders is (capaciteit, kolom, rij, tafelnummer).
    // Geeft het getal terug, of null als de admin q typt om te annuleren.
    private static int? ReadValidated(string prompt, Action<int> validate)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();

            // q = annuleren: de aanroeper gaat dan terug naar het menu
            if (input != null && input.ToLower() == "q") return null;

            // Is het wel een getal? Zo niet: rode melding en opnieuw vragen
            if (!int.TryParse(input, out int value))
            {
                PrintError("Invalid input. Please enter a number.");
                continue;
            }

            try
            {
                validate(value);   // Gooit een ArgumentException als de waarde niet mag
                return value;      // Geen fout: waarde is geldig
            }
            catch (ArgumentException ex)
            {
                PrintError(ex.Message);   // Rode melding, daarna begint de lus opnieuw
            }
        }
    }

    // Vraagt kolom en rij en controleert meteen of die plek vrij is.
    // Is de plek bezet, dan worden kolom en rij opnieuw gevraagd.
    // ignoreTableId: bij verplaatsen mag een tafel zijn eigen plek behouden.
    // Geeft null terug als de admin annuleert.
    private static (int X, int Y)? ReadFreePosition(int? ignoreTableId = null)
    {
        while (true)
        {
            int? x = ReadValidated($"Column (X, 0-{TableLogic.MaxX}): ", TableLogic.ValidateColumn);
            if (x == null) return null;

            int? y = ReadValidated($"Row (Y, 0-{TableLogic.MaxY}): ", TableLogic.ValidateRow);
            if (y == null) return null;

            try
            {
                // Controleert raster én of de plek vrij is
                TableLogic.ValidatePosition(x.Value, y.Value, ignoreTableId);
                return (x.Value, y.Value);
            }
            catch (ArgumentException ex)
            {
                PrintError(ex.Message);   // Bijvoorbeeld "bezet": vraag opnieuw
            }
        }
    }

    // Wacht tot de admin een toets indrukt, zodat meldingen leesbaar blijven
    // voordat het scherm in Show() weer wordt leeggemaakt
    private static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }

    // ---------- Menu's ----------

    // Voegt een tafel toe: eerst capaciteit, dan positie, dan opslaan
    private static void AddTableMenu()
    {
        Console.WriteLine("(type q to cancel)");

        int? capacity = ReadValidated("Capacity (2, 4 or 6): ", TableLogic.ValidateCapacity);
        if (capacity == null) return;   // Geannuleerd

        var position = ReadFreePosition();
        if (position == null) return;   // Geannuleerd

        try
        {
            // Opslaan in de database via de logica-laag
            TableLogic.AddNewTable(capacity.Value, position.Value.X, position.Value.Y);
            Console.WriteLine("Table added.");
        }
        catch (ArgumentException ex)
        {
            PrintError(ex.Message);   // Vangnet voor het geval er toch iets misgaat
        }
        Pause();
    }

    // Verplaatst een tafel: eerst welke tafel, dan nieuwe positie, dan opslaan
    private static void MoveTableMenu()
    {
        Console.WriteLine("(type q to cancel)");

        // Het tafelnummer wordt direct gecontroleerd: bestaat de tafel?
        int? id = ReadValidated("Table number to move: ", tableId =>
        {
            if (TableAccess.GetById(tableId) == null)
                throw new ArgumentException($"Table {tableId} could not be found.");
        });
        if (id == null) return;

        // De tafel zelf telt niet mee als "bezette plek"
        var position = ReadFreePosition(ignoreTableId: id.Value);
        if (position == null) return;

        try
        {
            TableLogic.MoveTable(id.Value, position.Value.X, position.Value.Y);
            Console.WriteLine("Table moved.");
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
        {
            PrintError(ex.Message);
        }
        Pause();
    }

    // Verwijdert een tafel, maar pas na bevestiging
    private static void DeleteTableMenu()
    {
        Console.WriteLine("(type q to cancel)");

        // Controleer direct of de tafel bestaat
        int? id = ReadValidated("Table number to delete: ", tableId =>
        {
            if (TableAccess.GetById(tableId) == null)
                throw new ArgumentException($"Table {tableId} could not be found.");
        });
        if (id == null) return;

        // Bevestiging: alleen y of j verwijdert, alles anders annuleert
        Console.Write($"Are you sure you want to delete table {id.Value}? (y/n): ");
        string? answer = Console.ReadLine()?.Trim().ToLower();

        if (answer == "y" || answer == "j")
        {
            try
            {
                TableLogic.DeleteTable(id.Value);
                Console.WriteLine("Table deleted.");
            }
            catch (InvalidOperationException ex)
            {
                PrintError(ex.Message);
            }
        }
        else
        {
            // Niets verwijderen: de tafel blijft ongewijzigd op de plattegrond
            Console.WriteLine("Deletion cancelled. The table remains unchanged.");
        }
        Pause();
    }
}