using Project.Presentation;

static class Menu
{
    private static readonly string[] _guestOptions = ["Log in", "Sign up", "View Menu", "About us"];
    private static readonly string[] _userOptions = ["Make a reservation", "View your reservations", "View Menu", "About us", "Edit personal details", "Log out"];
    private static readonly string[] _adminOptions = ["Make a reservation", "View your reservations", "View Menu", "About us", "Edit personal details", "Log out", "View admin menu"];

    public static void ShowRestaurantName()
    {
        if (!Console.IsOutputRedirected) Console.Clear();
        Console.WriteLine(@"
        (
        (  (         )        )        )\ )                 )
        )\))(   ' ( /( (   ( /(   (   (()/(    )         ( /(    (
        ((_)()\ )  )\()))\  )\()) ))\   /(_))  (      (   )\())  ))\
        _(())\_)()((_)(_))/ /((_) (_))    )\  '  )\ ((_\  /((_)
        \ \((_)/ /| |(_)(_) | |_ (_))   / __| _((_))  ((_)| |(_)(_))
         \ \/\/ / | ' \ | | |  _|/ -_)  \__ \| '  \()/ _ \| / / / -_)
          \_/\_/  |_||_||_| \\__|\___|  |___/|_|_|_| \___/|_\_\ \___|
