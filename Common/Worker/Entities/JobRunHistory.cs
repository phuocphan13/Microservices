using System.ComponentModel.DataAnnotations;
using Platform.Database.Entity.SQL;

namespace Worker.Entities;

public class JobRunHistory : BaseIdEntity
{
    public DateTime TimeStamp { get; set; }
    
    public RunStateEnum State { get; set; }
    
    public Guid JobExternalId { get; set; }

    [MaxLength(1000)]
    public string ErrorMessage { get; set; } = null!;
}