namespace Globomantics.Domain;

public record TodoTask(string Title,
    DateTimeOffset DueDate, User CreatedBy) 
    : Todo(Guid.NewGuid(), Title, DateTimeOffset.UtcNow, CreatedBy);
