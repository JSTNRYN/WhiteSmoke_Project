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
        public static void ShowReservations()
    {
        Console.Clear();
        List<ReservationModel> reservations = ReservationLogic.GetAll();

        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"List of Reservations (Total: {reservations.Count}):");
        Console.WriteLine(new string('-', 65));
        foreach (ReservationModel res in reservations)
        {
            Console.WriteLine($"ID: #{res.Id,-4} | Date: {res.Date,-10} | Time: {res.TimeSlot,-8} | Guests: {res.PartySize,-2} | Table: #{res.TableId}");
        }
        Console.WriteLine(new string('-', 65));

        Console.WriteLine("Enter the ID of the reservation you want to edit, or type 'q' to return:");
        string? input = Console.ReadLine()?.Trim();
        if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (long.TryParse(input, out long reservationId))
        {
            ReservationModel? reservation = ReservationLogic.GetById(reservationId);
            if (reservation == null)
            {
                Console.WriteLine("No reservation found with the provided ID.");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
            }
            else
            {
                EditReservation(reservation);
            }
        }
        else
        {
            Console.WriteLine("Invalid ID entered.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }

    private static void EditReservation(ReservationModel reservation)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Editing Reservation #{reservation.Id}");
            Console.WriteLine($"1. Date:       {reservation.Date}");
            Console.WriteLine($"2. Time:       {reservation.TimeSlot}");
            Console.WriteLine($"3. Guests:     {reservation.PartySize}");
            Console.WriteLine($"4. Table:      #{reservation.TableId}");
            Console.WriteLine("5. Cancel this reservation (Delete)");
            Console.WriteLine("6. Return to menu");
            Console.Write("Choose an option: ");
            string? option = Console.ReadLine()?.Trim();

            if (option == "1")
            {
                Console.Write("Enter new date (dd-MM-yyyy): ");
                string newDate = Console.ReadLine() ?? string.Empty;
                if (!ReservationLogic.IsValidDateFormat(newDate) || !ReservationLogic.IsValidateDate(newDate))
                {
                    Console.WriteLine("Invalid date or date is in the past! Press any key...");
                    Console.ReadKey();
                    continue;
                }
                reservation.Date = newDate;
                ReservationLogic.Update(reservation.Id, reservation.Date, reservation.TimeSlot, (int)reservation.PartySize, reservation.TableId);
                Console.WriteLine("Date updated successfully! Press any key...");
                Console.ReadKey();
            }
            else if (option == "2")
            {
                Console.WriteLine("Choose new time:\n1) 5:00 PM\n2) 9:30 PM");
                string timeChoice = Console.ReadLine()?.Trim() ?? string.Empty;
                if (timeChoice != "1" && timeChoice != "2")
                {
                    Console.WriteLine("Invalid choice! Press any key...");
                    Console.ReadKey();
                    continue;
                }
                reservation.TimeSlot = timeChoice == "1" ? "5:00 PM" : "9:30 PM";
                ReservationLogic.Update(reservation.Id, reservation.Date, reservation.TimeSlot, (int)reservation.PartySize, reservation.TableId);
                Console.WriteLine("Time updated successfully! Press any key...");
                Console.ReadKey();
            }
            else if (option == "3")
            {
                Console.Write("Enter new number of guests (1-6): ");
                if (!int.TryParse(Console.ReadLine(), out int guests) || !SeatCount.WithinRange(guests))
                {
                    Console.WriteLine("Invalid number of guests! Must be 1-6. Press any key...");
                    Console.ReadKey();
                    continue;
                }
                reservation.PartySize = guests;
                ReservationLogic.Update(reservation.Id, reservation.Date, reservation.TimeSlot, (int)reservation.PartySize, reservation.TableId);
                Console.WriteLine("Guests updated successfully! Press any key...");
                Console.ReadKey();
            }
            else if (option == "4")
            {
                Console.Write("Enter new table number: ");
                if (!long.TryParse(Console.ReadLine(), out long tableId))
                {
                    Console.WriteLine("Invalid table number! Press any key...");
                    Console.ReadKey();
                    continue;
                }
                reservation.TableId = tableId;
                ReservationLogic.Update(reservation.Id, reservation.Date, reservation.TimeSlot, (int)reservation.PartySize, reservation.TableId);
                Console.WriteLine("Table updated successfully! Press any key...");
                Console.ReadKey();
            }
            else if (option == "5")
            {
                Console.Write("Are you sure you want to cancel/delete this reservation? (yes/no): ");
                string? confirm = Console.ReadLine()?.Trim().ToLower();
                if (confirm == "yes" || confirm == "y")
                {
                    ReservationLogic.Delete(reservation.Id);
                    Console.WriteLine("Reservation cancelled successfully! Press any key...");
                    Console.ReadKey();
                    return;
                }
            }
            else if (option == "6")
            {
                return;
            }
        }
    }
}