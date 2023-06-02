namespace Globomantics.Domain;

public record Bug(string Title, 
    string Description,
    Severity Severity, 
    string AffectedVersion, 
    int AffectedUsers,
    User CreatedBy,
    User? AssigedTo,
    IEnumerable<byte[]> Images) : TodoTask(Title, DateTimeOffset.MinValue, CreatedBy);
