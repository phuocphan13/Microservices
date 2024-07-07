using EventBus.Messages.StateMachine;
using EventBus.Messages.StateMachine.Basket;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBus.Messages.Entities;

public class MessageDbContext : SagaDbContext
{
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new BasketStateMap(); }
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
        EntityTypeBuilder<Basket> registration = modelBuilder.Entity<Basket>();

        registration.Property(x => x.Id);
        registration.HasKey(x => x.Id);
        
        registration.Property(x => x.UserId);
        registration.Property(x => x.UserName);
        
        registration.Property(x => x.TotalPrice);
        registration.Property(x => x.Timestamp);
        registration.Property(x => x.MemberId).HasMaxLength(64);
        registration.Property(x => x.EventId).HasMaxLength(64);

        registration.HasIndex(x => new
        {
            x.MemberId,
            x.EventId
        }).IsUnique();
    }
}