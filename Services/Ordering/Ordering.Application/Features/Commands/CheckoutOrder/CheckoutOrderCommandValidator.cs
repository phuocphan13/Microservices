using FluentValidation;

namespace Ordering.Application.Features.Commands.CheckoutOrder;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

        RuleFor(x => x.BasketKey)
            .NotEmpty().WithMessage("BasketKey is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{BasketKey} must not exceed 50 characters.");
    }
}
