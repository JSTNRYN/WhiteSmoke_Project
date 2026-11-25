public static class AdminFunctions
{
    private static readonly string[] _adminMenuOptions = ["Manage Menu Items", "View and edit the floor plan", "View and manage customers", "View and edit reservations", "Back to main menu"];
    
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
                Console.WriteLine("Email successfully updated.");
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
    public static void ShowReservations()
    {
        while (true)
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            List<ReservationModel> reservations = ReservationLogic.GetAll();

            if (reservations.Count == 0)
            {
                Console.WriteLine("No reservations found.");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== ALL RESERVATIONS ===");
            Console.WriteLine("========================\n");
            Console.WriteLine("Select a reservation you want to view/edit:\n");

            List<string> options = new List<string>();
            foreach (ReservationModel res in reservations)
            {
                options.Add($"Reservation #{res.Id} - {res.Date} ({res.TimeSlot}) - {res.PartySize}p - Table #{res.TableId}");
            }
            options.Add("Back to admin menu");

            int selectedIndex = Menu.InteractiveMenu(options.ToArray());

            if (selectedIndex == options.Count - 1)
            {
                return;
            }

            ReservationModel selectedReservation = reservations[selectedIndex];
            EditReservation(selectedReservation);
        }
    }

    public static void EditReservation(ReservationModel reservation)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== EDITING RESERVATION #{reservation.Id} ===\n");
            Console.WriteLine("Select an option you want to change:\n");

            string[] options = [
                $"Date:       {reservation.Date}",
                $"Time:       {reservation.TimeSlot}",
                $"Guests:     {reservation.PartySize}",
                $"Table:      #{reservation.TableId}",
                "Cancel this reservation (Delete)",
                "Return to reservation list"
            ];

            int choice = Menu.InteractiveMenu(options);

            switch (choice)
            {
                case 0: 
                    Console.Clear();
                    Console.WriteLine($"Current date: {reservation.Date}");
                    Console.Write("Enter new date (dd-MM-yyyy): ");
                    string newDate = Console.ReadLine() ?? string.Empty;
                    if (!ReservationLogic.IsValidDateFormat(newDate) || !ReservationLogic.IsValidateDate(newDate))
                    {
                        Console.WriteLine("\nInvalid date! Changes not saved.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        continue;
                    }
                    reservation.Date = newDate;
                    ReservationLogic.Update(reservation);
                    Console.WriteLine("\nDate updated successfully! Press any key...");
                    Console.ReadKey(true);
                    break;

                case 1:
                    Console.Clear();
                    Console.WriteLine($"Current time: {reservation.TimeSlot}\nChoose new time slot:\n");
                    string[] timeSlots = ["5:00 PM", "9:30 PM"];
                    int timeChoice = Menu.InteractiveMenu(timeSlots);
                    reservation.TimeSlot = timeSlots[timeChoice];
                    ReservationLogic.Update(reservation);
                    Console.WriteLine("\nTime updated successfully! Press any key...");
                    Console.ReadKey(true);
                    break;

                case 2: 
                    Console.Clear();
                    Console.WriteLine($"Current guests: {reservation.PartySize}");
                    Console.Write("Enter new number of guests (1-6): ");
                    if (!int.TryParse(Console.ReadLine(), out int guests) || !SeatCount.WithinRange(guests))
                    {
                        Console.WriteLine("\nInvalid number of guests! Must be between 1 and 6.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        continue;
                    }
                    reservation.PartySize = guests;
                    ReservationLogic.Update(reservation);
                    Console.WriteLine("\nGuests updated successfully! Press any key...");
                    Console.ReadKey(true);
                    break;

                case 3: 
                    Console.Clear();
                    Console.WriteLine($"Current table: #{reservation.TableId}");
                    Console.Write("Enter new table number: ");
                    if (!long.TryParse(Console.ReadLine(), out long tableId))
                    {
                        Console.WriteLine("\nInvalid table number! Press any key...");
                        Console.ReadKey(true);
                        continue;
                    }
                    reservation.TableId = tableId;
                    ReservationLogic.Update(reservation);
                    Console.WriteLine("\nTable updated successfully! Press any key...");
                    Console.ReadKey(true);
                    break;

                case 4: 
                    Console.Clear();
                    Console.WriteLine($"Reservation details to cancel:");
                    Console.WriteLine($"Date: {reservation.Date} | Time: {reservation.TimeSlot} | Guests: {reservation.PartySize} | Table: #{reservation.TableId}");
                    Console.Write("\nAre you sure you want to cancel/delete this reservation? (yes/no): ");
                    string? confirm = Console.ReadLine()?.Trim().ToLower();
                    if (confirm == "yes" || confirm == "y")
                    {
                        ReservationLogic.Delete(reservation.Id);
                        Console.WriteLine("\nReservation cancelled successfully! Press any key...");
                        Console.ReadKey(true);
                        return;
                    }
                    break;

                case 5: // Return
                    return;
            }
        }
    }

    public static void AdminMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("ADMIN MENU\n");

            int choice = Menu.InteractiveMenu(_adminMenuOptions);
            switch (choice)
            {
                case 0:
                    MenuView.ShowMenu();
                    break;
                case 1:
                    FloorPlanUI.Show();
                    break;
                case 2:
                    ShowCustomers();
                    break;
                case 3:
                    ShowReservations();
                    break;
                case 4:
                    return;
            }
        }
    }
}