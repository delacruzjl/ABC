using ABC.Management.Api.Commands;
using ABC.SharedKernel;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Extensions;

public static class BaseResponseCommandExtensions
{
    public static async Task<T?> ExecuteHandler<T>(
        this IRequest<BaseResponseCommand<T>> command,
        IMediator handler,
        IResolverContext context) where T : Entity
    {
        var response = await handler.Send(command);
        response.Errors.ForEach(context.ReportError);

        return response.Entity;
    }
}
