namespace Project.Presentation;

public class UserSignUp
{
    private static UserLogic _userLogic = new UserLogic();

    public static void Start()
    {
        // Clear the console
        Console.Clear();
        Console.WriteLine("Account Sign up\n");

        Console.Write("Please enter your first name: ");
        string firstName = Console.ReadLine();
        
        Console.Write("Please enter your last name: ");
        string lastName = Console.ReadLine();
        
        string email = AskEmail();
        while (!UserLogic.ValidateEmail(email))
        {
            ShowError("Invalid email format!");
            email = AskEmail();
        }
        
        Console.Write("Please enter your phone number: ");
        string phoneNumber = Console.ReadLine();
        
        Console.Write("Please enter your password: ");
        string password = Console.ReadLine();
        UserModel acc = _userLogic.CreateAccount(email, password, firstName, lastName, phoneNumber);
        Menu.Start();
    }

    private static string AskEmail()
    {
        Console.Write("Please enter your e-mail: ");
        return Console.ReadLine();
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
}