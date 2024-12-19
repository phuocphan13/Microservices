namespace EventBus.Messages.Exceptions;

[Serializable]
public class DuplicateRegistrationException : Exception
{
    public DuplicateRegistrationException()
    {
    }

    public DuplicateRegistrationException(string? message) : base(message)
    {
    }

    public DuplicateRegistrationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}