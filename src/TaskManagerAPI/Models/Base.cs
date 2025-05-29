namespace TaskManagerAPI.Models;

public abstract class Base
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; }
}