// Menukeuzes als enum (Enum)
public enum MenuChoice
{
    Back = 0,
    Add = 1,
    Move = 2,
    Delete = 3
}

public static class TextHelper
{
    // Geeft een string met n spaties
    public static string Spaces(int n)
    {
        string result = "";
        for (int i = 0; i < n; i++)
        {
            result += " ";
        }
        return result;
    }

    // Tekst links uitgelijnd rechts aangevuld met spaties
    public static string FillRight(string text, int width)
    {
        if (text.Length >= width) return text.Substring(0, width);
        return text + Spaces(width - text.Length);
    }

    // Tekst in het midden links en rechts aangevuld met spaties
    public static string Middle(string text, int width)
    {
        if (text.Length >= width) return text.Substring(0, width);

        int spacesLeft = (width - text.Length) / 2;
        int spacesRight = width - text.Length - spacesLeft;
        return Spaces(spacesLeft) + text + Spaces(spacesRight);
    }
}

// Contract voor alles wat in de plattegrond getekend kan worden (Interface)
public interface IFloorSection
{
    List<string> Draw();
}

// Gedeelde basis voor alle vakken (Abstract class + Inheritance)
public abstract class FloorSection : IFloorSection
{
    public abstract List<string> Draw();
}

// Vaste vakken: bar, hot seats, toiletten (Concrete class)
public class StaticSection : FloorSection
{
    private readonly string[] _lines;

    public StaticSection(string[] lines)
    {
        _lines = lines;
    }

    public override List<string> Draw()
    {
        return new List<string>(_lines);
    }
}

// Het vak TABLES & DINING, opgebouwd uit de database
public class DiningSection : FloorSection
{
    private const int LabelWidth = 3;

    // Property met private set (Properties)
    public int InnerWidth { get; private set; }

    public DiningSection(int innerWidth)
    {
        InnerWidth = innerWidth;
    }

    public override List<string> Draw()
    {
        int cols = TableLogic.MaxX + 1;
        int rows = TableLogic.MaxY + 1;
        int cellWidth = (InnerWidth - LabelWidth) / cols;

        // 2D array: één keer vullen, daarna direct opzoeken via grid[x, y]
        TableModel?[,] grid = new TableModel?[cols, rows];
        foreach (var table in TableLogic.GetAllTables())
        {
            // Tafels zonder positie kunnen niet in het grid, die slaan we over
            if (table.X == null || table.Y == null) continue;

            int x = table.X.Value;
            int y = table.Y.Value;

            // Positie buiten het raster dus als die niet bestaat kan je hem niet aanmaken
            if (x < 0 || x >= cols || y < 0 || y >= rows) continue;

            grid[x, y] = table;
        }

        string border = "+" + new string('-', InnerWidth) + "+";
        string blank = "|" + TextHelper.Spaces(InnerWidth) + "|";

        var box = new List<string>();
        box.Add(border);
        box.Add("|" + TextHelper.Middle("TABLES & DINING", InnerWidth) + "|");

        string header = TextHelper.Spaces(LabelWidth);
        for (int x = 0; x < cols; x++)
            header += TextHelper.Middle($"X{x}", cellWidth);
        box.Add("|" + TextHelper.FillRight(header, InnerWidth) + "|");

        for (int y = 0; y < rows; y++)
        {
            string row = TextHelper.FillRight($"Y{y}", LabelWidth);

            for (int x = 0; x < cols; x++)
            {
                TableModel? t = grid[x, y];
                string text = t == null
                    ? ""
                    : $"[T{t.Id}: {t.Capacity}p{(t.IsOccupied == true ? "*" : "")}]";

                row += TextHelper.Middle(text, cellWidth);
            }

            box.Add("|" + TextHelper.FillRight(row, InnerWidth) + "|");
            if (y < rows - 1) box.Add(blank);
        }

        box.Add(border);
        return box;
    }
}

// Tekent het kader en zet twee vakken naast elkaar (non-static class)
public class FloorPlanRenderer
{
    public int FrameWidth { get; }
    public int LeftWidth { get; }
    public int RightWidth { get; }

    private readonly IFloorSection _left;
    private readonly IFloorSection _right;

    public FloorPlanRenderer(IFloorSection left, IFloorSection right,
                             int frameWidth, int leftWidth, int rightWidth)
    {
        _left = left;
        _right = right;
        FrameWidth = frameWidth;
        LeftWidth = leftWidth;
        RightWidth = rightWidth;
    }

