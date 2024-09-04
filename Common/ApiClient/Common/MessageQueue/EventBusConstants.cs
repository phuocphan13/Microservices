namespace ApiClient.Common.MessageQueue;

public static class EventBusConstants
{
    public const string BasketCheckoutQueue = "basketcheckout-queue";
    
    
    public static class OrderProccess
    {
        public const  string Checkout = "Checkout";
        public const  string Accepted = "Accepted";
        public const string Retry = "Retry";
        public const string Failed = "Failed";
    }
}