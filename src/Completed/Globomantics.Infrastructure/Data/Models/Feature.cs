namespace Globomantics.Infrastructure.Data.Models;

public class Feature : TodoTask
{
    public string Description { get; set; } = default!;
    public string Component { get; set; } = default!;
    public int Priority { get; set; }
    
    public Guid? AssignedToId { get; set; } = default;

    public virtual User? AssignedTo { get; set; } = default;
}