using Project.Presentation;

public class UserLogic
{
    public static UserModel? CurrentAccount { get; private set; }
    private UserAccess _access = new();

    public UserLogic()
    {
    }

    public UserModel CheckLogin(string email, string password)
    {
        email = email.Trim().ToLowerInvariant();
        UserModel acc = UserAccess.GetByEmail(email);
    
        if (acc != null && BCrypt.Net.BCrypt.Verify(password, acc.Password))
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }

    public void CreateAccount(string firstname, string lastname, string email, string phone, string password, string role = "Customer")
    {
        string hashedPassword = HashPassword(password);

        UserModel acc = new UserModel(0, email, hashedPassword, firstname, lastname, phone, role);
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

    public static bool IsValidEmailFormat(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public static bool ValidateName(string name)
    {
        return (name.Length > 1);
    }

    public static bool IsLoggedIn()
    {
        return CurrentAccount != null;
    }
    
    public static void Logout()
    {
        CurrentAccount = null;
    }

    public static void DeleteAccount(UserModel account)
    {
        UserAccess.Delete(account);
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

    public static string? AskPassword()
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
}
