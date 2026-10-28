namespace Project.Presentation;
public class UserSignUp
{
    private static UserLogic _userLogic = new UserLogic();
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Account Sign up\n");
        string firstName =UserLogic.AskFirstName();
        while (!UserLogic.ValidateName(firstName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            firstName = UserLogic.AskFirstName();
        }
        
        string lastName = UserLogic.AskLastName();
        while (!UserLogic.ValidateName(lastName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            lastName = UserLogic.AskLastName();
        }
        
        string email = UserLogic.AskEmail().ToLower();
        while (!UserLogic.ValidateEmail(email))
        {
            if (UserLogic.EmailExists(email))
            {
                ShowError("This email is already registered!");
            }
            else
            {
                ShowError("Invalid email format!");
            }
            email = UserLogic.AskEmail().ToLower();
        }
        
        string phoneNumber = UserLogic.AskPhoneNumber();
        
        string password = UserLogic.AskPassword();
        _userLogic.CreateAccount(firstName, lastName, email, phoneNumber, password);
    }
    
    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
}