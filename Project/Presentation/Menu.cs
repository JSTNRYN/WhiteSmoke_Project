using Project.Presentation;

static class Menu
{
    private static string _homepageMessage = "Enter on of the option numbers";
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
        ShowRestaurantName();
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
            Console.WriteLine(_homepageMessage);
            Console.WriteLine("1. Log in");
            Console.WriteLine("2. Sign up");
            Console.WriteLine("3. About");
            return;
        }

        var account = UserLogic.CurrentAccount!;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Logged in as: {account.FirstName} {account.LastName}");
        Console.ResetColor();
        Console.WriteLine(_homepageMessage);
        Console.WriteLine("1. Make a reservation");
        Console.WriteLine("2. View your reservations");
        Console.WriteLine("3. About");
        Console.WriteLine("4. Log out");
        Console.WriteLine("5: Edit personal details");

        if (!isAdmin) return;

        Console.WriteLine("6. Manage menu items");
        Console.WriteLine("7. View and edit the floor plan");
        Console.WriteLine("8. View and Manage customers");
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
            case "5": EditUserDetails(UserLogic.CurrentAccount); break;
            case "6" when isAdmin: break;
            case "7" when isAdmin: FloorPlanUI.Show(); break;
            case "8" when isAdmin: AdminFunctions.ShowCustomers(); break;
            default: ShowInvalidChoice(); break;
        }
    }

    private static void EditUserDetails(UserModel user)
    {
        Console.WriteLine("Current details:");
        Console.WriteLine($"1: First name: {user.FirstName}");
        Console.WriteLine($"2: Last name: {user.LastName}");
        Console.WriteLine($"3: Email address: {UserLogic.GetCustomerById(UserLogic.CurrentAccount.Id).EmailAddress}");
        Console.WriteLine($"4: Date of birth (DD-MM-YYYY): {user.DateOfBirth:dd-MM-yyyy}");
        Console.WriteLine($"5: Phone number: {user.PhoneNumber}");
        Console.WriteLine($"6: Change your password");
        Console.WriteLine("7: Quit");
        Console.WriteLine("-----------------");
        Console.WriteLine("Please enter the number of the detail you would like to edit:");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1": ChangeFirstName(); break;
            case "2": ChangeLastName(); break;
            case "3": ChangeEmail(); break;
            case "4": ChangeDob(); break;
            case "5": ChangePhoneNumber(); break;
            case "6": ChangePhoneNumber(); break;
            case "7" : break;
            default: ShowInvalidChoiceForPersonalData(user); break;
        }
    }
    
    private static void ChangeFirstName()
    {
        
    }
    
    private static void ChangeLastName()
    {
        
    }
    
    private static void ChangeEmail()
    {
        
    }
    
    private static void ChangeDob()
    {
        
    }
    
    private static void ChangePhoneNumber()
    {
        
    }
    
    private static void ShowInvalidChoice()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid input.");
        Console.ResetColor();
        Thread.Sleep(1000);
    }

    private static void ShowInvalidChoiceForPersonalData(UserModel user)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid input.");
        Console.ResetColor();
        Thread.Sleep(1000);
        EditUserDetails(user);
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
