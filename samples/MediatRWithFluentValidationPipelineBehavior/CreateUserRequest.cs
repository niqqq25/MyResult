using MediatR;
using MyResult;

namespace MediatRWithFluentValidationPipelineBehavior;

public sealed record CreateUserRequest(string FirstName, string LastName) : IRequest<Result<Guid>>;