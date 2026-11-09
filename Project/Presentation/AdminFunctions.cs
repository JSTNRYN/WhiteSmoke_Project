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
        Console.WriteLine("Enter the ID of the customer you want to edit, or type 'q' to return to the main menu:");
        string input = Console.ReadLine();
        if (input.ToLower() == "q")
        {
            return;
        }

        if (int.TryParse(input, out int customerId))
        {
            UserModel customer = UserLogic.GetCustomerById(customerId);
            if (customer != null)
            {
                EditCustomer(customer);
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }

    private static void EditCustomer(UserModel customer)
    {
        while (true)
        {
            Console.WriteLine($"Editing Customer: {customer.FirstName} {customer.LastName}");
            Console.WriteLine("1. Update First Name");
            Console.WriteLine("2. Update Last Name");
            Console.WriteLine("3. Update Email");
            Console.WriteLine("4. Update Phone Number");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            if (option == "1")
            {
                Console.Write("Enter new first name: ");
                string newFirstName = Console.ReadLine();
                while (!UserLogic.ValidateName(newFirstName))
                {
                    Console.WriteLine("Invalid name. It must be at least 2 characters. Please try again.");
                    Console.Write("Enter new first name: ");
                    newFirstName = Console.ReadLine();
                }
                UserLogic.UpdateFirstName(customer, newFirstName.Trim());
                Console.WriteLine("First name updated successfully.");
                return;
            }
            else if (option == "2")
            {
                Console.Write("Enter new last name: ");
                string newLastName = Console.ReadLine();
                while (!UserLogic.ValidateName(newLastName))
                {
                    Console.WriteLine("Invalid name. It must be at least 2 characters. Please try again.");
                    Console.Write("Enter new last name: ");
                    newLastName = Console.ReadLine();
                }
                UserLogic.UpdateLastName(customer, newLastName.Trim());
                Console.WriteLine("Last name updated successfully.");
                return;
            }
            else if (option == "3")
            {
                Console.Write("Enter new email: ");
                string newEmail = Console.ReadLine();
                while (!UserLogic.ValidateEmail(newEmail))
                {
                    Console.WriteLine("Invalid email format or email already in use. Please try again.");
                    Console.Write("Enter new email: ");
                    newEmail = Console.ReadLine();
                }
                UserLogic.UpdateEmail(customer, newEmail.Trim());
                Console.WriteLine("Email updated successfully.");
                return;
            }
            else if (option == "4")
            {
                Console.Write("Enter new phone number: ");
                string newPhoneNumber = Console.ReadLine();
                while (!UserLogic.ValidatePhoneNumber(newPhoneNumber))
                {
                    Console.WriteLine("Invalid phone number (8 to 15 digits, optional leading +). Please try again.");
                    Console.Write("Enter new phone number: ");
                    newPhoneNumber = Console.ReadLine();
                }
                UserLogic.UpdatePhoneNumber(customer, newPhoneNumber.Trim());
                Console.WriteLine("Phone number updated successfully.");
                return;
            }
            else if (option == "5")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid option. Please choose 1 to 5.");
            }
        }
    }
}