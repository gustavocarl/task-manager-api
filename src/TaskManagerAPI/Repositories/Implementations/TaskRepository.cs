using Microsoft.Data.SqlClient;
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

    public Task<Tasks?> GetTaskById(Guid taskId, Guid userId, CancellationToken cancellationToken)
    {
        const string query = "SELECT Id, Title, [Description], Priority, [Status], DueDate, " +
            "UserId, CreatedAt, UpdatedAt " +
            "FROM Tasks " +
            "WHERE Id = @id AND UserId = @userId";

        return null;
    }

    public Task<Tasks?> CreateTask(Tasks task, Guid userId, CancellationToken cancellationToken)
    {
        const string query = "INSERT INTO Tasks ( Title, Description, Priority, " +
            "Status, DueDate, UserId, CreatedAt, UpdatedAt " +
            " ) VALUES (   " +
            "@Title, @Description, @Priority, " +
            "@Status, @DueDate, @UserId, @CreatedAt, @UpdatedAt " +
            " )";
        throw new NotImplementedException();
    }

    public Task<Tasks?> UpdateTask(Tasks task, CancellationToken cancellationToken)
    {
        const string query = "UPDATE Tasks " +
            "SET Title = @Title, Description = @Description, Priority = @Priority, " +
            "Status = @Status, DueDate = @DueDate, UpdatedAt = @UpdatedAt " +
            "WHERE UserId = @UserId AND Id = @Id";

        throw new NotImplementedException();
    }

    public Task<Tasks?> DeleteTask(Guid taskId, Guid userId, CancellationToken cancellationToken)
    {
        const string query = "DELETE FROM Tasks " +
            "WHERE Id = @Id AND UserId = @UserId";

        throw new NotImplementedException();
    }
}