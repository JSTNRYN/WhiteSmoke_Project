namespace Project.Presentation;
public class UserSignUp
{
    private static UserLogic _userLogic = new UserLogic();
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Account Sign up\n");
        Console.WriteLine("Press q and enter to go back");
        string firstName = AskFirstName() ?? string.Empty;
        while (!UserLogic.ValidateName(firstName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            firstName = AskFirstName() ?? string.Empty;
        }
        
        string lastName = AskLastName() ?? string.Empty;
        while (!UserLogic.ValidateName(lastName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            lastName = AskLastName() ?? string.Empty;
        }

        DateTime dateOfBirth = AskDateOfBirth();
        
        string email = (AskEmail() ?? string.Empty).ToLowerInvariant();
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
            email = (AskEmail() ?? string.Empty).ToLowerInvariant();
        }
        
        string phoneNumber = AskPhoneNumber();
        
        string password = UserLogic.InterceptPassword("Please enter your password: ");
        _userLogic.CreateAccount(firstName, lastName, email, phoneNumber, password, dateOfBirth);
    }

    public static string? AskEmail()
    {
        Console.Write("Please enter your e-mail (Or type q and enter to go back): ");
        return Console.ReadLine();
    }
    
    public static string? AskFirstName()
    {
        Console.Write("Please enter your first name (Or type q and enter to go back): ");
        return Console.ReadLine();
    }

    public static string? AskLastName()
    {
        Console.Write("Please enter your last name (Or type q and enter to go back): ");
        return Console.ReadLine();
    }

    public static DateTime AskDateOfBirth()
    {
        const string format = "dd-MM-yyyy";

        while (true)
        {
            Console.Write($"Date of birth ({format}): ");
            string? input = Console.ReadLine();

            if (DateTime.TryParseExact(input, format, null, System.Globalization.DateTimeStyles.None, out DateTime date)
                && date.Date <= DateTime.Today
                && date.Date >= DateTime.Today.AddYears(-120))
            {
                return date.Date;
            }

            ShowError($"Please enter a valid date in {format} format.");
        }
    }

    public static string? AskPassword()
    {
        string specialCharacters = "!@#$%&*";
        while (true)
        {
            Console.Write("Please enter your password: ");
            string? password = Console.ReadLine();
            if (password is null)
            {
                return null;
            }
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

    public static string AskPhoneNumber()
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
    
    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
}