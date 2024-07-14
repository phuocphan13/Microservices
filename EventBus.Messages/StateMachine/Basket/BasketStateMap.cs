using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBus.Messages.StateMachine.Basket;

public class BasketStateMap : SagaClassMap<BasketState>
{
    protected override void Configure(EntityTypeBuilder<BasketState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CorrelationId);
        entity.Property(x => x.UserId);
        entity.Property(x => x.UserName);
        entity.Property(x => x.CurrentState);
        
        // entity.Property(x => x.Version);
        entity.Property(x => x.TotalPrice);
        entity.Property(x => x.CreatedDate);
        entity.Property(x => x.Timestamp);
        
        entity.Property(x => x.EventId);
        entity.Property(x => x.MemberId);
    }
}