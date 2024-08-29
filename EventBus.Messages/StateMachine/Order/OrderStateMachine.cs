using ApiClient.Basket.Events.CheckoutEvents;
using ApiClient.Catalog.Product.Events;
using MassTransit;

namespace EventBus.Messages.StateMachine.Basket;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public Event<BasketCheckoutMessage> BasketCheckoutEvent { get; private set; } = null!;
    public Event<ProductBalanceUpdateMessage> ProductBalanceUpdateEvent { get; private set; } = null!;

    public State Checkoutted { get; private set; } = null!;

    public State Accepted { get; private set; } = null!;

    public OrderStateMachine()
    {
        Event(() => BasketCheckoutEvent, x => x.CorrelateById(context => Guid.Parse(context.Message.BasketKey)));
        Event(() => ProductBalanceUpdateEvent, x => x.CorrelateById(context => Guid.Parse(context.Message.ReceiptNumber)));

        InstanceState(x => x.CurrentState);

        Initially(
            When(BasketCheckoutEvent)
                .Then(context =>
                {
                    context.Saga.CorrelationId = Guid.Parse(context.Message.BasketKey);
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
        
        During(Checkoutted,
            When(ProductBalanceUpdateEvent)
                .TransitionTo(Accepted));

        // DuringAny(
        //     When(BasketCheckoutEvent)
        //         .Then(context =>
        //         {
        //             context.Saga.Timestamp = context.Message.Timestamp;
        //         }));
    }
}