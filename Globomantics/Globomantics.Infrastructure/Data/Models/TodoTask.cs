namespace Globomantics.Infrastructure.Data.Models;

public class TodoTask : Todo
{
    public DateTimeOffset DueDate { get; init; }

    public virtual ICollection<TodoTask> SubTasks { get; init; } = default!;
}
