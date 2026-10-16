public class UserModel
{

    public int Id { get; set; }
    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Role { get; set; }
    
    public UserModel()
    {
    }

    public UserModel(int id, string email, string password, string firstName, string lastName, string phoneNumber, string role)
    {
        Id = id;
        EmailAddress = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Role = role;
    }
}
