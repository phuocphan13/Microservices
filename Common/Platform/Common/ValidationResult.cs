namespace Platform.Common;

public interface IValidationResult<T>
{
    public T Instance { get; set; }
    public List<ValidationFailure> ValidationFailures { get; set; }
    IValidationResult<TTransformed> Transform<TTransformed>(Func<T, TTransformed> transform);
}

public class ValidationResult<T> : IValidationResult<T>
{
    public T Instance { get; set; }
    public string? EntityName { get; set; }
    public List<ValidationFailure> ValidationFailures { get; set; }

    public bool IsValid => ValidationFailures.Count == 0;

    public ValidationResult(T instance)
    {
        Instance = instance;
        EntityName = nameof(instance);
        ValidationFailures = new List<ValidationFailure>();
    }

    public ValidationResult(IValidationResult<T> source)
    {
        Instance = source.Instance;
        EntityName = nameof(source.Instance);
        ValidationFailures = new List<ValidationFailure>();
    }

    public IValidationResult<TTransformed> Transform<TTransformed>(Func<T, TTransformed> transform)
    {
        ArgumentNullException.ThrowIfNull(transform, nameof(transform));
        
        if (Instance == null)
        {
            throw new InvalidOperationException("The invalid instance has not been specified, and therefore cannot be transformed.");
        }

        ValidationResult<TTransformed> validationResult = new ValidationResult<TTransformed>(transform(Instance));
        validationResult.ValidationFailures.AddRange(ValidationFailures);
        return validationResult;
    }
}