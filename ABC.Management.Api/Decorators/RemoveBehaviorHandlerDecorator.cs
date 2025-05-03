using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class RemoveBehaviorHandlerDecorator(
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<RemoveBehaviorResponseCommand, BaseResponseCommand<Behavior>>
{
    public async ValueTask<BaseResponseCommand<Behavior>> Handle(
        RemoveBehaviorResponseCommand message,
        MessageHandlerDelegate<RemoveBehaviorResponseCommand, BaseResponseCommand<Behavior>> next,
        CancellationToken cancellationToken) =>
        await ErrorValidationDecorator.Handle(
            _logger,
            message,
            next,
            cancellationToken);
    
}
