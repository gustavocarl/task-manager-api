using TaskManagerAPI.Models.Enums;

namespace TaskManagerAPI.Dtos;

public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Priority Priority { get; set; }

    public Status Status { get; set; }

    public DateTime DueDate { get; set; }
}