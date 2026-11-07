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
        EditCustomer();

    }

    public static void EditCustomer()
    {
        Console.Write("Enter the ID of the customer you want to edit or 'q' to quit: ");
        string input = Console.ReadLine();
        if (input?.ToLower() == "q")
        {
            return;
        }
        if (int.TryParse(input, out int customerId))
        {
            UserModel customer = UserLogic.GetCustomerById(customerId);
            if (customer != null)
            {
                Console.WriteLine($"Editing Customer: {customer.FirstName} {customer.LastName}");
                Console.WriteLine("1. Update First Name");
                Console.WriteLine("2. Update Last Name");
                Console.WriteLine("3. Update Email");
                Console.WriteLine("4. Update Phone Number");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter new first name: ");
                        string newFirstName = Console.ReadLine();
                        if(UserLogic.ValidateName(newFirstName))
                        {
                            UserLogic.UpdateFirstName(customer, newFirstName);
                        }
                        else
                        {
                            Console.WriteLine("Invalid first name. Please try again.");
                        }
                        break;
                    case "2":
                        Console.Write("Enter new last name: ");
                        string newLastName = Console.ReadLine();
                        if(UserLogic.ValidateName(newLastName))
                        {
                            UserLogic.UpdateLastName(customer, newLastName);
                        }
                        else
                        {
                            Console.WriteLine("Invalid last name. Please try again.");
                        }
                        break;
                    case "3":
                        Console.Write("Enter new email: ");
                        string newEmail = Console.ReadLine();
                        if(UserLogic.ValidateEmail(newEmail))
                        {
                            UserLogic.UpdateEmail(customer, newEmail);
                        }
                        else
                        {
                            Console.WriteLine("Invalid email. Please try again.");
                        }
                        break;
                    case "4":
                        Console.Write("Enter new phone number: ");
                        string newPhoneNumber = Console.ReadLine();
                        if(UserLogic.ValidatePhoneNumber(newPhoneNumber))
                        {
                            UserLogic.UpdatePhoneNumber(customer, newPhoneNumber);
                        }
                        else
                        {
                            Console.WriteLine("Invalid phone number. Please try again.");
                        }
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }
}