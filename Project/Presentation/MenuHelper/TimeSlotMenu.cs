public static class TimeSlotMenu
{
    public static string Run()
    {
        List<string> options = new List<string>
        {
            "5:00 PM",
            "9:30 PM",
        };

        while (true)
        {
            int choice = MenuHelper.Run(options, "Please select an time slot:");

            switch (choice)
            {
                case 0:
                    return "5:00 PM";
                    
                case 1:
                    return "9:30 PM";
                    
                    
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}