using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBus.Messages.Entities;

public class OrderMessageDbContext : SagaDbContext
{
    public OrderMessageDbContext(DbContextOptions<OrderMessageDbContext> options) : base(options)
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
        EntityTypeBuilder<Order> registration = modelBuilder.Entity<Order>();

        registration.Property(x => x.Id);
        registration.HasKey(x => x.Id);
        
        registration.Property(x => x.UserId).HasMaxLength(64);
        registration.Property(x => x.UserName).HasMaxLength(128);
        registration.Property(x => x.ReceiptNumber).HasMaxLength(36);
        
        registration.Property(x => x.Description).HasMaxLength(256);
        registration.Property(x => x.Proccess);
        
        registration.Property(x => x.Timestamp);
        registration.Property(x => x.MemberId).HasMaxLength(64);
        registration.Property(x => x.EventId).HasMaxLength(64);

        registration.HasIndex(x => new
        {
            x.MemberId,
            x.EventId,
            x.ReceiptNumber
        }).IsUnique();
    }
}