namespace Project.Presentation;

public class UserSignUp
{
    private static UserLogic _userLogic = new UserLogic();

    public static void Start()
    {
        
        Console.Clear();
        Console.WriteLine("Account Sign up\n");

        
        string firstName = AskFirstName();
        while (!UserLogic.ValidateName(firstName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            firstName = AskFirstName();
        }
        
        string lastName = AskLastName();
        while (!UserLogic.ValidateName(lastName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            lastName = AskLastName();
        }
        
        string email = AskEmail().ToLower();
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
            email = AskEmail().ToLower();
        }
        
        string phoneNumber = AskPhoneNumber();
        
        string password = AskPassword();
        _userLogic.CreateAccount(firstName, lastName, email, phoneNumber, password);
    }

    private static string? AskEmail()
    {
        Console.Write("Please enter your e-mail: ");
        return Console.ReadLine();
    }
    
    private static string? AskFirstName()
    {
        Console.Write("Please enter your first name: ");
        return Console.ReadLine();
    }

    private static string? AskLastName()
    {
        Console.Write("Please enter your last name: ");
        return Console.ReadLine();
    }

    private static string? AskPassword()
    {
        string specialCharacters = "!@#$%&*";
        while (true)
        {
            Console.Write("Please enter your password: ");
            string? password = Console.ReadLine();
            // Kijken of er een speciaal karakter in zit.
            bool hasSpecialChar = password.Any(ch => specialCharacters.Contains(ch));

            if (password.Length < 6 || !hasSpecialChar)
            {
                ShowError("Password must be at least 6 characters and contain a special character!");
            }
            else
            {
                return password;
            }
        }
    }

    private static string AskPhoneNumber()
    {
        while (true)
        {
            Console.Write("Please enter your phone number: ");
            string? phoneNumber = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.All(char.IsDigit))
            {
                return phoneNumber;
            }

            ShowError("Invalid phone number! Please enter numbers only.");
        }
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
    
    
}
