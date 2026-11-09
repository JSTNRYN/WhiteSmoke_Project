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

    public static List<UserModel> GetAllCustomers()
    {
        return UserAccess.GetAllCustomers();
    }

    public static UserModel GetCustomerById(int id)
    {
        return UserAccess.GetById(id);
    }

    public static bool IsValidEmailFormat(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public static bool ValidateName(string name)
    {
        return (name.Length > 1);
    }

    public static bool ValidatePhone(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            return false;
        if (!phoneNumber.StartsWith("+"))
            return false;
        if (phoneNumber.Length != 12)
            return false;
        for (int i = 1; i < phoneNumber.Length; i++)
        {
            if (!char.IsDigit(phoneNumber[i]))
                return false;
        }
        return true;
    }
    
    public static bool IsCustomerIdValid(int id)
    { 
        var user = UserAccess.GetById(id);
        if (user == null)
        {
            return false;
        }
        return true;
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

    public static void AskCustomerToEdit()
    {
        var customers = GetAllCustomers();
        Console.WriteLine($"There are currently: {customers.Count - 1} users.");
        foreach (var customer in customers)
        {
            if (customer.Id != CurrentAccount.Id)
            {
                Console.WriteLine($"Id: {customer.Id}");
                Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");
                Console.WriteLine($"Email: {customer.EmailAddress}");
                Console.WriteLine($"Phone number: {customer.PhoneNumber}");
                Console.WriteLine("-------------------------");
            }
        }

        int customerId = int.Parse(AskCustomerId());
        if (IsCustomerIdValid(customerId))
        {
           ModifyCustomerData(customerId);
        }
        else
        {
            UserSignUp.ShowError("Invalid input!");
            AskCustomerToEdit();
        }

    }

    public static void ModifyCustomerData(int customerId)
    {
        bool editing = true;
        while (editing)
        {
            var user = GetCustomerById(customerId);
            
            Console.WriteLine($"\nYou have selected {user.FirstName} {user.LastName}");
            Console.WriteLine("What would you like to change?");
            Console.WriteLine($"1: First name ({user.FirstName})\n2: Last name ({user.LastName})\n3: Email ({user.EmailAddress})\n4: Phone number ({user.PhoneNumber})");
            Console.Write("> ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int result))
            {
                if (result == 1)
                {
                    AskFirstNameToEdit(user);
                }
                else if (result == 2)
                {
                    AskLastNameToEdit(user);
                }
                else if (result == 3)
                {
                    AskEmailToEdit(user);
                }
                else if (result == 4)
                {
                    AskPhoneToEdit(user);
                }
                else
                {
                    UserSignUp.ShowError("Invalid input!");
                    continue;
                }

                Console.Write("\nWould you like to edit anything else for this user? (y/n): ");
                string continueInput = Console.ReadLine()?.Trim().ToLower();
                
                if (continueInput != "y")
                {
                    editing = false;
                }
            }
            else
            {
                UserSignUp.ShowError("Invalid input!");
            }
        }
    }

    public static void AskFirstNameToEdit(UserModel user)
    {
        Console.WriteLine($"Old name: {user.FirstName}");
        Console.Write($"New name: ");
        string newName = Console.ReadLine();
    
        if (ValidateName(newName) == false)
        {
            UserSignUp.ShowError("Please enter a name with at least 2 characters.");
            AskFirstNameToEdit(user);
        }
        else
        {
            UserAccess.UpdateFirstName(user.Id, newName);
        }
    }
    
    public static void AskLastNameToEdit(UserModel user)
    {
        Console.WriteLine($"Old name: {user.LastName}");
        Console.Write($"New name: ");
        string newName = Console.ReadLine();
        if (ValidateName(newName) == false)
        {
            UserSignUp.ShowError("Please enter a name with at least 2 characters.");
            AskLastNameToEdit(user);
        }
        else
        {
            UserAccess.UpdateLastName(user.Id, newName);
        }
    }
    
    public static void AskEmailToEdit(UserModel user)
    {
        Console.WriteLine($"Old email: {user.EmailAddress}");
        Console.Write($"New email: ");
        string newEmail = Console.ReadLine();
        if (ValidateEmail(newEmail) == false)
        {
            UserSignUp.ShowError("Invalid email format!");
            AskEmailToEdit(user);
        }
        else
        {
            UserAccess.UpdateEmail(user.Id, newEmail);
        }
    }
    
    public static void AskPhoneToEdit(UserModel user)
    {
        Console.WriteLine($"Old phone number: {user.PhoneNumber}");
        Console.Write($"New phone number: ");
        string newPhoneNumber = Console.ReadLine();
        if (ValidatePhone(newPhoneNumber) == false)
        {
            UserSignUp.ShowError("Invalid phone number!");
            AskPhoneToEdit(user);
        }
        else
        {
            UserAccess.UpdatePhone(user.Id, newPhoneNumber);
        }
    }
    public static string? AskLastName()
    {
        Console.Write("Please enter your last name: ");
        return Console.ReadLine();
    }
    
    public static bool IsAdmin()
    {
        return CurrentAccount.Role is "Admin";
    }

    public static string AskCustomerId()
    {
        Console.Write("Please enter the id of the user you would like to modify\n> ");
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

