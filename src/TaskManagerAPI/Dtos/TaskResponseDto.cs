using TaskManagerAPI.Models.Enums;

namespace TaskManagerAPI.Dtos;

public record TaskResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Priority Priority { get; set; }

    public Status Status { get; set; }
}
