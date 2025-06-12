using Microsoft.Data.SqlClient;
using TaskManagerAPI.Data.Interface;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.Enums;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories.Implementations;

public class UserRepository(ISqlConnectionFactory connectionFactory) : IUserRepository
{
    private readonly ISqlConnectionFactory _connectionFactory = connectionFactory;

    public async Task<IEnumerable<User?>> GetAllUsers(CancellationToken cancellationToken)
    {
        const string query = $"SELECT Id, Name, Email, PasswordHash, Role, CreatedAt " +
            $"FROM Users";

        var userList = new List<User>();

        using var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);

        try
        {
            SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync())
            {
                var user = new User()
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    PasswordHash = reader["PasswordHash"].ToString()!,
                    Role = (Role)Enum.Parse(typeof(Role), reader["Role"].ToString()!),
                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()!)
                };

                userList.Add(user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }

        return userList;
    }

    public async Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        const string query = $"SELECT Id, Name, Email, PasswordHash, Role, CreatedAt " +
            $"FROM Users " +
            $"WHERE Id = @userId";

        var user = new User();

        using var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);
        try
        {
            command.Parameters.AddWithValue("@userId", userId);

            SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                user = new User()
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    PasswordHash = reader["PasswordHash"].ToString()!,
                    Role = (Role)Enum.Parse(typeof(Role), reader["Role"].ToString()!),
                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()!)
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }

        return user;
    }

    public async Task<User?> CreateUser(User user, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        const string query = "INSERT INTO Users (Id, Name, Email, PasswordHash, Role, CreatedAt) " +
                     "VALUES (@Id, @Name, @Email, @PasswordHash, @Role, @CreatedAt)";

        var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);
        try
        {
            command.Parameters.AddWithValue("@Id", userId);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@Role", user.Role.ToString());
            command.Parameters.AddWithValue("@CreatedAt", createdAt);

            await command.ExecuteNonQueryAsync(cancellationToken);

            return new User()
            {
                Id = userId,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role,
                CreatedAt = createdAt
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> UpdateUser(User user, CancellationToken cancellationToken)
    {
        const string query = "UPDATE Users " +
            "SET Name = @Name, Email = @Email, PasswordHash = @PasswordHash, Role = @Role " +
            "WHERE Id = @Id";

        var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);

        try
        {
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@Role", user.Role.ToString());
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred {ex.Message}");
            throw;
        }

        return user;
    }

    public async Task<User?> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetUserById(userId, cancellationToken);
        if (user is null) return null;

        const string query = "DELETE FROM Users WHERE Id = @userId";

        await using var connection = _connectionFactory.GetConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(query, connection);
        try
        {
            command.Parameters.AddWithValue("@userId", userId);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }

        return user;
    }
}