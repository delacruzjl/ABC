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
        IResolverContext context,
        CancellationToken cancellationToken) where T : Entity
    {
        var response = await handler.Send(command);
        response.Errors.ToList()
            .ForEach(context.ReportError);

        return response.Entity;
    }

    public static async Task<T?> ExecuteHandler<T>(
    this IRequest<BaseResponseCommand<T>> command,
    IMediator handler,
    IResolverContext context) where T : Entity =>
        await command.ExecuteHandler(handler, context, new CancellationToken());
}
