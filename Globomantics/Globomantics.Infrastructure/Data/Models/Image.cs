namespace Globomantics.Infrastructure.Data.Models;

public class Image
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ImageData { get; set; } = default!;
}
