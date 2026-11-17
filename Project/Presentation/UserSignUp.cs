namespace Project.Presentation;

public class UserSignUp
{
    private static UserLogic _userLogic = new UserLogic();

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Account Sign up\n");
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
        
        string password = InterceptPassword();
        _userLogic.CreateAccount(firstName, lastName, email, phoneNumber, password, dateOfBirth);
    }
    
    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
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

    public static string? AskEmail()
    {
        Console.Write("Please enter your e-mail (Or type q and enter to go back): ");
        return Console.ReadLine();
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

    public static string InterceptPassword()
    {
        Console.Write("Please enter your password: ");
        string password = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
                continue;
            }
            if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write('*');
            }
        }
        return password;
    }
}