");
    }

    public static void Start()
    {
        while (true)
        {
            ShowRestaurantName();

            bool loggedIn = UserLogic.IsLoggedIn();
            bool isAdmin = loggedIn && UserLogic.IsAdmin();

            if (loggedIn)
            {
                var account = UserLogic.CurrentAccount!;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Logged in as: {account.FirstName} {account.LastName}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are currently not logged in.");
                Console.ResetColor();
            }

            if (isAdmin)
            {
                HandleLoggedInChoice(InteractiveMenu(_adminOptions), isAdmin);
            }
            else if (loggedIn)
            {
                HandleLoggedInChoice(InteractiveMenu(_userOptions), isAdmin);
            }
            else
            {
                HandleGuestChoice(InteractiveMenu(_guestOptions));
            }
        }
    }

    private static void HandleGuestChoice(int choice)
    {
        switch (choice)
        {
            case 0: UserLogin.Start(); break;
            case 1: UserSignUp.Start(); break;
            case 2: ShowFoodMenu(); break;
            case 3: About(); break;
        }
    }

    private static void HandleLoggedInChoice(int choice, bool isAdmin)
    {
        switch (choice)
        {
            case 0: MakeReservation.Start(); break;
            case 1: MyReservation.ShowReservations(UserLogic.CurrentAccount!.Id); break;
            case 2: ShowFoodMenu(); break;
            case 3: About(); break;
            case 4: EditUserDetails(UserLogic.CurrentAccount!); break;
            case 5: UserLogic.Logout(); break;
            case 6 when isAdmin: AdminFunctions.AdminMenu(); break;
        }
    }

    private static void ShowFoodMenu()
    {
        List<MenuItemModel> list = MenuLogic.GetNonAdminMenuItems();
        MenuView.PrintItems(list);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Press Enter to return...");
        Console.ResetColor();
        Console.ReadLine();
        Console.Clear();
    }

    private static void EditUserDetails(UserModel user)
    {
        string newFirstName = user.FirstName;
        string newLastName = user.LastName;
        string newEmail = user.EmailAddress;
        string newDob = $"{user.DateOfBirth:dd-MM-yyyy}";
        string newPhone = user.PhoneNumber;
        string newPassword = "";

        while (true)
        {
            ShowRestaurantName();
            Console.WriteLine("Choose what you want to change. Pick 'Save changes' when you are done.");

            string passwordText = "Change your password";
            if (newPassword != "")
            {
                passwordText = "Change your password (changed)";
            }
            string[] options =
            [
                $"First name: {newFirstName}",
                $"Last name: {newLastName}",
                $"Email address: {newEmail}",
                $"Date of birth (DD-MM-YYYY): {newDob}",
                $"Phone number: {newPhone}",
                passwordText,
                "Save changes",
                "Back without saving"
            ];

            switch (InteractiveMenu(options))
            {
                case 0: newFirstName = AskNewName("first name"); break;
                case 1: newLastName = AskNewName("last name"); break;
                case 2: newEmail = AskNewEmail(user.EmailAddress); break;
                case 3: newDob = AskNewDob(); break;
                case 4: newPhone = AskNewPhoneNumber(); break;
                case 5:
                    string password = AskNewPassword();
                    if (password != "") newPassword = password;
                    break;
                case 6:
                    if (ConfirmChanges(user, newFirstName, newLastName, newEmail, newDob, newPhone, newPassword != ""))
                    {
                        SaveChanges(user, newFirstName, newLastName, newEmail, newDob, newPhone, newPassword);
                        return;
                    }
                    break;
                case 7: return;
            }
        }
    }

    private static bool ConfirmChanges(UserModel user, string newFirstName, string newLastName, string newEmail, string newDob, string newPhone, bool passwordChanged)
    {
        ShowRestaurantName();

        Console.WriteLine("Current details:");
        Console.WriteLine($"First name:    {user.FirstName}");
        Console.WriteLine($"Last name:     {user.LastName}");
        Console.WriteLine($"Email address: {user.EmailAddress}");
        Console.WriteLine($"Date of birth: {user.DateOfBirth:dd-MM-yyyy}");
        Console.WriteLine($"Phone number:  {user.PhoneNumber}");
        Console.WriteLine();

        Console.WriteLine("New details:");
        Console.WriteLine($"First name:    {newFirstName}");
        Console.WriteLine($"Last name:     {newLastName}");
        Console.WriteLine($"Email address: {newEmail}");
        Console.WriteLine($"Date of birth: {newDob}");
        Console.WriteLine($"Phone number:  {newPhone}");
        if (passwordChanged)
        {
            Console.WriteLine("Password:      changed");
        }
        else
        {
            Console.WriteLine("Password:      not changed");
        }
        int choice = InteractiveMenu(["Confirm", "Go back"]);
        return choice == 0;
    }

    private static void SaveChanges(UserModel user, string newFirstName, string newLastName, string newEmail, string newDob, string newPhone, string newPassword)
    {
        if (newFirstName != user.FirstName) UserLogic.UpdateFirstName(user, newFirstName);
        if (newLastName != user.LastName) UserLogic.UpdateLastName(user, newLastName);
        if (newEmail != user.EmailAddress) UserLogic.UpdateEmail(user, newEmail);
        if (newDob != $"{user.DateOfBirth:dd-MM-yyyy}") UserLogic.UpdateDob(user, newDob);
        if (newPhone != user.PhoneNumber) UserLogic.UpdatePhoneNumber(user, newPhone);
        if (newPassword != "") UserLogic.UpdatePassword(user, newPassword);
    }


    private static string AskNewName(string label)
    {
        while (true)
        {
            Console.Write($"New {label}: ");
            string input = Console.ReadLine()?.Trim() ?? "";
            if (UserLogic.ValidateName(input)) return input;
            ShowInvalidChoice();
        }
    }

    private static string AskNewEmail(string currentEmail)
    {
        while (true)
        {
            Console.Write("New email: ");
            string input = Console.ReadLine()?.Trim() ?? "";
            if (input == currentEmail || UserLogic.ValidateEmail(input)) return input;
            ShowInvalidChoice();
        }
    }

    private static string AskNewDob()
    {
        while (true)
        {
            Console.Write("New date of birth (dd-mm-yyyy): ");
            string input = Console.ReadLine()?.Trim() ?? "";
            if (UserLogic.ValidateDob(input)) return input;
            ShowInvalidChoice();
        }
    }

    private static string AskNewPhoneNumber()
    {
        while (true)
        {
            Console.Write("New phone number: ");
            string input = Console.ReadLine()?.Trim() ?? "";
            if (UserLogic.ValidatePhoneNumber(input)) return input;
            ShowInvalidChoice();
        }
    }

    private static string AskNewPassword()
    {
        string oldPassword = UserLogic.InterceptPassword("Please enter your old password: ");
        if (!UserLogic.CheckIfPasswordsMatch(oldPassword))
        {
            ShowInvalidChoiceWithMsg("Your old password was wrong.");
            return "";
        }
        return UserLogic.InterceptPassword("Please enter your new password: ");
    }

    private static void ShowInvalidChoiceWithMsg(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
        Thread.Sleep(1000);
    }

    private static void ShowInvalidChoice()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid input.");
        Console.ResetColor();
        Thread.Sleep(1000);
    }

    public static void About()
    {
        ShowRestaurantName();
        Console.WriteLine("About\n");
        Console.WriteLine(@"
White Smoke is built around one idea: cook with fire, and let the ingredients do the talking.
Our Pitmasters tend an open fire daily, slowly cooking meat, fish, and vegetables until they develop deep, smoky flavour without losing their freshness.
Every dish is finished light and bright, so the smoke adds depth instead of weight.
We keep the menu honest and the method simple, because good fire-cooked food doesn’t need much else.

Pull up a chair. The fire is already lit.

The crew:
 - Marcus Hale, Head Pitmaster: runs the fire and the cooking schedule.
 - Elena Voss, Pitmaster, Fish and Vegetables: handles the delicate cooks over open flame.
 - Tomás Rivera, Sous Chef: finishes the dishes and keeps the plates light and fresh.
 - Priya Nair, Front of House Manager: seats guests and runs the floor.

            ");

        Console.WriteLine(@"
Welcome to White Smoke, where fire and smoke bring every dish to life.
Our pitmasters slowly cook meat, fish and vegetables over an open fire,
for deep flavour with a light, fresh finish.
Take a seat, the fire is already lit.

Address
  White Smoke
  Wijnhaven 107
  3011 WN Rotterdam
  The Netherlands

Opening hours
  Monday        Closed
  Tuesday       Closed
  Wednesday     5:00 PM - 11:00 PM
  Thursday      5:00 PM - 11:00 PM
  Friday        5:00 PM - 11:00 PM
  Saturday      12:00 PM - 11:00 PM
  Sunday        12:00 PM - 10:00 PM
");

        Console.WriteLine("Press enter to go back");
        Console.ReadLine();
        Console.Clear();
    }

    public static int InteractiveMenu(string[] menuOptions)
    {
        ConsoleKeyInfo key;
        int option = 0;
        int maxMenuOptionNumber = menuOptions.Length - 1;
        bool notSelected = true;
        (int left, int top) = Console.GetCursorPosition();

        while (notSelected)
        {
            Console.SetCursorPosition(left, top);

            Console.WriteLine("Press up/down arrows to go over the options");
            Console.WriteLine("Press enter to confirm option");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                Console.WriteLine($"{(option == i ? $"* {menuOptions[i]}" : $"  {menuOptions[i]}")}");
            }

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    option = (option == maxMenuOptionNumber) ? maxMenuOptionNumber : option + 1;
                    break;
                case ConsoleKey.UpArrow:
                    option = (option == 0) ? 0 : option - 1;
                    break;
                case ConsoleKey.Enter:
                    notSelected = false;
                    break;
            }
        }

        return option;
    }
}