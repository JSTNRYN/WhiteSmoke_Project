using Microsoft.Data.Sqlite;

using Dapper;


public class UserAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string Table = "User";

    public long Write(UserModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, firstName, lastName, phonenumber, role) VALUES (@EmailAddress, @Password, @FirstName, @LastName, @PhoneNumber, @Role) RETURNING id";
        _connection.Open();
        long id = _connection.ExecuteScalar<long>(sql, account);
        _connection.Close();

        return id;
    }

    public static UserModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        _connection.Open();
        var user = _connection.QueryFirstOrDefault<UserModel>(sql, new { Email = email });
        _connection.Close();
        return user;
    }

    public void Update(UserModel account)
    {
        string sql = $"UPDATE {Table} SET email = @EmailAddress, password = @Password, firstName = @FirstName, lastName = @LastName, phonenumber = @PhoneNumber, role = @Role WHERE id = @Id";
        _connection.Open();
        _connection.Execute(sql, account);
        _connection.Close();
    }

    public static void Delete(UserModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Open();
        _connection.Execute(sql, new { Id = account.Id });
        _connection.Close();
    }
    public static UserModel? GetById(int id)
    {
        string sql = $"SELECT id, firstName, lastName, phonenumber, email AS EmailAddress, role FROM {Table} WHERE id = @id";
        _connection.Open();
        var user = _connection.QueryFirstOrDefault<UserModel>(sql, new { id = id });
        _connection.Close();
        return user;
    }

    public static List<UserModel> GetAllCustomers_ExceptPassword()
    {
        string sql = $"SELECT id, firstName, lastName, phonenumber, email AS EmailAddress, role FROM {Table}";
        _connection.Open();
        List<UserModel> customers = _connection.Query<UserModel>(sql).ToList();
        _connection.Close();
        return customers;
    }

    public static void UpdateFirstName(UserModel customer, string newFirstName)
    {
        string sql = $"UPDATE {Table} SET firstName = @NewFirstName WHERE id = @Id";
        _connection.Open();
        _connection.Execute(sql, new { NewFirstName = newFirstName, Id = customer.Id });
        _connection.Close();
    }

    public static void UpdateLastName(UserModel customer, string newLastName)
    {
        string sql = $"UPDATE {Table} SET lastName = @NewLastName WHERE id = @Id";
        _connection.Open();
        _connection.Execute(sql, new { NewLastName = newLastName, Id = customer.Id });
        _connection.Close();
    }

    public static void UpdatePhoneNumber(UserModel customer, string newPhoneNumber)
    {
        string sql = $"UPDATE {Table} SET phonenumber = @NewPhoneNumber WHERE id = @Id";
        _connection.Open();
        _connection.Execute(sql, new { NewPhoneNumber = newPhoneNumber, Id = customer.Id });
        _connection.Close();
    }

    public static void UpdateEmail(UserModel customer, string newEmail)
    {
        string sql = $"UPDATE {Table} SET email = @NewEmail WHERE id = @Id";
        _connection.Open();
        _connection.Execute(sql, new { NewEmail = newEmail, Id = customer.Id });
        _connection.Close();
    }

}
