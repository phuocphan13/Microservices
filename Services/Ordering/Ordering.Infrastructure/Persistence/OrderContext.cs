using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Platform.Database.Entity.SQL;

namespace Ordering.Infrastructure.Persistence;

public class OrderContext : DbContext
{
    public OrderContext()
    {
        
    }

    public OrderContext(DbContextOptions options) : base(options)
    {
    }
    
    // public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    // {
    // }

    //Todo: add some field for storing Discount, Coupon
    public virtual DbSet<Order> Orders { get; set; }
    
    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<DiscountItem> Discounts { get; set; }

    public virtual DbSet<CouponItem> Coupons { get; set; }
    
    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=OrderDb;User Id=sa;Password=SwN12345678;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Server=192.168.2.11,1433;Database=OrderDb;User Id=sa;Password=SwN12345678;TrustServerCertificate=True;");
        }
        
        base.OnConfiguring(optionsBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "Lucifer ChangeTracker";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "Lucifer ChangeTracker";
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        return base.SaveChangesAsync(cancellationToken);
    }
}
