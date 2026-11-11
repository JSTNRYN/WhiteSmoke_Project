using System.Text;
using Project.Presentation;

public class UserLogic
{
    public static UserModel? CurrentAccount { get; private set; }
    private UserAccess _access = new();

    public UserLogic()
    {
    }

    public UserModel? CheckLogin(string email, string password)
    {
        email = email.Trim().ToLowerInvariant();
        UserModel? acc = UserAccess.GetByEmail(email);
        if (acc != null && BCrypt.Net.BCrypt.Verify(password, acc.Password))
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }

    public void CreateAccount(string firstname, string lastname, string email, string phone, string password, DateTime dateOfBirth, string role = "Customer")
    {
        string hashedPassword = HashPassword(password);

        UserModel acc = new UserModel(0, email, hashedPassword, firstname, lastname, phone, role, dateOfBirth);
        acc.Id = _access.Write(acc);
        CurrentAccount = acc;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool ValidateEmail(string email)
    {
        return IsValidEmailFormat(email) && !EmailExists(email);
    }

    public static bool EmailExists(string email)
    {
        return UserAccess.GetByEmail(email) != null;
    }

    public static bool IsValidEmailFormat(string? email)
    {
        return !string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Contains(".");
    }

    public static bool ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        string digits = phoneNumber.StartsWith("+") ? phoneNumber.Substring(1) : phoneNumber;

        if (digits.Length == 0 || !digits.All(char.IsDigit))
        {
            return false;
        }

        return digits.Length >= 8 && digits.Length <= 15;
    }
    
    public static string ReadPassword(string prompt = "Please enter your password: ")
    {
        Console.Write(prompt);
        var password = new StringBuilder();
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true); // true = don't echo the key

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password.Length--;   
                    Console.Write("\b \b");
                }
                continue;
            }
            if (!char.IsControl(key.KeyChar))
            {
                password.Append(key.KeyChar);
                Console.Write('*');
            }
        }
        return password.ToString();
    }

    public static bool ValidateName(string? name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.Trim().Length > 1;
    }

    public static bool IsLoggedIn()
    {
        return CurrentAccount != null;
    }
    
    public static void Logout()
    {
        CurrentAccount = null;
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
    
    public static bool IsAdmin()
    {
        return CurrentAccount?.Role is "Admin";
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

            UserSignUp.ShowError($"Please enter a valid date in {format} format.");
        }
    }

    public static bool IsAdult(DateTime? dateOfBirth)
    {
        if (dateOfBirth == null) return false;

        DateTime today = DateTime.Today;
        int age = today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > today.AddYears(-age)) age--;
        return age >= 18;
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
                UserSignUp.ShowError("Password must be at least 6 characters and contain a special character!");
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

            UserSignUp.ShowError("Invalid phone number! Please enter numbers only.");
        }
    }

    public static List<UserModel> GetAllCustomers_ExceptPassword()
    {
        return UserAccess.GetAllCustomers_ExceptPassword();
    }

    public static void UpdateFirstName(UserModel customer, string newFirstName)
    {
        UserAccess.UpdateFirstName(customer, newFirstName);
    }

    public static void UpdateLastName(UserModel customer, string newLastName)
    {
        UserAccess.UpdateLastName(customer, newLastName);
    }

    public static void UpdateEmail(UserModel customer, string newEmail)
    {
        UserAccess.UpdateEmail(customer, newEmail);
    }

    public static void UpdatePhoneNumber(UserModel customer, string newPhoneNumber)
    {
        UserAccess.UpdatePhoneNumber(customer, newPhoneNumber);
    }

    public static UserModel GetCustomerById(int id)
    {
        return UserAccess.GetById(id);
    }


}
