using System;
using System.Collections.Generic;
using System.Linq;

// Alles wat de admin ziet en intypt. Hier staat geen SQL en geen businessregel:
// de UI vraagt invoer, laat TableLogic controleren en toont het resultaat.
// De UI praat nooit rechtstreeks met TableAccess.
public static class FloorPlanUI
{
    // Breedtes van de plattegrond (samen 74 tekens, net als je ontwerp)
    private const int FrameWidth = 74;   // Hele kader, inclusief de buitenste "|"
    private const int LeftWidth = 28;    // Breedte van de vakken links (bar, hot seats, toiletten)
    private const int InnerWidth = 36;   // Binnenbreedte van het vak TABLES & DINING
    private const int LabelWidth = 3;    // Ruimte voor het rijlabel (Y0, Y1, ...)

    // De vaste linkerkant van de plattegrond: elke string is één regel.
    // Een lege string is een lege regel tussen twee vakken.
    private static readonly string[] LeftColumn =
    {
        "+--------------------------+",
        "|           BAR            |",
        "|                          |",
        "|   (B1) (B2) (B3) (B4)    |",
        "|   (B5) (B6) (B7) (B8)    |",
        "|     (8 Bar Stools)       |",
        "+--------------------------+",
        "",
        "+--------------------------+",
        "|        HOT SEATS         |",
        "|    (H1)  (H2)  (H3)      |",
        "|                          |",
        "|  (H7)  +--------+  (H9)  |",
        "|  (H8)  |KITCHEN |  (H10) |",
        "|        |Prep &  |        |",
        "|        |Cooking |        |",
        "|        +--------+        |",
        "|                          |",
        "|    (H4)  (H5)  (H6)      |",
        "+--------------------------+",
        "",
        "+--------------------------+",
        "|        RESTROOMS         |",
        "|[Women] [Men] [Accessible]|",
        "+--------------------------+"
    };

