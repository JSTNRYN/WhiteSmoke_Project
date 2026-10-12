

//This class is not static so later on we can use inheritance and interfaces
using System.Security.Cryptography.X509Certificates;

public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }
    private AccountsAccess _access = new();
    public static bool LoggedIn()
        {
            return CurrentAccount != null;
        }


    public AccountModel CheckLogin(string email, string password)
    {


        AccountModel acc = _access.GetByEmail(email);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }
}




