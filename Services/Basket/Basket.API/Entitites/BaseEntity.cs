namespace Basket.API.Entitites;

public class BaseEntity
{
    public string CreatedBy { get; set; } = null!;
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime LastModifiedDate { get; set; }
}