    // Hoofdscherm van de plattegrond. Blijft herhalen tot de admin 0 kiest.
    public static void Show()
    {
        if (!UserLogic.IsLoggedIn() || !UserLogic.IsAdmin())
        {
            Console.WriteLine("Only logged-in administrators can access the floor plan.");
            return;
        }

        while (true)
        {
            Console.Clear();
            DrawFloorPlan();
            Console.WriteLine();
            Console.WriteLine("1. Add table");
            Console.WriteLine("2. Move table");
            Console.WriteLine("3. Delete table");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Choice: ");

            switch (Console.ReadLine())
            {
                case "1": AddTableMenu(); break;
                case "2": MoveTableMenu(); break;
                case "3": DeleteTableMenu(); break;
                case "0":
                    Console.Clear();
                    return;   // Terug naar het hoofdmenu
                default:
                    PrintError("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    // Tekent de hele plattegrond: kader, kop, linkerkant (vast) en rechterkant (uit de database)
    public static void DrawFloorPlan()
    {
        string frame = new string('=', FrameWidth);
        string empty = "|" + new string(' ', FrameWidth - 2) + "|";

        // Kop van het restaurant
        Console.WriteLine(frame);
        Console.WriteLine("|" + CenterText("WHITE SMOKE", FrameWidth - 2) + "|");
        Console.WriteLine("|" + CenterText("Wijnhaven 107, 3011 WN Rotterdam", FrameWidth - 2) + "|");
        Console.WriteLine(frame);
        Console.WriteLine("|" + " [ ENTRANCE / RECEPTION ]".PadRight(FrameWidth - 2) + "|");
        Console.WriteLine(empty);

        // De rechterkant wordt opgebouwd uit de database
        List<string> right = BuildDiningBox();

        // Links en rechts naast elkaar zetten, regel voor regel.
        // Is een kant korter, dan wordt de rest aangevuld met spaties.
        int lines = Math.Max(LeftColumn.Length, right.Count);
        for (int i = 0; i < lines; i++)
        {
            string left = i < LeftColumn.Length ? LeftColumn[i] : "";
            string rightPart = i < right.Count ? right[i] : "";

            // "| " + links (28) + 4 spaties + rechts (38) + " |" = 74 tekens
            Console.WriteLine("| " + left.PadRight(LeftWidth) + "    "
                              + rightPart.PadRight(InnerWidth + 2) + " |");
        }

        Console.WriteLine(frame);
        Console.WriteLine("* = occupied");
    }

    // Bouwt het vak TABLES & DINING als lijst van regels, op basis van de tafels in de database
    private static List<string> BuildDiningBox()
    {
        int cols = TableLogic.MaxX + 1;
        int rows = TableLogic.MaxY + 1;

        // Breedte per kolom: de ruimte naast het rijlabel, eerlijk verdeeld over de kolommen
        int cellWidth = (InnerWidth - LabelWidth) / cols;

        // Tafels ophalen via de logica-laag (niet direct via TableAccess)
        var tables = TableLogic.GetAllTables();

        string border = "+" + new string('-', InnerWidth) + "+";
        string blank = "|" + new string(' ', InnerWidth) + "|";

        var box = new List<string>();
        box.Add(border);
        box.Add("|" + CenterText("TABLES & DINING", InnerWidth) + "|");

        // Kopregel met de kolomnummers, zodat de admin weet welke X hij moet intypen
        string header = new string(' ', LabelWidth);
        for (int x = 0; x < cols; x++)
            header += CenterText($"X{x}", cellWidth);
        box.Add("|" + header.PadRight(InnerWidth) + "|");

        for (int y = 0; y < rows; y++)
        {
            // Elke rij begint met het rijlabel (Y0, Y1, ...)
            string row = $"Y{y}".PadRight(LabelWidth);

            for (int x = 0; x < cols; x++)
            {
                // Zoek de tafel op deze plek (null als er geen is)
                var t = tables.FirstOrDefault(t => t.X == x && t.Y == y);

                // Lege plek = lege cel. Anders bijvoorbeeld [T3: 4p] of [T3: 4p*] als bezet.
                string text = t == null
                    ? ""
                    : $"[T{t.Id}: {t.Capacity}p{(t.IsOccupied == true ? "*" : "")}]";

                row += CenterText(text, cellWidth);
            }

            box.Add("|" + row.PadRight(InnerWidth) + "|");

            // Lege regel tussen de rijen, behalve na de laatste
            if (y < rows - 1) box.Add(blank);
        }

        box.Add(border);
        return box;
    }

    // ---------- Helpers ----------

    // Zet tekst in het midden van een vaste breedte (aanvullen met spaties)
    private static string CenterText(string text, int width)
    {
        if (text.Length >= width) return text.Substring(0, width);

        int left = (width - text.Length) / 2;
        int right = width - text.Length - left;
        return new string(' ', left) + text + new string(' ', right);
    }

    // Print een foutmelding in het rood en zet de kleur daarna terug
    private static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    // Vraagt een getal en blijft vragen tot het geldig is.
    // "validate" is de controle die per vraag anders is.
    // Geeft het getal terug, of null als de admin q typt om te annuleren.
    private static int? ReadValidated(string prompt, Action<int> validate)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();

            if (input != null && input.ToLower() == "q") return null;

            if (!int.TryParse(input, out int value))
            {
                PrintError("Invalid input. Please enter a number.");
                continue;
            }

            try
            {
                validate(value);   // Gooit een ArgumentException als de waarde niet mag
                return value;
            }
            catch (ArgumentException ex)
            {
                PrintError(ex.Message);   // Rode melding, daarna opnieuw vragen
            }
        }
    }

    // Vraagt kolom en rij en controleert meteen of die plek vrij is.
    // Bij een fout (bijvoorbeeld bezet) worden kolom en rij opnieuw gevraagd.
    // ignoreTableId: bij verplaatsen telt de tafel zelf niet als bezette plek.
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
                TableLogic.ValidatePosition(x.Value, y.Value, ignoreTableId);
                return (x.Value, y.Value);
            }
            catch (ArgumentException ex)
            {
                PrintError(ex.Message);
            }
        }
    }

    // Wacht op een toets zodat meldingen leesbaar blijven voordat het scherm wordt gewist
    private static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }

    // ---------- Menu's ----------

    // Voegt een tafel toe: capaciteit, positie, opslaan
    private static void AddTableMenu()
    {
        Console.WriteLine("(type q to cancel)");

        int? capacity = ReadValidated("Capacity (2, 4 or 6): ", TableLogic.ValidateCapacity);
        if (capacity == null) return;

        var position = ReadFreePosition();
        if (position == null) return;

        try
        {
            TableLogic.AddNewTable(capacity.Value, position.Value.X, position.Value.Y);
            Console.WriteLine("Table added.");
        }
        catch (ArgumentException ex)
        {
            PrintError(ex.Message);
        }
        Pause();
    }

    // Verplaatst een tafel: welke tafel, nieuwe positie, opslaan
    private static void MoveTableMenu()
    {
        Console.WriteLine("(type q to cancel)");

        // Het tafelnummer wordt direct gecontroleerd door de logica-laag
        int? id = ReadValidated("Table number to move: ", TableLogic.ValidateTableExists);
        if (id == null) return;

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

        int? id = ReadValidated("Table number to delete: ", TableLogic.ValidateTableExists);
        if (id == null) return;

        // Alleen y of j verwijdert, alles anders annuleert
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
            // Niets verwijderen: de tafel blijft ongewijzigd
            Console.WriteLine("Deletion cancelled. The table remains unchanged.");
        }
        Pause();
    }
}
