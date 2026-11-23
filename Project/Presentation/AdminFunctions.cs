public static class AdminFunctions
{
    public static void ShowCustomers()
    {
        List<UserModel> customers = new List<UserModel>();
        foreach (UserModel user in UserLogic.GetAllCustomers_ExceptPassword())
        {
            if (user != null && user.Id != UserLogic.CurrentAccount.Id)
            {
                customers.Add(user);
            }
        }

        Console.Clear();
        if (customers.Count == 0)
        {
            Console.WriteLine("No customers found.");
            Console.WriteLine("Press any key to return to the admin menu...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"List of Customers (Amount: {customers.Count}):");
        foreach (UserModel customer in customers)
        {
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

            if (customer == null || customerId == UserLogic.CurrentAccount.Id)
            {
                Console.Clear();
                Console.WriteLine("No customer found with the provided ID.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                EditCustomer(customer);
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
                    Console.Clear();
                    Console.WriteLine("Invalid name. It must be at least 2 characters. Please try again.");
                    Console.Write("Enter new first name: ");
                    newFirstName = Console.ReadLine();
                }
                UserLogic.UpdateFirstName(customer, newFirstName.Trim());
                Console.WriteLine("First name updated successfully.");
                Console.Clear();
                return;
            }
            else if (option == "2")
            {
                Console.Write("Enter new last name: ");
                string newLastName = Console.ReadLine();
                while (!UserLogic.ValidateName(newLastName))
                {
                    Console.Clear();
                    Console.WriteLine("Invalid name. It must be at least 2 characters. Please try again.");
                    Console.Write("Enter new last name: ");
                    newLastName = Console.ReadLine();
                }
                UserLogic.UpdateLastName(customer, newLastName.Trim());
                Console.WriteLine("Last name updated successfully.");
                Console.Clear();
                return;
            }
            else if (option == "3")
            {
                Console.Write("Enter new email: ");
                string newEmail = Console.ReadLine();
                while (!UserLogic.ValidateEmail(newEmail))
                {
                    Console.Clear();
                    Console.WriteLine("Invalid email format or email already in use. Please try again.");
                    Console.Write("Enter new email: ");
                    newEmail = Console.ReadLine();
                }
                UserLogic.UpdateEmail(customer, newEmail.Trim());
                Console.WriteLine("Email succesfully updated.");
                Console.Clear();
                return;
            }
            else if (option == "4")
            {
                Console.Write("Enter new phone number: ");
                string newPhoneNumber = Console.ReadLine();
                while (!UserLogic.ValidatePhoneNumber(newPhoneNumber))
                {
                    Console.Clear();
                    Console.WriteLine("Invalid phone number (optional leading +). Please try again.");
                    Console.Write("Enter new phone number: ");
                    newPhoneNumber = Console.ReadLine();
                }
                UserLogic.UpdatePhoneNumber(customer, newPhoneNumber.Trim());
                Console.WriteLine("Phone number updated successfully.");
                Console.Clear();
                return;
            }
            else if (option == "5")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid option. Please choose 1 to 5.");
                Console.Clear();
            }
        }
    }
}