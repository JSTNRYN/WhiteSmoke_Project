using System;
using System.Text;

static class UserLogin
{
    private static UserLogic _userLogic = new UserLogic();

    public static void Start()
    {

        Console.Write("Please enter your e-mail: ");
        string email = Console.ReadLine();
        
        Console.Write("Please enter your password: ");
        string password = AskPassword();
        
        UserModel acc = _userLogic.CheckLogin(email, password);
        
        if (acc != null)
        {
            // Return to the existing menu loop after a successful login.
        }
        else
        {
            ShowError("No account found with that email and password combination.");
        }
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }

    private static string askEmail()
    {
        Console.Write("Please enter your e-mail: ");
        return Console.ReadLine();
    }
    
    
    public static string AskPassword()
    {
        Console.Write("Please enter your password: ");
        string password = "";

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
        return password;
    }

    
}
