namespace Catalog.API.Models.Cached;

public class BaseCachedModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public bool HasChange { get; set; }
    public DateTime LastUpdated { get; set; }
}