using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class RemoveConsequenceHandlerDecorator(
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<RemoveConsequenceResponseCommand, BaseResponseCommand<Consequence>>
{
    public async ValueTask<BaseResponseCommand<Consequence>> Handle(
        RemoveConsequenceResponseCommand message,
        MessageHandlerDelegate<RemoveConsequenceResponseCommand, BaseResponseCommand<Consequence>> next,
        CancellationToken cancellationToken) =>
    await ErrorValidationDecorator.Handle(
            _logger,
            message,
            next,
            cancellationToken);
    
}
