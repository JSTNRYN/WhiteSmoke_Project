using Project.Presentation;

static class Menu
{
    public static void ShowRestaurantName()
    {
        Console.Clear();
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
        while (true)
        {
            bool isLoggedIn = UserLogic.IsLoggedIn();
            if (isLoggedIn)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You are logged in as: {UserLogic.CurrentAccount!.FirstName} {UserLogic.CurrentAccount.LastName}");
                Console.ResetColor();
                Console.WriteLine("Enter 1 to make a reservation");
                Console.WriteLine("Enter 2 to view your reservations");
                Console.WriteLine("Enter 3 to view the menu");
                Console.WriteLine("Enter 4 to log out");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are currently not logged in!");
                Console.ResetColor();
                Console.WriteLine("Enter 1 to login");
                Console.WriteLine("Enter 2 to sign up");
                Console.WriteLine("Enter 3 to view the about page");
            }

            string? input = Console.ReadLine()?.Trim();
            if (!MenuLogic.IsKnownChoice(input, isLoggedIn, UserLogic.IsAdmin()))
            {
                if (!isLoggedIn && input == "3") { About(); continue; }
                Console.WriteLine("Invalid input");
                continue;
            }

            if (input == "1" && !isLoggedIn) UserLogin.Start();
            else if (input == "2" && !isLoggedIn) UserSignUp.Start();
            else if (input == "1") MakeReservation.Start();
            else if (input == "2") MyReservation.ShowReservations(UserLogic.CurrentAccount!.Id);
            else if (input == "3" && isLoggedIn) MenuView.ShowMenu(UserLogic.CurrentAccount!);
            else if (input == "4" && isLoggedIn) UserLogic.Logout();
        }
    }

    public static void About()
    {
        Console.Clear();
        ShowRestaurantName();
        Console.WriteLine("About\n");
        ShowRestaurantDescription();
        Console.WriteLine("\nPress 1 to go back.");
        while (Console.ReadLine() != "1") Console.WriteLine("Wrong input!");
    }
}
