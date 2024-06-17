using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContext : DbContext
{
    public OrderContext()
    {
        
    }

    public OrderContext(DbContextOptions options) : base(options)
    {
    }
    
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    //Todo: add some field for storing Discount, Coupon
    public virtual DbSet<Order> Orders { get; set; }
    
    public virtual DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=OrderDb;User Id=sa;Password=SwN12345678;TrustServerCertificate=True;");
            // optionsBuilder.UseSqlServer(_configuration["Configuration:ConnectionString"]);
            //optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=Authentication;Trusted_Connection=True;");
        }
        
        base.OnConfiguring(optionsBuilder);
    }

    // public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    // {
    //     foreach (var entry in ChangeTracker.Entries<EntityBase>())
    //     {
    //         switch (entry.State)
    //         {
    //             case EntityState.Added:
    //                 entry.Entity.CreatedDate = DateTime.Now;
    //                 entry.Entity.CreatedBy = "Lucifer";
    //                 break;
    //             case EntityState.Modified:
    //                 entry.Entity.LastModifiedDate = DateTime.Now;
    //                 entry.Entity.LastModifiedBy = "Lucifer";
    //                 break;
    //             case EntityState.Detached:
    //                 break;
    //             case EntityState.Unchanged:
    //                 break;
    //             case EntityState.Deleted:
    //                 break;
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //     }
    //
    //     return base.SaveChangesAsync(cancellationToken);
    // }
}
