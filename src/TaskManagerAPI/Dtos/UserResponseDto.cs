using TaskManagerAPI.Models.Enums;

namespace TaskManagerAPI.Dtos;

public record UserResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Role Role { get; set; }
}