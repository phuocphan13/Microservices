using ApiClient.Basket.Events.CheckoutEvents;
using MassTransit;

namespace EventBus.Messages.StateMachine;

public class BasketStateMachine : MassTransitStateMachine<BasketStateInstance>
{
    public Event<BasketCheckoutMessage> BasketCheckoutEvent { get; private set; } = null!;

    public State Checkoutted { get; private set; } = null!;

    public BasketStateMachine()
    {
        Event(() => BasketCheckoutEvent, x => x.CorrelateById(context => Guid.Parse(context.Message.UserId)));

        InstanceState(x => x.CurrentState);

        // Initially(
        //     When(BasketCheckoutEvent)
        //         .Then(context =>
        //         {
        //             context.Saga.UserId = context.Message.UserId;
        //             context.Saga.TotalPrice = context.Message.TotalPrice;;
        //
        //             context.Saga.CreatedDate = DateTime.UtcNow;
        //             context.Saga.Timestamp = DateTime.UtcNow;
        //         })
        //         .TransitionTo(Checkoutted));
        
        // During(Checkoutted,
        //     Ignore(BasketCheckoutEvent));
        
        // DuringAny(
        //     When(BasketCheckoutEvent)
        //         .Then(context =>
        //         {
        //             context.Saga.Timestamp = context.Message.Timestamp;
        //         }));
    }
}