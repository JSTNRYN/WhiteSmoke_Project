using Project.Presentation;

static class Menu
{
    public static void ShowRestaurantName()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
        Console.WriteLine(@"White Smoke");
    }

    public static void ShowRestaurantDescription()
    {
        Console.WriteLine(@"Welcome to White Smoke, where fire and smoke bring every dish to life.
Our pitmasters slowly cook meat, fish and vegetables over an open fire,
for deep flavour with a light, fresh finish.");
    }

    public static void Start()
    {
        ShowRestaurantName();
        ShowRestaurantDescription();
        bool firstMenu = true;

        while (true)
        {
            if (!firstMenu)
                ShowRestaurantName();
            firstMenu = false;

            bool loggedIn = UserLogic.IsLoggedIn();
            bool isAdmin = loggedIn && UserLogic.IsAdmin();
            ShowMainOptions(loggedIn, isAdmin);

            string? choice = Console.ReadLine()?.Trim();
            if (loggedIn)
                HandleLoggedInChoice(choice, isAdmin);
            else
                HandleGuestChoice(choice);
        }
    }

    private static void ShowMainOptions(bool loggedIn, bool isAdmin)
    {
        if (!loggedIn)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You are currently not logged in.");
            Console.ResetColor();
            Console.WriteLine("1. Log in");
            Console.WriteLine("2. Sign up");
            Console.WriteLine("3. About");
            return;
        }

        var account = UserLogic.CurrentAccount!;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Logged in as: {account.FirstName} {account.LastName}");
        Console.ResetColor();
        Console.WriteLine("1. Make a reservation");
        Console.WriteLine("2. View your reservations");
        Console.WriteLine("3. About");
        Console.WriteLine("4. Log out");

        if (!isAdmin) return;

        Console.WriteLine("5. Edit accounts");
        Console.WriteLine("6. View and edit the floor plan");
    }

    private static void HandleGuestChoice(string? choice)
    {
        switch (choice)
        {
            case "1": UserLogin.Start(); break;
            case "2": UserSignUp.Start(); break;
            case "3": About(); break;
            default: ShowInvalidChoice(); break;
        }
    }

    private static void HandleLoggedInChoice(string? choice, bool isAdmin)
    {
        switch (choice)
        {
            case "1": MakeReservation.Start(); break;
            case "2": MyReservation.ShowReservations(UserLogic.CurrentAccount!.Id); break;
            case "3": About(); break;
            case "4": UserLogic.Logout(); break;
            case "5" when isAdmin: AdminFunctions.ShowCustomers(); break;
            case "6" when isAdmin: FloorPlanUI.Show(); break;
            default: ShowInvalidChoice(); break;
        }
    }

    private static void ShowInvalidChoice()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid input.");
        Console.ResetColor();
        Thread.Sleep(1000);
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
Our pitmasters tend an open fire daily, slowly cooking meat, fish, and vegetables until they develop deep, smoky flavour without losing their freshness.
Every dish is finished light and bright, so the smoke adds depth instead of weight.
We keep the menu honest and the method simple, because good fire-cooked food doesn’t need much else.

Pull up a chair. The fire is already lit.

The crew:
 - Marcus Hale, Head Pitmaster: runs the fire and the cooking schedule.
 - Elena Voss, Pitmaster, Fish and Vegetables: handles the delicate cooks over open flame.
 - Tomás Rivera, Sous Chef: finishes the dishes and keeps the plates light and fresh.
 - Priya Nair, Front of House Manager: seats guests and runs the floor.

            ");

            Console.WriteLine("enter 1 to back");
            string? input = Console.ReadLine();
            if (input == "1")
            {
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong input!");
                Console.ResetColor();
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
