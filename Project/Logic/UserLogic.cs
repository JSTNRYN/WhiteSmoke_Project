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
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }

    public UserModel CreateAccount(string email, string password, string firstname, string lastname, string phoneNumber, string role="Customer")
    {
        UserModel acc = new UserModel(0, email, password, firstname, lastname, phoneNumber, role);
        _access.Write(acc);
        return acc;
    }

    public static bool ValidateEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}