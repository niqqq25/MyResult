using FluentValidation;

namespace MediatRWithFluentValidationPipelineBehavior;

public sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(10);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(10);
    }
}