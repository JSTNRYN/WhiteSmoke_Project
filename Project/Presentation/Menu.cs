using Project.Presentation;

static class Menu
{
    private static readonly string[] _guestOptions = ["Log in", "Sign up", "View Menu", "About us"];
    private static readonly string[] _UserOptions = ["Make a reservation", "View your reservations", "View Menu", "About us", "Log out"];
    private static readonly string[] _adminOptions = ["Make a reservation", "View your reservations", "View Menu", "About us", "Log out", "View admin menu"];

    public static void ShowRestaurantName()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
        Console.WriteLine(@"
        (                             
        (  (         )        )        )\ )                 )        
        )\))(   ' ( /( (   ( /(   (   (()/(    )         ( /(    (   
        ((_)()\ )  )\()))\  )\()) ))\   /(_))  (      (   )\())  ))\  
        _(())\_)()((_)(_))/ /((_) (_))    )\  '  )\ ((_\  /((_) 
        \ \((_)/ /| |(_)(_) | |_ (_))   / __| _((_))  ((_)| |(_)(_))   
         \ \/\/ / | ' \ | | |  _|/ -_)  \__ \| '  \()/ _ \| / / / -_)  
          \_/\_/  |_||_||_| \\__|\___|  |___/|_|_|_| \___/|_\_\ \___|
");
    }

    public static void Start()
    {
        while (true)
        {
            ShowRestaurantName();

            bool loggedIn = UserLogic.IsLoggedIn();
            bool isAdmin = loggedIn && UserLogic.IsAdmin();



            if (isAdmin)
            {
                HandleLoggedInChoice(InteractiveMenu(_adminOptions), isAdmin);
            }
            else if (loggedIn)
            {
                HandleLoggedInChoice(InteractiveMenu(_UserOptions), isAdmin);
            }
            else
            {
                HandleGuestChoice(InteractiveMenu(_guestOptions));
            }
        }
    }

    private static void HandleGuestChoice(int choice)
    {
        switch (choice)
        {
            case 0: UserLogin.Start(); break;
            case 1: UserSignUp.Start(); break;
            case 2:
                List<MenuItemModel> list = MenuLogic.GetMenuItemsForDisplay();
                MenuView.PrintItems(list);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press Enter to return...");
                Console.ResetColor();
                Console.ReadLine();
                Console.Clear();
                break;
            case 3: About(); break;
        }
    }

    private static void HandleLoggedInChoice(int? choice, bool isAdmin)
    {
        switch (choice)
        {
            case 0: MakeReservation.Start(); break;
            case 1: MyReservation.ShowReservations(UserLogic.CurrentAccount!.Id); break;
            case 2:
                List<MenuItemModel> list = MenuLogic.GetMenuItemsForDisplay();
                MenuView.PrintItems(list);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Press Enter to return...");
                Console.ResetColor();
                Console.ReadLine();
                Console.Clear();
                break;
            case 3: About(); break;
            case 4: UserLogic.Logout(); break;
            case 5 when isAdmin: AdminFunctions.AdminMenu(); break;
        }
    }

    public static void About()
    {
        Console.Clear();
        bool repeat = true;
        while (repeat)
        {
            ShowRestaurantName();
            Console.WriteLine("About\n");
            Console.WriteLine(@"
White Smoke is built around one idea: cook with fire, and let the ingredients do the talking.
Our Pitmasters tend an open fire daily, slowly cooking meat, fish, and vegetables until they develop deep, smoky flavour without losing their freshness.
Every dish is finished light and bright, so the smoke adds depth instead of weight.
We keep the menu honest and the method simple, because good fire-cooked food doesn’t need much else.

Pull up a chair. The fire is already lit.

The crew:
 - Marcus Hale, Head Pitmaster: runs the fire and the cooking schedule.
 - Elena Voss, Pitmaster, Fish and Vegetables: handles the delicate cooks over open flame.
 - Tomás Rivera, Sous Chef: finishes the dishes and keeps the plates light and fresh.
 - Priya Nair, Front of House Manager: seats guests and runs the floor.

            ");

            Console.WriteLine(@"
Welcome to White Smoke, where fire and smoke bring every dish to life.
Our pitmasters slowly cook meat, fish and vegetables over an open fire,
for deep flavour with a light, fresh finish.
Take a seat, the fire is already lit.

Address
  White Smoke
  Wijnhaven 107
  3011 WN Rotterdam
  The Netherlands

Opening hours
  Monday        Closed
  Tuesday       Closed
  Wednesday     5:00 PM - 11:00 PM
  Thursday      5:00 PM - 11:00 PM
  Friday        5:00 PM - 11:00 PM
  Saturday      12:00 PM - 11:00 PM
  Sunday        12:00 PM - 10:00 PM
");

            Console.WriteLine("Press enter to go back");
            Console.ReadLine();
            Console.Clear();
            return;
        }
    }

    public static int InteractiveMenu(string[] menuOptions)
    {
        ConsoleKeyInfo key;
        int option = 0;
        int maxMenuOptionNumber = menuOptions.Length - 1;
        bool notSelected = true;
        (int left, int top) = Console.GetCursorPosition();

        while (notSelected)
        {
            Console.SetCursorPosition(left, top);

            Console.WriteLine("Press up/down arrows to go over the options");
            Console.WriteLine("Press enter to confirm option");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine($"{(option == i ? $"* {menuOptions[i]}" : $"  {menuOptions[i]}")}");
            }

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    option = (option == maxMenuOptionNumber) ? maxMenuOptionNumber : option + 1;
                    break;
                case ConsoleKey.UpArrow:
                    option = (option == 0) ? 0 : option - 1;
                    break;
                case ConsoleKey.Enter:
                    notSelected = false;
                    break;
            }
        }

        return option;
    }
}