static class Menu
{

    public static void ShowRestaurantName()
    {
        Console.WriteLine("blah");
        Console.Clear();
        Console.WriteLine(@"
                                    (                             
    (  (         )        )        )\ )                 )        
    )\))(   ' ( /( (   ( /(   (   (()/(    )         ( /(    (   
    ((_)()\ )  )\()))\  )\()) ))\   /(_))  (      (   )\())  ))\  
    _(())\_)()((_)\((_)(_))/ /((_) (_))    )\  '  )\ ((_)\  /((_) 
    \ \((_)/ /| |(_)(_)| |_ (_))   / __| _((_))  ((_)| |(_)(_))   
    \ \/\/ / | ' \ | ||  _|/ -_)  \__ \| '  \()/ _ \| / / / -_)  
    \_/\_/  |_||_||_| \\__|\___|  |___/|_|_|_| \___/|_\_\ \___|
        ");
    }
    public static void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to sign up");
        Console.WriteLine("Enter 3 to do something else in the future");

        string input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            // UserSignUp.Start();
        }
        else if (input == "3")
        {
            Console.WriteLine("This feature is not yet implemented");
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}