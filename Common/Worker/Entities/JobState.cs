using Platform.Database.Entity.SQL;

namespace Worker.Entities;

public class JobState : EntityBase
{
    public Guid JobExternalId { get; set; }
    
    public JobStateEnum State { get; set; }
}