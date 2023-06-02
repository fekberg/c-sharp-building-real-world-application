namespace Globomantics.Infrastructure.Data.Models;

public abstract class Todo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public User CreatedBy { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public virtual Todo? Parent { get; set; }
}