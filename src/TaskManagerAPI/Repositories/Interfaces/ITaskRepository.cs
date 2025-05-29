using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<Tasks>> GetAllTasksByUser(Guid userId, CancellationToken cancellationToken);

    Task<Tasks?> GetTaskById(Guid taskId, Guid userId, CancellationToken cancellationToken);

    Task<Tasks?> CreateTask(Tasks task, Guid userId, CancellationToken cancellationToken);

    Task<Tasks?> UpdateTask(Tasks task, CancellationToken cancellationToken);

    Task<Tasks?> DeleteTask(Guid taskId, Guid userId, CancellationToken cancellationToken);
}