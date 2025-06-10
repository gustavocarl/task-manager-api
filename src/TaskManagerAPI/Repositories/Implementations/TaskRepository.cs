using Microsoft.Data.SqlClient;
using System.Net.WebSockets;
using TaskManagerAPI.Data.Interface;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories.Implementations;

public class TaskRepository : ITaskRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public TaskRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Tasks>> GetAllTasksByUser(Guid userId, CancellationToken cancellationToken)
    {
        const string query = $"SELECT Id, Title, [Description], Priority, [Status], DueDate, " +
            $"UserId, CreatedAt, UpdatedAt " +
            $"FROM Tasks " +
            $"Where UserId = @userId";

        var taskList = new List<Tasks>();

        using var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@userId", userId);

        SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var task = new Tasks
            {
                Id = Guid.Parse(reader["Id"].ToString()!),
                Title = reader["Title"].ToString()!,
                Description = reader["Description"].ToString(),
                Priority = (Models.Enums.Priority)Enum.Parse(typeof(Models.Enums.Priority), reader["Priority"].ToString()!),
                Status = (Models.Enums.Status)Enum.Parse(typeof(Models.Enums.Status), reader["Status"].ToString()!),
                DueTime = reader["DueDate"] != DBNull.Value ? DateTime.Parse(reader["DueDate"].ToString()!) : null,
                UserId = Guid.Parse(reader["UserId"].ToString()!),
                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()!),
                UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString()!)
            };
            taskList.Add(task);
        }
        await connection.CloseAsync();

        return taskList;
    }

    public async Task<Tasks?> GetTaskById(Guid taskId, Guid userId, CancellationToken cancellationToken)
    {
        const string query = "SELECT Id, Title, [Description], Priority, [Status], DueDate, " +
            "UserId, CreatedAt, UpdatedAt " +
            "FROM Tasks " +
            "WHERE Id = @taskId AND UserId = @userId";

        var task = new Tasks();

        using var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@taskId", taskId);
        command.Parameters.AddWithValue("@userId", userId);

        SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            task = new Tasks
            {
                Id = Guid.Parse(reader["Id"].ToString()!),
                Title = reader["Title"].ToString()!,
                Description = reader["Description"].ToString(),
                Priority = (Models.Enums.Priority)Enum.Parse(typeof(Models.Enums.Priority), reader["Priority"].ToString()!),
                Status = (Models.Enums.Status)Enum.Parse(typeof(Models.Enums.Status), reader["Status"].ToString()!),
                DueTime = reader["DueDate"] != DBNull.Value ? DateTime.Parse(reader["DueDate"].ToString()!) : null,
                UserId = Guid.Parse(reader["UserId"].ToString()!),
                CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()!),
                UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString()!)
            };
        }
        return task;
    }

    public async Task<Tasks?> CreateTask(Tasks task, Guid userId, CancellationToken cancellationToken)
    {
        const string query = "INSERT INTO Tasks ( Title, Description, Priority, " +
            "Status, DueDate, UserId, CreatedAt, UpdatedAt " +
            " ) VALUES (   " +
            "@Title, @Description, @Priority, " +
            "@Status, @DueDate, @UserId, @CreatedAt, @UpdatedAt " +
            " )";

        using var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);

        try
        {
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@Status", task.Status);
            command.Parameters.AddWithValue("@DueDate", task.DueTime);
            command.Parameters.AddWithValue("@UserId", task.UserId);
            command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", task.UpdatedAt);
            await command.ExecuteNonQueryAsync(cancellationToken);

            task = new Tasks
            {
                Id = Guid.NewGuid(),
                Title = task.Title.Trim(),
                Description = task.Description!.Trim(),
                Priority = task.Priority,
                Status = task.Status,
                DueTime = task.DueTime,
                UserId = task.UserId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
            };

            return task;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
        finally
        {
            command.Dispose();
        }
    }

    public async Task<Tasks?> UpdateTask(Tasks task, CancellationToken cancellationToken)
    {
        const string query = "UPDATE Tasks " +
            "SET Title = @Title, Description = @Description, Priority = @Priority, " +
            "Status = @Status, DueDate = @DueDate, UpdatedAt = @UpdatedAt " +
            "WHERE Id = @Id AND UserId = @UserId ";

        var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", task.Id);
        command.Parameters.AddWithValue("@UserId", task.UserId);
        command.Parameters.AddWithValue("@Title", task.Title);
        command.Parameters.AddWithValue("@Description", task.Description);
        command.Parameters.AddWithValue("@Priority", task.Priority.ToString());
        command.Parameters.AddWithValue("@Status", task.Status.ToString());
        command.Parameters.AddWithValue("@DueDate", task.DueTime);
        command.Parameters.AddWithValue("@UpdatedAt", task.UpdatedAt);

        await command.ExecuteNonQueryAsync(cancellationToken);
        await connection.CloseAsync();

        return task;
    }

    public async Task<Tasks?> DeleteTask(Guid taskId, Guid userId, CancellationToken cancellationToken)
    {
        const string query = "DELETE FROM Tasks " +
            "WHERE Id = @Id AND UserId = @UserId";

        var task = new Tasks();

        var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", taskId);
        command.Parameters.AddWithValue("@UserId", userId);

        command.ExecuteNonQuery();

        return task;
    }
}