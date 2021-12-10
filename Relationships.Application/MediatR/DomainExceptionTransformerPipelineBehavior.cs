using Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions;
using MediatR;
using Relationships.Domain;
using ApplicationException = Enmeshed.BuildingBlocks.Application.Abstractions.Exceptions.ApplicationException;

namespace Relationships.Application.MediatR;

public class DomainExceptionTransformerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (DomainException ex)
        {
            throw new ApplicationException(new ApplicationError(ex.Error.Code, ex.Error.Message), ex);
        }
    }
}
