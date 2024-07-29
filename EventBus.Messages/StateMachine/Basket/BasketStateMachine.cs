using ApiClient.Basket.Events.CheckoutEvents;
using MassTransit;

namespace EventBus.Messages.StateMachine.Basket;

public class BasketStateMachine : MassTransitStateMachine<BasketState>
{
    public Event<BasketCheckoutMessage> BasketCheckoutEvent { get; private set; } = null!;

    public State Checkoutted { get; private set; } = null!;

    public State Accepted { get; private set; } = null!;

    public BasketStateMachine()
    {
        Event(() => BasketCheckoutEvent, x => x.CorrelateById(context => Guid.Parse(context.Message.UserId)));

        InstanceState(x => x.CurrentState);

        Initially(
            When(BasketCheckoutEvent)
                .Then(context =>
                {
                    context.Saga.CorrelationId = Guid.Parse(context.Message.UserId);
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.UserName = context.Message.UserName;
                    context.Saga.TotalPrice = context.Message.TotalPrice;

                    context.Saga.EventId = context.Message.EventId;
                    context.Saga.MemberId = context.Message.MemberId;

                    context.Saga.CreatedDate = DateTime.UtcNow;
                    context.Saga.Timestamp = DateTime.UtcNow;
                })
                .TransitionTo(Checkoutted));

        During(Checkoutted,
            When(BasketCheckoutEvent)
                .Then(context =>
                {
                    // Logic to send the message if it hasn't been sent
                    // Remember to set the flag to true after sending
                }));

        During(Checkoutted,
            Ignore(BasketCheckoutEvent));

        // DuringAny(
        //     When(BasketCheckoutEvent)
        //         .Then(context =>
        //         {
        //             context.Saga.Timestamp = context.Message.Timestamp;
        //         }));
    }
}