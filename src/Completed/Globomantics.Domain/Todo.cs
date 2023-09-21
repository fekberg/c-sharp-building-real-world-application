namespace Globomantics.Domain;

public abstract record Todo(Guid Id,
    string Title,
    DateTimeOffset CreatedDate,
    User CreatedBy,
    bool IsCompleted = false,
    bool IsDeleted = false)
{
    public Todo? Parent { get; init; }
}