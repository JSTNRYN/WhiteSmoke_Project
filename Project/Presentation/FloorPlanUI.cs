using System;
using System.Linq;

public static class FloorPlanUI
{
    public static void Show()
    {
        while (true)
        {
            Console.Clear();
            DrawBarArea();
            Console.WriteLine();
            DrawDiningArea();
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
                case "0": return;
                default:
                    PrintError("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    public static void DrawBarArea()
    {
        Console.WriteLine("BAR");
        Console.WriteLine("+--------------------------+");
        Console.WriteLine("| (B1) (B2) (B3) (B4)      |");
        Console.WriteLine("| (B5) (B6) (B7) (B8)      |");
        Console.WriteLine("| (8 Bar Stools)           |");
        Console.WriteLine("+--------------------------+");
    }

    public static void DrawDiningArea()
    {
        const int Cols = TableLogic.MaxX + 1;
        const int Rows = TableLogic.MaxY + 1;
        const int CellWidth = 14;

        var tables = TableAccess.GetAll();

        Console.WriteLine("TABLES & DINING (X = column, Y = row)");
        Console.WriteLine(new string('-', Cols * CellWidth));

        for (int y = 0; y < Rows; y++)
        {
            string line = "";
            for (int x = 0; x < Cols; x++)
            {
                var t = tables.FirstOrDefault(t => t.X == x && t.Y == y);
                string cell = t == null
                    ? ""
                    : $"[T{t.Id}: {t.Capacity}p{(t.IsOccupied == true ? "*" : "")}]";
                line += cell.PadRight(CellWidth);
            }
            Console.WriteLine(line);
        }

        Console.WriteLine(new string('-', Cols * CellWidth));
        Console.WriteLine("* = occupied");
    }

    // ---------- Helpers ----------

    private static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    // Blijft vragen tot de invoer geldig is. Typ q om te annuleren: geeft null terug.
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
                validate(value);
                return value;
            }
            catch (ArgumentException ex)
            {
                PrintError(ex.Message);
            }
        }
    }

    // Vraagt kolom en rij en controleert direct of de positie vrij is.
    // Geeft null terug als de admin annuleert met q.
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
                PrintError(ex.Message);   // bijvoorbeeld: positie bezet, opnieuw vragen
            }
        }
    }

    private static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }

    // ---------- Menu's ----------

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

    private static void MoveTableMenu()
    {
        Console.WriteLine("(type q to cancel)");

        int? id = ReadValidated("Table number to move: ", tableId =>
        {
            if (TableAccess.GetById(tableId) == null)
                throw new ArgumentException($"Table {tableId} could not be found.");
        });
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

    private static void DeleteTableMenu()
    {
        Console.WriteLine("(type q to cancel)");

        int? id = ReadValidated("Table number to delete: ", tableId =>
        {
            if (TableAccess.GetById(tableId) == null)
                throw new ArgumentException($"Table {tableId} could not be found.");
        });
        if (id == null) return;

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
            Console.WriteLine("Deletion cancelled. The table remains unchanged.");
        }
        Pause();
    }
}