namespace TaskManagerAPI.Dtos;

public record RegisterTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid UserId { get; set; }
}