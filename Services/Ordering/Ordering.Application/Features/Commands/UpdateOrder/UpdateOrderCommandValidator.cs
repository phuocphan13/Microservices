using FluentValidation;

namespace Ordering.Application.Features.Commands.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("{UserName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required.");

        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required.")
            .GreaterThan(0).WithMessage("{TOtalPrice} shoulbe greater than 0.");
    }
}