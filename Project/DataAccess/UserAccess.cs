using Microsoft.Data.Sqlite;

using Dapper;


public class UserAccess
{
    private static string Table = "User";

    public long Write(UserModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, firstName, lastName, phonenumber, role) VALUES (@EmailAddress, @Password, @FirstName, @LastName, @PhoneNumber, @Role) RETURNING id";
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



}
