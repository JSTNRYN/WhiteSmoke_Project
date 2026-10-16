using Project.Presentation;
public class MainMenu
{
    public static void Run()
    {

        List<string> loggedInOptions = new List<string>
        {
            "Make a reservation",
            "View your reservations",
            "Logout"
        };

        List<string> loggedOutOptions = new List<string>
        {
            "Login",
            "Sign up"
        };

        while(true)
        {
            int choice = MenuHelper.Run(UserLogic.IsLoggedIn() ? loggedInOptions : loggedOutOptions, "Welcome to WhiteSmoke! Please select an option:");

            switch (choice)
            {
                case 0:
                    if (UserLogic.IsLoggedIn())
                    {
                        MakeReservation.Start();
                    }
                    else
                    {
                        UserLogin.Start();
                    }
                    break;
                case 1:
                    if (UserLogic.IsLoggedIn())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("view your reservations is nog niet geimplementeerd");
                        Console.ResetColor();
                        //MyReservation.ShowReservations(UserLogic.CurrentAccount!.Id);
                    }
                    else
                    {
                        UserSignUp.Start();
                    }
                    break;
                case 2:
                    if (UserLogic.IsLoggedIn())
                    {
                        UserLogic.Logout();
                        Console.WriteLine("You have been logged out.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }


        }
    }
}