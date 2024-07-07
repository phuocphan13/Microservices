using System.Runtime.Serialization;

namespace EventBus.Messages.Exceptions;

[Serializable]
public class DuplicateRegistrationException : Exception
{
    public DuplicateRegistrationException()
    {
    }

    protected DuplicateRegistrationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DuplicateRegistrationException(string? message) : base(message)
    {
    }

    public DuplicateRegistrationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}