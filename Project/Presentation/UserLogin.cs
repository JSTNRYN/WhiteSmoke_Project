using System;
using System.Text;

static class UserLogin
{
    private static AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Account Login\n");

        string email = PromptInput("Email");
        string password = AskForPassword();

        AccountModel acc = accountsLogic.CheckLogin(email, password);

        if (acc != null)
        {
            Menu.Start();
        }
        else
        {
            ShowError("No account found with that email and password combination.");
            Console.WriteLine("Press Enter to try again.");
            Console.ReadLine();
            
            Start();
        }
    }

    private static string PromptInput(string label)
    {
        Console.Write($"{label}: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public static string AskForPassword()
    {
        Console.Write("Password: ");

        StringBuilder pass = new StringBuilder();

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (pass.Length > 0)
                {
                    pass.Remove(pass.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                pass.Append(keyInfo.KeyChar);
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return pass.ToString();
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
}