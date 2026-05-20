using MarvelManager.Extensions;
using MarvelManager.Helpers;
using MarvelManager.Models;
using Npgsql;

namespace MarvelManager.Services;

public class UserService
{
    private readonly string _connectionString;

    public UserService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public User? GetByEmail(string email)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = """
            SELECT id, name, email, password_hash, role
            FROM users
            WHERE email = @email
        """;

        command.AddParameter("@email", email);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            PasswordHash = reader.GetString(3),
            Role = reader.GetString(4)
        };
    }

    public void Register(string name, string email, string password)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO users (name, email, password_hash, role)
            VALUES (@name, @email, @passwordHash, 'User')
        """;

        command.AddParameter("@name", name);
        command.AddParameter("@email", email);
        command.AddParameter("@passwordHash", PasswordHelper.HashPassword(password));

        command.ExecuteNonQuery();
    }

    public bool ValidateLogin(string email, string password, out User? user)
    {
        user = GetByEmail(email);

        if (user == null)
            return false;

        return PasswordHelper.VerifyPassword(password, user.PasswordHash);
    }
}