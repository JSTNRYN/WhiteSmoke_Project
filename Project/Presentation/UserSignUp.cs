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
        if (IsQuit(firstName)) return;
        while (!UserLogic.ValidateName(firstName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            firstName = AskFirstName() ?? string.Empty;
            if (IsQuit(firstName)) return;
        }

        string lastName = AskLastName() ?? string.Empty;
        if (IsQuit(lastName)) return;
        while (!UserLogic.ValidateName(lastName))
        {
            ShowError("Please enter a name with at least 2 characters.");
            lastName = AskLastName() ?? string.Empty;
            if (IsQuit(lastName)) return;
        }

        DateTime? dateOfBirth = AskDateOfBirth();
        if (dateOfBirth == null) return;

        string email = (AskEmail() ?? string.Empty).ToLowerInvariant();
        if (IsQuit(email)) return;
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
            if (IsQuit(email)) return;
        }

        string? phoneNumber = AskPhoneNumber();
        if (phoneNumber == null) return;

        string password = UserLogic.InterceptPassword("Please enter your password: ");
        if (IsQuit(password)) return;

        _userLogic.CreateAccount(firstName, lastName, email, phoneNumber, password, dateOfBirth.Value);
    }

    private static bool IsQuit(string? input)
    {
        return input?.Trim().ToLower() == "q";
    }

    public static string? AskEmail()
    {
        Console.Write("Please enter your e-mail: ");
        return Console.ReadLine();
    }

    public static string? AskFirstName()
    {
        Console.Write("Please enter your first name: ");
        return Console.ReadLine();
    }

    public static string? AskLastName()
    {
        Console.Write("Please enter your last name: ");
        return Console.ReadLine();
    }

    public static DateTime? AskDateOfBirth()
    {
        const string format = "dd-MM-yyyy";

        while (true)
        {
            Console.Write($"Date of birth ({format}): ");
            string? input = Console.ReadLine();
            if (IsQuit(input)) return null;

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
            if (password is null || IsQuit(password))
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

    public static string? AskPhoneNumber()
    {
        while (true)
        {
            Console.Write("Please enter your phone number: ");
            string? phoneNumber = Console.ReadLine();
            if (IsQuit(phoneNumber)) return null;

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