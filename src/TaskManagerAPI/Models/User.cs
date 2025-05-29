using TaskManagerAPI.Models.Enums;

namespace TaskManagerAPI.Models;

public class User : Base
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.User;
}