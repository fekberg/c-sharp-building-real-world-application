namespace Globomantics.Infrastructure.Data.Models;

public class TodoTask : Todo
{
    public DateTimeOffset DueDate { get; init; }
}