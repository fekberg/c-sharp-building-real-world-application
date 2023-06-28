namespace Globomantics.Domain;

public record Bug(string Title,
    string Description,
    Severity Severity,
    string AffectedVersion,
    int AffectedUsers,
    User CreatedBy,
    User? AssignedTo,
    IEnumerable<byte[]> Images) : TodoTask(Title, DateTimeOffset.MinValue, CreatedBy);
