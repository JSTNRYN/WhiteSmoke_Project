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

    public static void UpdateFirstName(long id, string newName)
    {
        string sql = $"UPDATE {Table} SET firstName = @FirstName WHERE id = @Id";
    
        _connection.Open();
        _connection.Execute(sql, new { FirstName = newName, Id = id });
        _connection.Close();
    }
    
    public static void UpdateLastName(long id, string newLastName)
    {
        string sql = $"UPDATE {Table} SET lastName = @LastName WHERE id = @Id";
    
        _connection.Open();
        _connection.Execute(sql, new { LastName = newLastName, Id = id });
        _connection.Close();
    }
    
    public static void UpdateEmail(long id, string newEmail)
    {
        string sql = $"UPDATE {Table} SET email = @EmailAddress WHERE id = @Id";
    
        _connection.Open();
        _connection.Execute(sql, new { EmailAddress = newEmail, Id = id });
        _connection.Close();
    }
    
    public static void UpdatePhone(long id, string newPhoneNumber)
    {
        string sql = $"UPDATE {Table} SET phonenumber = @PhoneNumber WHERE id = @Id";
    
        _connection.Open();
        _connection.Execute(sql, new { PhoneNumber = newPhoneNumber, Id = id });
        _connection.Close();
    }

    public static UserModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        _connection.Open();
        var user = _connection.QueryFirstOrDefault<UserModel>(sql, new { Email = email });
        _connection.Close();
        return user;
    }

    public static UserModel? GetById(int id)
    {
        string sql = $"SELECT id, firstName, lastName, phonenumber, email AS EmailAddress, role FROM {Table} WHERE id = @id";
        _connection.Open();
        var user = _connection.QueryFirstOrDefault<UserModel>(sql, new { id = id });
        _connection.Close();
        return user;
    }
    
    public static List<UserModel> GetAllCustomers()
    {
        string sql = $"SELECT id, firstName, lastName, phonenumber, email AS EmailAddress, role FROM {Table}";
        _connection.Open();
        var users = _connection.Query<UserModel>(sql).ToList();
        _connection.Close();
        return users;
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



}
