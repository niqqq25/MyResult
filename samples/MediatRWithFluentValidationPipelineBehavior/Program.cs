using System.Reflection;
using FluentValidation;
using MediatR;
using MediatRWithFluentValidationPipelineBehavior;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddMediatR(c =>
    {
        c.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        c.AddOpenBehavior(typeof(FluentValidationPipelineBehavior<,>));
    })
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .BuildServiceProvider();

var sender = serviceProvider.GetRequiredService<ISender>();

var result = await sender.Send(new CreateUserRequest("Bob", "TooLongLastName"));

Console.WriteLine($"IsSuccess: {result.IsSuccess}");

if (result.IsSuccess)
{
    Console.WriteLine($"User id: {result.Value}");
}
else
{
    Console.WriteLine($"Error code: {result.Error.Code}");
    Console.WriteLine($"Error description: {result.Error.Description}");
}
