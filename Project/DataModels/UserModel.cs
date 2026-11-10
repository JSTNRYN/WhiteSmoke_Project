public class UserModel
{

    public Int64 Id { get; set; }
    public string EmailAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    
    public UserModel()
    {
    }

    public UserModel(Int64 id, string email, string password, string firstName, string lastName, string phoneNumber, string role, DateTime dateOfBirth)
    {
        Id = id;
        EmailAddress = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Role = role;
        DateOfBirth = dateOfBirth;
    }
}
