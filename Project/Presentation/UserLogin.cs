static class UserLogin
{
    private static readonly UserLogic UserLogic = new();

    public static void Start()
    {
        Console.Write("Please enter your e-mail: ");
        string? email = Console.ReadLine();
        string? password = UserLogic.ReadPassword();

        UserModel? account = email != null && password != null
            ? UserLogic.CheckLogin(email, password)
            : null;

        if (account == null) ShowError("No account found with that email and password combination.");
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }
}
