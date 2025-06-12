using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User?>> GetAllUsers(CancellationToken cancellationToken);

    Task<User?> GetUserById(Guid userId, CancellationToken cancellationToken);

    Task<User?> CreateUser(User user, CancellationToken cancellationToken);

    Task<User?> UpdateUser(User user, CancellationToken cancellationToken);

    Task<User?> DeleteUser(Guid userId, CancellationToken cancellationToken);
}