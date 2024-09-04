namespace EventBus.Messages.StateMachine.Basket;

public static class OrderConstants
{
    public static class OrderState
    {
        public const string Checkoutted = "Checkoutted";

        public const string Accepted = "Accepted";
        
        public const string Failed = "Failed";
    }
}