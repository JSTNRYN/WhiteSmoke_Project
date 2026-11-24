public static class CustomerFunctions
{
    public static void DeleteAccount()
    {
        UserModel? currentUser = UserLogic.CurrentAccount;
        if (currentUser == null)
        {
            Console.WriteLine("No user is currently logged in.");
            return;
        }

        bool hasReservations = ReservationLogic.GetWithUserID(currentUser.Id).Count > 0;

        if (hasReservations)
        {
            Console.WriteLine("Your current account has existing reservations. Deleting this account will also delete the reservations.");
            Console.WriteLine($"Are you sure you want to delete the account: {currentUser.FirstName} {currentUser.LastName}? (y/n): ");
        }
        else
        {
            Console.WriteLine($"Are you sure you want to delete the account: {currentUser.FirstName} {currentUser.LastName}? (y/n): ");
        }
        string confirmation = Console.ReadLine()?.Trim().ToLower();

        if (confirmation == "y")
        {

            ReservationLogic.DeleteReservationsByUserId(currentUser.Id);

            bool success = UserLogic.DeleteCustomer(currentUser);
            if (success)
            {
                Console.WriteLine("Your account has been deleted successfully.");
                UserLogic.Logout();
            }
            else
            {
                Console.WriteLine("An error occurred while trying to delete your account.");
            }
        }
        else if (confirmation == "n")
        {
            Console.WriteLine("Account deletion canceled.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
        }
    }
}