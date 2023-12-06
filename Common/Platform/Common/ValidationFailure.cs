namespace Platform.Common;

public class ValidationFailure
{
    public string PropertyName { get; set; }
    public string Message { get; set; }
    
    public ValidationFailure(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }
}