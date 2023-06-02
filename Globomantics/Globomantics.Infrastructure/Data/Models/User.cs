namespace Globomantics.Infrastructure.Data.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;
}
