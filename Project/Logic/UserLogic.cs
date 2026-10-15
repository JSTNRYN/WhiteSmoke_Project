public class UserLogic
{
    public static UserModel? CurrentAccount { get; private set; }
    private UserAccess _access = new();

    public UserLogic()
    {
    }

    public UserModel CheckLogin(string email, string password)
    {
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
        _access.Write(acc);
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
}