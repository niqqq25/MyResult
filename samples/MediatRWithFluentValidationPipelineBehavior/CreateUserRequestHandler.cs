using MediatR;
using MyResult;

namespace MediatRWithFluentValidationPipelineBehavior;

public sealed class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, Result<Guid>>
{
    public Task<Result<Guid>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result<Guid>.Ok(Guid.NewGuid()));
    }
}