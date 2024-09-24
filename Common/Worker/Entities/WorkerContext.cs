using Microsoft.EntityFrameworkCore;

namespace Worker.Entities;

public class WorkerContext : DbContext
{
    public WorkerContext()
    {

    }

    public WorkerContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Job> Jobs { get; set; } = null!;
    public virtual DbSet<JobState> JobStates { get; set; } = null!;
    public virtual DbSet<JobRunHistory> JobRunHistories { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1436;Database=WorkerDb;User Id=sa;Password=SwN12345678;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer("Server=192.168.2.11,1436;Database=WorkerDb;User Id=sa;Password=SwN12345678;TrustServerCertificate=True;");
        }

        base.OnConfiguring(optionsBuilder);
    }
}