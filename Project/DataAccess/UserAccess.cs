using Microsoft.Data.Sqlite;

using Dapper;


public class UserAccess
{
    private static string Table = "User";

    public long Write(UserModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, firstName, lastName, phonenumber, role, DateOfBirth) VALUES (@EmailAddress, @Password, @FirstName, @LastName, @PhoneNumber, @Role, @DateOfBirth) RETURNING id";
        using var connection = Database.CreateConnection();
        connection.Open();
        long id = connection.ExecuteScalar<long>(sql, account);

        return id;
    }

    public static UserModel? GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        using var connection = Database.CreateConnection();
        connection.Open();
        var user = connection.QueryFirstOrDefault<UserModel>(sql, new { Email = email });
        return user;
    }

    public void Update(UserModel account)
    {
        string sql = $"UPDATE {Table} SET email = @EmailAddress, password = @Password, firstName = @FirstName, lastName = @LastName, phonenumber = @PhoneNumber, role = @Role WHERE id = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(sql, account);
    }

    public static void Delete(UserModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(sql, new { Id = account.Id });
    }
    public static UserModel? GetById(int id)
    {
        string sql = $"SELECT id, firstName, lastName, phonenumber, email AS EmailAddress, role FROM {Table} WHERE id = @id";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.QueryFirstOrDefault<UserModel>(sql, new { id });
    }

    public static List<UserModel> GetAllCustomers_ExceptPassword()
    {
        string sql = $"SELECT id, firstName, lastName, phonenumber, email AS EmailAddress, role FROM {Table}";
        using var connection = Database.CreateConnection();
        connection.Open();
        return connection.Query<UserModel>(sql).ToList();
    }

    public static void UpdateFirstName(UserModel customer, string newFirstName)
    {
        string sql = $"UPDATE {Table} SET firstName = @NewFirstName WHERE id = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(sql, new { NewFirstName = newFirstName, Id = customer.Id });
    }

    public static void UpdateLastName(UserModel customer, string newLastName)
    {
        string sql = $"UPDATE {Table} SET lastName = @NewLastName WHERE id = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(sql, new { NewLastName = newLastName, Id = customer.Id });
    }

    public static void UpdatePhoneNumber(UserModel customer, string newPhoneNumber)
    {
        string sql = $"UPDATE {Table} SET phonenumber = @NewPhoneNumber WHERE id = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(sql, new { NewPhoneNumber = newPhoneNumber, Id = customer.Id });
    }

    public static void UpdateEmail(UserModel customer, string newEmail)
    {
        string sql = $"UPDATE {Table} SET email = @NewEmail WHERE id = @Id";
        using var connection = Database.CreateConnection();
        connection.Open();
        connection.Execute(sql, new { NewEmail = newEmail, Id = customer.Id });
    }

}