    public void Draw()
    {
        string frame = new string('=', FrameWidth);
        string empty = "|" + TextHelper.Spaces(FrameWidth - 2) + "|";

        Console.WriteLine(frame);
        Console.WriteLine("|" + TextHelper.Middle("WHITE SMOKE", FrameWidth - 2) + "|");
        Console.WriteLine("|" + TextHelper.Middle("Wijnhaven 107, 3011 WN Rotterdam", FrameWidth - 2) + "|");
        Console.WriteLine(frame);
        Console.WriteLine("|" + TextHelper.FillRight(" [ ENTRANCE / RECEPTION ]", FrameWidth - 2) + "|");
        Console.WriteLine(empty);

        List<string> left = _left.Draw();
        List<string> right = _right.Draw();

        int lines = Math.Max(left.Count, right.Count);
        for (int i = 0; i < lines; i++)
        {
            string l = i < left.Count ? left[i] : "";
            string r = i < right.Count ? right[i] : "";

            Console.WriteLine("| " + TextHelper.FillRight(l, LeftWidth) + "    "
                              + TextHelper.FillRight(r, RightWidth) + " |");
        }

        Console.WriteLine(frame);
        Console.WriteLine("* = occupied");
    }
}

// Alles wat de admin ziet en intypt
public static class FloorPlanUI
{
    private const int FrameWidth = 74;
    private const int LeftWidth = 28;
    private const int InnerWidth = 36;

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

    // Laatste acties, nieuwste bovenaan (Stack)
    private static readonly Stack<string> ActionLog = new Stack<string>();

    public static void Show()
    {
        if (!UserLogic.IsLoggedIn() || !UserLogic.IsAdmin())
        {
            Console.WriteLine("Only logged-in administrators can access the floor plan.");
            return;
        }

        while (true)
        {
            DrawFloorPlan();
            PrintRecentActions();
            Console.WriteLine();
            Console.WriteLine("1. Add table");
            Console.WriteLine("2. Move table");
            Console.WriteLine("3. Delete table");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Choice: ");

            MenuChoice? choice = ReadMenuChoice();

            switch (choice)
            {
                case MenuChoice.Add: AddTableMenu(); break;
                case MenuChoice.Move: MoveTableMenu(); break;
                case MenuChoice.Delete: DeleteTableMenu(); break;
                case MenuChoice.Back:
                    return;
                default:
                    PrintError("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    public static void DrawFloorPlan()
    {
        var renderer = new FloorPlanRenderer(
            new StaticSection(LeftColumn),
            new DiningSection(InnerWidth),
            FrameWidth, LeftWidth, InnerWidth + 2);

        renderer.Draw();
    }


    private static MenuChoice? ReadMenuChoice()
    {
        string? input = Console.ReadLine()?.Trim();

        if (Enum.TryParse(input, out MenuChoice choice) && Enum.IsDefined(typeof(MenuChoice), choice))
            return choice;

        return null;
    }

    private static void PrintRecentActions()
    {
        if (ActionLog.Count == 0) return;

        Console.WriteLine();
        Console.WriteLine("Recent actions:");

        int shown = 0;
        foreach (string action in ActionLog)  
        {
            Console.WriteLine(" - " + action);
            if (++shown == 3) break;
        }
    }

    private static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

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

    private static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }


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
            ActionLog.Push($"Added {capacity.Value}p table at X{position.Value.X}, Y{position.Value.Y}");
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

        int? id = ReadValidated("Table number to move: ", TableLogic.ValidateTableExists);
        if (id == null) return;

        var position = ReadFreePosition(ignoreTableId: id.Value);
        if (position == null) return;

        try
        {
            TableLogic.MoveTable(id.Value, position.Value.X, position.Value.Y);
            Console.WriteLine("Table moved.");
            ActionLog.Push($"Moved T{id.Value} to X{position.Value.X}, Y{position.Value.Y}");
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

        int? id = ReadValidated("Table number to delete: ", TableLogic.ValidateTableExists);
        if (id == null) return;

        Console.Write($"Are you sure you want to delete table {id.Value}? (y/n): ");
        string? answer = Console.ReadLine()?.Trim().ToLower();

        if (answer == "y" || answer == "j")
        {
            try
            {
                TableLogic.DeleteTable(id.Value);
                Console.WriteLine("Table deleted.");
                ActionLog.Push($"Deleted T{id.Value}");
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
    static string GetMessage(TableError error)
    {
    return error switch
    {
        TableError.NotFound        => "Table could not be found.",
        TableError.InvalidCapacity => "Invalid table capacity. A table must have a capacity of 2, 4, or 6 persons.",
        TableError.InvalidColumn   => $"Invalid column. X must be between 0 and {TableLogic.MaxX}.",
        TableError.InvalidRow      => $"Invalid row. Y must be between 0 and {TableLogic.MaxY}.",
        TableError.PositionTaken   => "That position is already occupied by another table.",
        TableError.HasReservations => "Table can't be deleted because it still has reservations.",
        _                          => "Something went wrong."
    };
    }   
}