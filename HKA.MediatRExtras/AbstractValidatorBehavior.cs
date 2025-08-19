using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HKA.MediatRExtras;

/// <summary>
/// Executes Validations
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class AbstractValidatorBehavior<TRequest, TResponse>(IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var validationRules = scope.ServiceProvider.GetService<IValidator<TRequest>>();
        validationRules?.ValidateAndThrow(request);
        return await next(cancellationToken);
    }
}
