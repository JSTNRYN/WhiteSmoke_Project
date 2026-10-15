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
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You are currently not logged in!");
            Console.ResetColor();
            Console.WriteLine("Enter 1 to login");
            Console.WriteLine("Enter 2 to sign up");
        }
        
        Console.WriteLine("Enter 3 to do something else in the future");

        string input = Console.ReadLine();

        if (input == "1" && !UserLogic.IsLoggedIn())
        {
            UserLogin.Start();
        }
        else if (input == "2" && !UserLogic.IsLoggedIn())
        {
            UserSignUp.Start();
        }
        else if (input == "1" && UserLogic.IsLoggedIn())
        {
            MakeReservation.Start();
        }
        else if (input == "2" && UserLogic.IsLoggedIn())
        
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("view your reservations is nog niet geimplementeerd");
            Console.ResetColor();


            MyReservation.ShowReservations(UserLogic.CurrentAccount!.Id);
        }
        else
        {
            Console.WriteLine("Invalid input");
        }
    }
}
}
