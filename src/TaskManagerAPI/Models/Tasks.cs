using TaskManagerAPI.Models.Enums;

namespace TaskManagerAPI.Models;

public sealed class Tasks : Base
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Low;

    public Status Status { get; set; } = Status.ToDo;

    public DateTime? DueTime { get; set; }

    public Guid UserId { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}