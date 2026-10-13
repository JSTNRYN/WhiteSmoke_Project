namespace Project.Presentation;

public class UserSignUp
{
    private static UserLogic _userLogic = new UserLogic();

    public static void Start()
    {
        // Clear the console
        Console.Clear();
        Console.WriteLine("Account Sign up\n");

        
        string firstName = AskName();
        while (!UserLogic.ValidateName(firstName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            firstName = AskName();
        }
        
        string lastName = AskName();
        while (!UserLogic.ValidateName(lastName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            lastName = AskName();
        }
        
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
        _userLogic.CreateAccount(firstName, lastName, email, phoneNumber, password);
        Menu.Start();
    }

    private static string? AskEmail()
    {
        Console.Write("Please enter your e-mail: ");
        return Console.ReadLine();
    }
    
    private static string? AskName()
    {
        Console.Write("Please enter your first name: ");
        return Console.ReadLine();
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
    
    
}