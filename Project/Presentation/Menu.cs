using Project.Presentation;

static class Menu
{
    public static void ShowRestaurantName()
    {
        Console.Clear();
        Console.WriteLine(@"
        (                             
        (  (         )        )        )\ )                 )        
        )\))(   ' ( /( (   ( /(   (   (()/(    )         ( /(    (   
        ((_)()\ )  )\()))\  )\()) ))\   /(_))  (      (   )\())  ))\  
        _(())\_)()((_)\((_)(_))/ /((_) (_))    )\  '  )\ ((_)\  /((_) 
        \ \((_)/ /| |(_)(_) | |_ (_))   / __| _((_))  ((_)| |(_)(_))   
         \ \/\/ / | ' \ | | |  _|/ -_)  \__ \| '  \()/ _ \| / / / -_)  
          \_/\_/  |_||_||_| \\__|\___|  |___/|_|_|_| \___/|_\_\ \___|
");
    }

    public static void ShowRestaurantDescription()
    {
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
    }
    public static void Start()
    {
        while (true)
        {
            if (UserLogic.IsLoggedIn())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You are logged in as: {UserLogic.CurrentAccount.FirstName} {UserLogic.CurrentAccount.LastName}");
                Console.ResetColor();
                Console.WriteLine("Enter 1 to make a reservation");
                Console.WriteLine("Enter 2 to view your reservations");
                Console.WriteLine("Enter 3 to view the about page");
                Console.WriteLine("Enter 4 to log out\n");
                if (UserLogic.IsAdmin())
                {
                    Console.WriteLine("Enter 5 to edit accounts");
                    Console.WriteLine("Enter 6 to edit reservations");
                    Console.WriteLine("Enter 7 to view and edit the floor plan");
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You are currently not logged in!");
                Console.ResetColor();
                Console.WriteLine("Enter 1 to login");
                Console.WriteLine("Enter 2 to sign up");
                Console.WriteLine("Enter 3 to the view about page");
            }

            string input = Console.ReadLine();

            if (input == "1" && !UserLogic.IsLoggedIn())
            {
                UserLogin.Start();
            }
            else if (input == "2" && !UserLogic.IsLoggedIn())
            {
                UserSignUp.Start();
            }
            else if (input == "3" && !UserLogic.IsLoggedIn())
            {
                Menu.About();
            }
            else if (input == "1" && UserLogic.IsLoggedIn())
            {
                MakeReservation.Start();
            }
            else if (input == "2" && UserLogic.IsLoggedIn())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.ResetColor();


                MyReservation.ShowReservations(UserLogic.CurrentAccount!.Id);
            }
            else if (input == "3" && UserLogic.IsLoggedIn())
            {
                About();
            }
            else if (input == "4" && UserLogic.IsLoggedIn())
            {
                UserLogic.Logout();
            }
            else if (input == "7" && UserLogic.IsLoggedIn() && UserLogic.IsAdmin())
            {
                FloorPlanUI.Show();
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
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
