using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBus.Messages.StateMachine.Logging;

public class LogStateMap : SagaClassMap<LogState>
{
    protected override void Configure(EntityTypeBuilder<LogState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CorrelationId);
        entity.Property(x => x.Type);
        entity.Property(x => x.Text);

        entity.Property(x => x.EventId);
    }
}