using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Platform.Configurations.Options;

namespace Worker.Entities;

public class WorkerContext : DbContext
{
    private readonly WorkerOptions _workerOptions;
    
    public WorkerContext(IOptions<WorkerOptions> workerOptions)
    {
        _workerOptions = workerOptions.Value;
    }

    public WorkerContext(DbContextOptions options, IOptions<WorkerOptions> workerOptions) : base(options)
    {
        _workerOptions = workerOptions.Value;
    }

    public virtual DbSet<Job> Jobs { get; set; } = null!;
    public virtual DbSet<JobState> JobStates { get; set; } = null!;
    public virtual DbSet<JobRunHistory> JobRunHistories { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=OrderDb;User Id=sa;Password=SwN12345678;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer(_workerOptions.WorkerConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }
}