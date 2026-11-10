using Dapper;

public class UserAccess
{
    public long Create(UserModel account)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.ExecuteScalar<long>(@"
            INSERT INTO User (email, password, firstName, lastName, phonenumber, role, DateOfBirth)
            VALUES (@EmailAddress, @Password, @FirstName, @LastName, @PhoneNumber, @Role, @DateOfBirth)
            RETURNING id", account);
    }

    public static UserModel? GetByEmail(string email)
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<UserModel>("SELECT * FROM User WHERE email = @Email", new { Email = email });
    }
}
