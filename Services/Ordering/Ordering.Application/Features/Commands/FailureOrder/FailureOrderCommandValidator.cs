using FluentValidation;

namespace Ordering.Application.Features.Commands.FailureOrder;

public class FailureOrderCommandValidator : AbstractValidator<FailureOrderCommand>
{
    public FailureOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

        RuleFor(x => x.ReceiptNumber)
            .NotEmpty().WithMessage("ReceiptNumber is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{ReceiptNumber} must not exceed 50 characters.");
    }
}