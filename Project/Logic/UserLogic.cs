public class UserLogic
{
    public static UserModel? CurrentAccount { get; private set; }
    private UserAccess _access = new();

    public UserLogic()
    {
    }

    public UserModel CheckLogin(string email, string password)
    {
        UserModel acc = _access.GetByEmail(email);
    
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
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool ValidateEmail(string email)
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
}