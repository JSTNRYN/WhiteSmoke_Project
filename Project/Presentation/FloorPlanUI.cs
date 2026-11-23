public static class FloorPlanUI
{
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
        DrawRoomLayout();
        Console.WriteLine("TABLES & DINING");
        DrawTableRows(TableLogic.GetAllTables());
    }

    public static void DrawReservationFloorPlan(List<TableModel> reservationTables, HashSet<long> reservedTableIds)
    {
        DrawRoomLayout();
        Console.WriteLine("TABLES & DINING - choose by table number");
        DrawTableRows(reservationTables, reservedTableIds);
    }

    private static void DrawRoomLayout()
    {
        Console.WriteLine("============================================================");
        Console.WriteLine("                     WHITE SMOKE                            ");
        Console.WriteLine("             Wijnhaven 107, Rotterdam                       ");
        Console.WriteLine("============================================================");
        Console.WriteLine("                 ENTRANCE / RECEPTION                       ");
    }

    private static void DrawTableRows(List<TableModel> tables, HashSet<long>? reservedTableIds = null)
    {
        var leftSide = new List<string>
        {
            "+----------------------+",
            "|         BAR          |",
            "| B1 B2 B3 B4 B5 B6... |",
            "+---------------------- +",
            "+----------------------+",
            "|      HOT SEATS       |",
            "| H1 H2 H3 H4 H5 H6    |",
            "| H7 H8  +--------+    |",
            "| H9 H10 | KITCHEN|    |",
            "+--------+--------+----+",
            "+----------------------+",
            "|      RESTROOMS       |",
            "| Women Men Accessible|",
            "+----------------------+"
        };

        var tableSide = new List<string>
        {
            "+-----------------------------------------------------------------+",
            "|                       TABLES & DINING                            |",
            "+-----------------------------------------------------------------+",
            "|    Y    |       X0       |       X1       |       X2       |",
            "+-----------------------------------------------------------------+"
        };

        for (int y = 0; y <= TableLogic.MaxY; y++)
        {
            tableSide.Add("");
            tableSide.Add("+-----------------------------------------------------------------+");
        }

        int lineCount = Math.Max(leftSide.Count, tableSide.Count);
        for (int i = 0; i < lineCount; i++)
        {
            string left = i < leftSide.Count ? leftSide[i] : "                        ";
            string right = i < tableSide.Count ? tableSide[i] : "";
            Console.Write(left);
            Console.Write("    ");

            if (i >= 5 && i < 5 + TableLogic.MaxY * 2 + 1 && (i - 5) % 2 == 0)
            {
                int y = (i - 5) / 2;
                Console.Write("|    Y" + y + "    |");
                for (int x = 0; x <= TableLogic.MaxX; x++)
                {
                    TableModel? table = tables.FirstOrDefault(t => t.X == x && t.Y == y);
                    if (table == null)
                    {
                        Console.Write("    [ empty ]    |");
                        continue;
                    }

                    bool isReserved = reservedTableIds?.Contains(table.Id) == true;
                    if (reservedTableIds != null)
                        Console.ForegroundColor = isReserved ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write("    [T" + table.Id + ": " + table.Capacity + "p]    |");
                    if (reservedTableIds != null) Console.ResetColor();
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(right);
            }
        }
    }

    // Print een foutmelding in het rood en zet de kleur daarna terug
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
            Console.WriteLine("Deletion cancelled. The table remains unchanged.");
        }
        Pause();
    }
}
