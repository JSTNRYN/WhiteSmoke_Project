public static class AdminFunctions
{
    public static void ShowCustomers()
    {
        List<UserModel> customers = UserLogic.GetAllCustomers_ExceptPassword();
        Console.Clear();
        Console.WriteLine("List of Customers (Amount: {customers.Count}):");
        foreach (var customer in customers)
        {
            if (customer == null)
            {
                Console.WriteLine("Customer is null.");
                continue;
            }
            Console.WriteLine($"ID: {customer.Id}");
            Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");
            Console.WriteLine($"Email: {customer.EmailAddress}");
            Console.WriteLine($"Phone Number: {customer.PhoneNumber}");
            Console.WriteLine("-----------------------------");
        }
        Console.WriteLine("Enter the ID of the customer you want to edit.");
        string input = Console.ReadLine();

    }
}