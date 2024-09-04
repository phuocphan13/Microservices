using System.ComponentModel;
using ApiClient.Common.MessageQueue;

public enum ProcessEnum
{
    [Description(EventBusConstants.OrderProccess.Checkout)]
    Checkoutte = 1,    
    
    [Description("Accepted")]
    Accepted = 2,
    
    [Description("Retry")]
    Retry = 3,
    
    [Description("Failed")]
    Failed = 4,
}