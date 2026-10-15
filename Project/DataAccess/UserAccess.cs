using Microsoft.Data.Sqlite;

using Dapper;


public class UserAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string Table = "User";

    public void Write(UserModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, firstName, lastName, phonenumber, role) VALUES (@EmailAddress, @Password, @FirstName, @LastName, @PhoneNumber, @Role)";
        _connection.Open();
        _connection.Execute(sql, account);
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