static class UserLogin
{
    private static readonly UserLogic UserLogic = new();

    public static void Start()
    {
        Console.Write("Please enter your e-mail: ");
        string? email = Console.ReadLine();
        string? password = Project.Presentation.UserSignUp.InterceptPassword();

        UserModel? account = email != null && password != null
            ? UserLogic.CheckLogin(email, password)
            : null;

        if (account == null) ShowError("No account found with that email and password combination.");
    }

    public static string InterceptPassword()
    {
        Console.Write("Please enter your password: ");
        string password = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
                continue;
            }
            if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write('*');
            }
        }
        return password;
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
}
