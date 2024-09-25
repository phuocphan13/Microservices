using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBus.Messages.Entities;

public class OutboxMessageDbContext : SagaDbContext
{
    public OutboxMessageDbContext(DbContextOptions<OutboxMessageDbContext> options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new OrderStateMap(); }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        MapRegistration(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    private static void MapRegistration(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Order> orderRegistration = modelBuilder.Entity<Order>();

        orderRegistration.Property(x => x.Id);
        orderRegistration.HasKey(x => x.Id);
        
        orderRegistration.Property(x => x.UserId).HasMaxLength(64);
        orderRegistration.Property(x => x.UserName).HasMaxLength(128);
        orderRegistration.Property(x => x.ReceiptNumber).HasMaxLength(36);
        
        orderRegistration.Property(x => x.Description).HasMaxLength(256);
        orderRegistration.Property(x => x.Proccess);
        
        orderRegistration.Property(x => x.Timestamp);
        orderRegistration.Property(x => x.MemberId).HasMaxLength(64);
        orderRegistration.Property(x => x.EventId).HasMaxLength(64);

        orderRegistration.HasIndex(x => new
        {
            x.MemberId,
            x.EventId,
            x.ReceiptNumber
        }).IsUnique();

        EntityTypeBuilder<Log> logRegistration = modelBuilder.Entity<Log>();

        logRegistration.Property(x => x.Id);
        logRegistration.HasKey(x => x.Id);

        logRegistration.Property(x => x.Type);

        logRegistration.HasIndex(x => new
        {
            x.EventId
        }).IsUnique();
    }
}