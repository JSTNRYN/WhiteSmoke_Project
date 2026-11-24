public static class MenuHelper
{
    public static int Run(List<string> menuOptions, string title)
    {
        int menuIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < menuOptions.Count; i++)
            {
                if (i == menuIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {menuOptions[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {menuOptions[i]}");
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                menuIndex += 1;
                if (menuIndex >= menuOptions.Count)
                    menuIndex = 0;
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                menuIndex -= 1;
                if (menuIndex < 0)
                    menuIndex = menuOptions.Count - 1;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                return menuIndex;
            }
        }
    }
}