using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class RemoveAntecedentHandlerDecorator(
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<RemoveAntecedentResponseCommand, BaseResponseCommand<Antecedent>>
{
    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        RemoveAntecedentResponseCommand message,
        MessageHandlerDelegate<RemoveAntecedentResponseCommand, BaseResponseCommand<Antecedent>> next,
        CancellationToken cancellationToken)
    {

        _logger.LogTrace("Handling {CommandName} with {HandlerName}", nameof(RemoveAntecedentResponseCommand),
            nameof(RemoveAntecedentHandlerDecorator));

        return await ErrorValidationDecorator.Handle(
            _logger,
            message,
            next,
            cancellationToken);
    }
}

