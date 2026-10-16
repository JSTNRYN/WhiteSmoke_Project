public static class MenuHelper
{
    //This class is the base tool for using menu navigation. If you create a menu you can use this class to navigate through the menu and select an option. It will return the index of the selected option.
    public static int Run(List<string> menuOptions, string? message = null)
    {
        int menuIndex = 0; // Keep track of which option is selected
        Console.CursorVisible = false; // Make the cursor in the console invisible while making a choice

        while (true)
        {
            Console.Clear();

            // If no message was given this will not execute
            // You can use a message if you want to reiterate something to the user
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"{message}\n");
            }

            for (int i = 0; i < menuOptions.Count; i++)
            {
                if (i == menuIndex)
                {
                    Console.WriteLine($"> {menuOptions[i]}");
                }
                else
                {
                    Console.WriteLine(menuOptions[i]);
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                menuIndex += 1;

                if (menuIndex >= menuOptions.Count)
                {
                    menuIndex = 0;
                }
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                menuIndex -= 1;

                if (menuIndex < 0)
                {
                    menuIndex = menuOptions.Count - 1;
                }
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                return menuIndex;
            }
            else
            {
                Console.WriteLine("\n Ongeldige toets! Keert terug naar eerste optie.");
                Console.WriteLine("Druk op een toets om verder te gaan...");
                Console.ReadKey();

                menuIndex = 0;
            }
        }
    }
}