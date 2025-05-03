using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class RemoveChildHandlerDecorator(
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<RemoveChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        RemoveChildResponseCommand message,
        MessageHandlerDelegate<RemoveChildResponseCommand, BaseResponseCommand<Child>> next,
        CancellationToken cancellationToken) =>
    await ErrorValidationDecorator.Handle(
            _logger,
            message,
            next,
            cancellationToken);
    
}
